using FluentValidation;

namespace MenuDigital.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(150).WithMessage("O nome do produto não pode exceder 150 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("A descrição não pode exceder 1000 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(2048).WithMessage("A URL da imagem está muito longa.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("A ordem de exibição não pode ser negativa.");
    }
}
