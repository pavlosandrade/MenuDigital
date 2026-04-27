using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    int DisplayOrder) : IRequest<Result<Guid>>;
