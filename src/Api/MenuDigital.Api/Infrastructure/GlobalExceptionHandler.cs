using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Infrastructure;

/// <summary>
/// Interceptador Global de Exceções.
/// Captura qualquer erro não tratado na aplicação e devolve um ProblemDetails formatado,
/// blindando a API para não vazar a stack trace para o cliente.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Logamos o erro internamente para os desenvolvedores
        _logger.LogError(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);

        // 2. Montamos a resposta bonitinha (padrão RFC 7807) para o Front-end
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Erro Interno do Servidor",
            Detail = "Um erro inesperado ocorreu na nossa infraestrutura. Por favor, tente novamente em instantes.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Instance = httpContext.Request.Path
        };

        // NOTA DE ARQUITETURA: 
        // Como estamos usando o Result Pattern (via fluent validation e retornos do MediatR),
        // exceções de negócio (Bad Requests) não vão cair aqui.
        // O que cai aqui é erro feio mesmo: NullReference, Banco fora do ar, etc.

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        // 3. Escrevemos a resposta JSON
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Retorna true para sinalizar que nós lidamos com o erro, o .NET não precisa fazer mais nada.
    }
}
