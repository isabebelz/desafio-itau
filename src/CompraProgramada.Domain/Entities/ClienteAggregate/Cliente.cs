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

            var cpfNormalizado = NormalizarCpf(cpf);

            if (!ValidaCPF(cpfNormalizado))
                throw new DomainException("CPF inválido.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email é obrigatório.");

            if (valorAporteMensal < valorAporteMinimo)
                throw new DomainException($"Valor mínimo de aporte é R$ {valorAporteMinimo}.");

            Nome = nome;
            CPF = cpfNormalizado;
            Email = email;
            ValorAporteMensal = valorAporteMensal;
            Ativo = true;
            DataAdesao = DateTime.UtcNow;
        }

        public static bool ValidaCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            if (!cpf.All(char.IsDigit))
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            var resto = soma % 11;
            var primeiroDigito = resto < 2 ? 0 : 11 - resto;

            if ((cpf[9] - '0') != primeiroDigito)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            resto = soma % 11;
            var segundoDigito = resto < 2 ? 0 : 11 - resto;

            if ((cpf[10] - '0') != segundoDigito)
                return false;

            return true;
        }

        private static string NormalizarCpf(string cpf)
        {
            return cpf.Trim().Replace(".", "").Replace("-", "");
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

        public void Reativar()
        {
            if (Ativo)
                throw new DomainException("Cliente já está ativo.");

            Ativo = true;
            DataSaida = null;
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
        public decimal CalcularValorParcela(int parcelasMes)
        {
            return Math.Round(ValorAporteMensal / parcelasMes, 2);
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
