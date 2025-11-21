using FluentValidation;
using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Validators;

public class CriarCheckInDtoValidator : AbstractValidator<CriarCheckInDto>
{
    public CriarCheckInDtoValidator()
    {
        RuleFor(x => x.ColaboradorId)
            .GreaterThan(0).WithMessage("O ID do colaborador deve ser maior que zero.");

        RuleFor(x => x.Humor)
            .InclusiveBetween(1, 5).WithMessage("O humor deve estar entre 1 e 5.");

        RuleFor(x => x.NivelEstresse)
            .InclusiveBetween(1, 5).WithMessage("O nível de estresse deve estar entre 1 e 5.");

        RuleFor(x => x.QualidadeSono)
            .InclusiveBetween(1, 5).WithMessage("A qualidade do sono deve estar entre 1 e 5.");
    }
}

