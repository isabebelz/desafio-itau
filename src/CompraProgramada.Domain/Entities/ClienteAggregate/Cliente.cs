using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.ClienteAggregate
{
    /// <summary>
    /// Representa um cliente que adere ao produto de compra programada de ações.
    /// Ao aderir, o sistema cria automaticamente uma Conta Gráfica (filhote) vinculada.
    /// O valor mensal de aporte é dividido em 3 parcelas (dias 5, 15, 25 do mês).
    /// </summary>
    public class Cliente : Entity
    {
        public const decimal VALOR_APORTE_MINIMO = 100.00m;
        public const int PARCELAS_POR_MES = 3;

        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
        public decimal ValorAporteMensal { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataAdesao { get; private set; }
        public DateTime? DataSaida { get; private set; }

        public ContaGrafica? ContaGrafica { get; private set; }

        private Cliente()
        {
            Nome = null!;
            CPF = null!;
            Email = null!;
        }

        /// <summary>
        /// Adesão ao produto.
        /// Valida dados obrigatórios, valor mínimo e cria o cliente ativo.
        /// </summary>
        public Cliente(string nome, string cpf, string email,
                       decimal valorAporteMensal, decimal valorAporteMinimo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                throw new DomainException("CPF deve conter 11 dígitos.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email é obrigatório.");

            if (valorAporteMensal < valorAporteMinimo)
                throw new DomainException($"Valor mínimo de aporte é R$ {valorAporteMinimo}.");

            Nome = nome;
            CPF = cpf;
            Email = email;
            ValorAporteMensal = valorAporteMensal;
            Ativo = true;
            DataAdesao = DateTime.UtcNow;
        }

        /// <summary>
        /// Ao aderir, cria automaticamente a conta gráfica vinculada.
        /// </summary>
        public ContaGrafica CriarContaGrafica()
        {
            if (ContaGrafica is not null)
                throw new DomainException("Cliente já possui conta gráfica.");

            ContaGrafica = new ContaGrafica(Id);
            return ContaGrafica;
        }

        /// <summary>
        /// Saída do produto.
        /// Marca como inativo e registra a data de saída.
        /// A posição existente na custódia filhote é mantida.
        /// </summary>
        public void Sair()
        {
            if (!Ativo)
                throw new DomainException("Cliente já está inativo.");

            Ativo = false;
            DataSaida = DateTime.UtcNow;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Alteração do valor mensal de aporte.
        /// O novo valor será usado a partir da próxima data de compra.
        /// </summary>
        public void AlterarValorAporte(decimal novoValor, decimal valorAporteMinimo)
        {
            if (novoValor < valorAporteMinimo)
                throw new DomainException($"Valor mínimo de aporte é R$ {valorAporteMinimo}.");

            if (!Ativo)
                throw new DomainException("Cliente inativo não pode alterar valor de aporte.");

            ValorAporteMensal = novoValor;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Calcula o valor de 1/3 do aporte mensal (parcela por data de compra).
        /// O valor é dividido em 3 parcelas: dias 5, 15 e 25.
        /// </summary>
        public decimal CalcularValorParcela()
        {
            return Math.Round(ValorAporteMensal / PARCELAS_POR_MES, 2);
        }

        /// <summary>
        /// Verifica se o cliente pode participar do agrupamento de compras.
        /// </summary>
        public bool PodeParticiparCompra()
        {
            return Ativo;
        }
    }
}
