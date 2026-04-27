using FluentValidation;

namespace MenuDigital.Application.Tenants.Commands.CreateTenant;

public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do restaurante é obrigatório.")
            .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("O identificador da URL (slug) é obrigatório.")
            .Matches("^[a-z0-9-]+$").WithMessage("O slug deve conter apenas letras minúsculas, números e hífens para ser uma URL válida.")
            .MaximumLength(100).WithMessage("O slug não pode exceder 100 caracteres.");
    }
}
