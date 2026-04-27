using MediatR;
using MenuDigital.Application.Products.Commands.CreateProduct;
using MenuDigital.Application.Products.Commands.AddProductAddon;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Endpoints;

public static class ProductsEndpoint
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
                       .WithTags("Products");

        group.MapPost("", async (
            [FromBody] CreateProductCommand command, 
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Message });
            }

            return Results.Created($"/api/products/{result.Value}", new { id = result.Value });
        });

        // 📝 [POST] /api/products/{id}/addons - Adicionar um Extra ao Produto
        group.MapPost("/{id:guid}/addons", async (
            [FromRoute] Guid id,
            [FromBody] AddProductAddonRequest request, 
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AddProductAddonCommand(
                id, 
                request.Name, 
                request.Description, 
                request.Price, 
                request.IsRequired, 
                request.MaxSelection, 
                request.DisplayOrder);

            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Message });
            }

            return Results.NoContent(); // 204 No Content é o padrão para "Atualizei seu objeto com sucesso, não há ID novo pra devolver".
        });
    }
}

// DTO para não precisarmos mandar o ProductId no Body (ele vem da Rota {id})
public record AddProductAddonRequest(string Name, string? Description, decimal Price, bool IsRequired, int MaxSelection, int DisplayOrder);
