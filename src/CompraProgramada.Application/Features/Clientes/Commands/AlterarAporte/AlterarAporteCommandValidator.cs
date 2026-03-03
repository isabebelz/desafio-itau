using FluentValidation;

namespace CompraProgramada.Application.Features.Clientes.Commands.AlterarAporte
{
    public class AlterarAporteCommandValidator : AbstractValidator<AlterarAporteCommand>
    {
        public AlterarAporteCommandValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("ClienteId inválido.");

            RuleFor(x => x.Aporte.NovoValor)
                .GreaterThan(0).WithMessage("Valor de aporte deve ser maior que zero.");
        }
    }
}