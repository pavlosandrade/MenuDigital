using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Menus.Queries.GetMenuCatalog;

public sealed class GetMenuCatalogQueryHandler : IRequestHandler<GetMenuCatalogQuery, Result<MenuCatalogDto>>
{
    private readonly IMenuDigitalDbContext _context;

    public GetMenuCatalogQueryHandler(IMenuDigitalDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MenuCatalogDto>> Handle(GetMenuCatalogQuery request, CancellationToken cancellationToken)
    {
        // PERFORMANCE: Como é apenas leitura, usamos .AsNoTracking()
        // Isso desliga o Change Tracker do EF Core, deixando a query até 40% mais rápida
        // O EF Core irá buscar tudo atrelado ao TenantId automaticamente via Global Query Filter.
        var menu = await _context.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.MenuId, cancellationToken);

        if (menu == null)
            return Result.Failure<MenuCatalogDto>(new Error("Menu.NotFound", "Cardápio não encontrado."));

        // Buscamos as categorias com os seus produtos e addons num único hit no banco de dados.
        // O EF Core vai usar "Split Queries" se necessário ou um baita JOIN, mas nos trará tudo montado.
        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.MenuId == request.MenuId && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DisplayOrder = c.DisplayOrder,
                Products = _context.Products
                    .Where(p => p.CategoryId == c.Id && p.IsAvailable)
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        DisplayOrder = p.DisplayOrder,
                        IsAvailable = p.IsAvailable,
                        Addons = p.Addons.OrderBy(a => a.DisplayOrder).Select(a => new ProductAddonDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Price = a.Price,
                            IsRequired = a.IsRequired,
                            MaxSelection = a.MaxSelection,
                            DisplayOrder = a.DisplayOrder
                        }).ToList()
                    }).ToList()
            })
            .ToListAsync(cancellationToken);

        var catalogDto = new MenuCatalogDto
        {
            Id = menu.Id,
            Name = menu.Name,
            Categories = categories
        };

        return Result.Success(catalogDto);
    }
}
