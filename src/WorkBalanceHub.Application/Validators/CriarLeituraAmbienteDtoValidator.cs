using FluentValidation;
using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Validators;

public class CriarLeituraAmbienteDtoValidator : AbstractValidator<CriarLeituraAmbienteDto>
{
    public CriarLeituraAmbienteDtoValidator()
    {
        RuleFor(x => x.EstacaoTrabalhoId)
            .GreaterThan(0).WithMessage("O ID da estação de trabalho deve ser maior que zero.");

        RuleFor(x => x.Temperatura)
            .InclusiveBetween(-50, 60).WithMessage("A temperatura deve estar entre -50°C e 60°C.");

        RuleFor(x => x.NivelRuido)
            .InclusiveBetween(0, 150).WithMessage("O nível de ruído deve estar entre 0 e 150 dB.");

        RuleFor(x => x.Luminosidade)
            .InclusiveBetween(0, 100000).WithMessage("A luminosidade deve estar entre 0 e 100.000 lux.");
    }
}

