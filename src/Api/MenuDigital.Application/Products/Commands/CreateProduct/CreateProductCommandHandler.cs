using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Entities.Products;
using MenuDigital.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IMenuDigitalDbContext _context;
    private readonly ITenantService _tenantService;

    public CreateProductCommandHandler(IMenuDigitalDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantService.GetTenantId();

        if (tenantId == Guid.Empty)
        {
            return Result.Failure<Guid>(new Error("Product.TenantRequired", "Acesso Negado: Identificação do restaurante ausente."));
        }

        // SEGURANÇA (IDOR Prevention):
        // Garante que a categoria informada existe e realmente pertence a esse Restaurante
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure<Guid>(new Error(
                "Product.CategoryNotFound", 
                "A categoria informada não foi encontrada ou não pertence a este restaurante."));
        }

        var productResult = Product.Create(
            tenantId, 
            request.CategoryId, 
            request.Name, 
            request.Description, 
            request.Price, 
            request.ImageUrl, 
            request.DisplayOrder);

        if (productResult.IsFailure)
        {
            return Result.Failure<Guid>(productResult.Error);
        }

        _context.Products.Add(productResult.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(productResult.Value.Id);
    }
}
