using FluentValidation;
using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Behaviors;

/// <summary>
/// Pipeline interceptor do MediatR. Ele intercepta TODA requisição que entra no sistema.
/// Se houver validadores (FluentValidation) para a request, ele os executa.
/// Se houver erro, devolve um Result.Failure IMEDIATAMENTE, poupando processamento e o acesso ao Handler.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(); // Não tem validação pra esse comando? Pode seguir em frente.
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            // Em cenários avançados podemos retornar todos os erros (lista), 
            // mas aqui vamos pegar o primeiro para respeitar o padrão Error base que montamos.
            var firstFailure = failures.First();
            var error = new Error("Validation", firstFailure.ErrorMessage);

            // Usa reflection para invocar o Result.Failure genérico dependendo se for Result ou Result<T>
            return CreateValidationResult<TResponse>(error);
        }

        return await next(); // Validação passou com sucesso, execute o Handler!
    }

    private static TResult CreateValidationResult<TResult>(Error error)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResultWith(error) as TResult)!;
        }

        // Recupera o Result.Failure<T>(Error) dinamicamente
        object validationResult = typeof(Result)
            .GetMethods()
            .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
            .MakeGenericMethod(typeof(TResult).GenericTypeArguments[0])
            .Invoke(null, new object[] { error })!;

        return (TResult)validationResult;
    }

    private static Result ValidationResultWith(Error error) => Result.Failure(error);
}
