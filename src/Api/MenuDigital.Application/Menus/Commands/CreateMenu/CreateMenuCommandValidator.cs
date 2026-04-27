using FluentValidation;

namespace MenuDigital.Application.Menus.Commands.CreateMenu;

/// <summary>
/// Validações de entrada antes do comando chegar no Handler.
/// </summary>
public sealed class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do cardápio é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("O horário de início deve ser menor que o horário de término.");
    }
}
