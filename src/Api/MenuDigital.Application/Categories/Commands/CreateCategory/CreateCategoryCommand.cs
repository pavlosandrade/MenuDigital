using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Categories.Commands.CreateCategory;

/// <summary>
/// Comando (DTO de Entrada) para criar uma nova categoria em um cardápio.
/// </summary>
public sealed record CreateCategoryCommand(
    Guid MenuId,
    string Name,
    string? Description,
    int DisplayOrder) : IRequest<Result<Guid>>;
