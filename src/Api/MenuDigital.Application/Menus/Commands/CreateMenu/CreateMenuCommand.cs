using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Menus.Commands.CreateMenu;

/// <summary>
/// Comando (DTO de Entrada) para criar um novo cardápio.
/// </summary>
public sealed record CreateMenuCommand(
    string Name, 
    TimeSpan StartTime, 
    TimeSpan EndTime) : IRequest<Result<Guid>>;
