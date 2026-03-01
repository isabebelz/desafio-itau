using FluentValidation;
using CompraProgramada.Application.Features.Acoes.Commands.CadastrarAcao;

public class AdicionarAcaoCommandValidator : AbstractValidator<CadastrarAcaoCommand>
{
    public AdicionarAcaoCommandValidator()
    {
        RuleFor(x => x.acao.Codigo)
            .NotEmpty().WithMessage("Código da ação é obrigatório.")
            .MaximumLength(10).WithMessage("Código da ação deve ter até 10 caracteres.")
            .Matches("^[A-Z0-9]+$").WithMessage("O código deve ter apenas letras maiúsculas e números.")
            .Must(codigo => !codigo.EndsWith("F")).WithMessage("Não é permitido cadastrar tickers fracionários diretamente (sufixo F).");

        RuleFor(x => x.acao.NomeEmpresa)
            .NotEmpty().WithMessage("Nome da empresa é obrigatório.")
            .MaximumLength(100).WithMessage("Nome máximo de 100 caracteres.");

        RuleFor(x => x.acao.Preco)
            .GreaterThan(0).WithMessage("O preço da ação deve ser maior que zero.");
    }
}