using MediatR;
using MenuDigital.Application.Tenants.Commands.CreateTenant;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Endpoints;

public static class TenantsEndpoint
{
    public static void MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tenants").WithTags("Tenants");

        group.MapPost("", async ([FromBody] CreateTenantCommand command, ISender sender) =>
        {
            // ISender é o disparador do MediatR
            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                // Aqui estamos mapeando o nosso Error padrão para um response BadRequest 400
                return Results.BadRequest(new { 
                    error = result.Error.Code, 
                    message = result.Error.Message 
                });
            }

            // Status 201 Created com a URL para possível GET do recurso no futuro
            return Results.Created($"/api/tenants/{result.Value}", new { Id = result.Value });
        });
    }
}
