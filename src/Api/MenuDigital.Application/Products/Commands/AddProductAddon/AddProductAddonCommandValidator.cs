using FluentValidation;

namespace MenuDigital.Application.Products.Commands.AddProductAddon;

public sealed class AddProductAddonCommandValidator : AbstractValidator<AddProductAddonCommand>
{
    public AddProductAddonCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("O ID do produto é obrigatório.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("O nome do adicional é obrigatório.").MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");
        RuleFor(x => x.MaxSelection)
            .GreaterThanOrEqualTo(1)
            .When(x => !x.IsRequired)
            .WithMessage("A seleção máxima deve ser ao menos 1 se não for um item obrigatório.");
    }
}
