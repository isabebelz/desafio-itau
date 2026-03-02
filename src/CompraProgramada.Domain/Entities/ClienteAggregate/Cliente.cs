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

        public Cliente(string nome, string cpf, string email, decimal valorAporteMensal)
        {
            Nome = nome;
            CPF = cpf;
            Email = email;
            ValorAporteMensal = valorAporteMensal;
            Ativo = true;
            DataAdesao = DateTime.UtcNow;
        }
    }
}
