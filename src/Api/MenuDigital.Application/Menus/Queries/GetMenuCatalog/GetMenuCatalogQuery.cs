using MediatR;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Menus.Queries.GetMenuCatalog;

// Uma Query (Consulta) solicita dados e NUNCA altera o estado do sistema.
// O retorno é sempre um Result contendo o DTO de leitura.
public record GetMenuCatalogQuery(Guid MenuId) : IRequest<Result<MenuCatalogDto>>;
