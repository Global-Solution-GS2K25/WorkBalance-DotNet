using FluentValidation;
using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Validators;

public class CriarColaboradorDtoValidator : AbstractValidator<CriarColaboradorDto>
{
    public CriarColaboradorDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail deve ter um formato válido.")
            .MaximumLength(255).WithMessage("O e-mail não pode exceder 255 caracteres.");

        RuleFor(x => x.Cargo)
            .NotEmpty().WithMessage("O cargo é obrigatório.")
            .MaximumLength(100).WithMessage("O cargo não pode exceder 100 caracteres.");

        RuleFor(x => x.EquipeId)
            .GreaterThan(0).WithMessage("O ID da equipe deve ser maior que zero.");
    }
}

