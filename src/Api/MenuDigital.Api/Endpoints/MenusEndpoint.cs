using MediatR;
using MenuDigital.Application.Menus.Commands.CreateMenu;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Endpoints;

public static class MenusEndpoint
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/menus")
                       .WithTags("Menus");

        // 📝 [POST] /api/menus - Criar um novo cardápio
        // Lembrete Sênior: O Minimal APIs usa [FromBody] para mapear o JSON no Record Command.
        group.MapPost("", async (
            [FromBody] CreateMenuCommand command, 
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            // O MediatR se encarrega de chamar o Validator e depois o Handler
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                // Se cair aqui, ou falhou no FluentValidation, ou na regra de negócio (ex: faltou TenantId)
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Message });
            }

            // Status 201 (Created) é a convenção REST correta para criação.
            return Results.Created($"/api/menus/{result.Value}", new { id = result.Value });
        });
    }
}
