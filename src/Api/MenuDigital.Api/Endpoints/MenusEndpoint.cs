using MediatR;
using MenuDigital.Application.Menus.Commands.CreateMenu;
using MenuDigital.Application.Menus.Queries.GetMenuCatalog;
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
        // 📝 [GET] /api/menus/{id}/catalog - Retorna o cardápio completo com categorias, produtos e adicionais
        group.MapGet("/{id:guid}/catalog", async (
            [FromRoute] Guid id,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMenuCatalogQuery(id);
            var result = await mediator.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return Results.NotFound(new { error = result.Error.Code, message = result.Error.Message });
            }

            return Results.Ok(result.Value);
        })
        .WithName("GetMenuCatalog")
        .WithDescription("Retorna a árvore completa do cardápio pronto para o cliente.")
        .Produces<MenuCatalogDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
