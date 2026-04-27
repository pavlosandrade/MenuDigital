using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Products.Commands.AddProductAddon;

public sealed record AddProductAddonCommand(
    Guid ProductId,
    string Name,
    string? Description,
    decimal Price,
    bool IsRequired,
    int MaxSelection,
    int DisplayOrder) : IRequest<Result>;
