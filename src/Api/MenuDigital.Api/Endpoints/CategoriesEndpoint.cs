using MediatR;
using MenuDigital.Application.Categories.Commands.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Api.Endpoints;

public static class CategoriesEndpoint
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
                       .WithTags("Categories");

        group.MapPost("", async (
            [FromBody] CreateCategoryCommand command, 
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(new { error = result.Error.Code, message = result.Error.Message });
            }

            return Results.Created($"/api/categories/{result.Value}", new { id = result.Value });
        });
    }
}
