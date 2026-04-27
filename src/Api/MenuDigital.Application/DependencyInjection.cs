using FluentValidation;
using MenuDigital.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MenuDigital.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Registra o MediatR e todos os seus Handlers
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);

            // Adiciona a nossa barreira/funil de Validação como um Pipeline global
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Registra o FluentValidation automaticamente para buscar todos os validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
