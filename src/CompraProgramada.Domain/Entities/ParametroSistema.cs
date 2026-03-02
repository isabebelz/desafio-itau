using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities
{
    /// <summary>
    /// Armazena parâmetros configuráveis do sistema.
    /// </summary>
    public class ParametroSistema : Entity
    {
        public string Chave { get; private set; }
        public string Valor { get; private set; }
        public string Descricao { get; private set; }

        private ParametroSistema()
        {
            Chave = null!;
            Valor = null!;
            Descricao = null!;
        }

        public ParametroSistema(string chave, string valor, string descricao)
        {
            if (string.IsNullOrWhiteSpace(chave))
                throw new DomainException("Chave é obrigatória.");

            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("Valor é obrigatório.");

            Chave = chave;
            Valor = valor;
            Descricao = descricao;
        }

        public void AlterarValor(string novoValor)
        {
            if (string.IsNullOrWhiteSpace(novoValor))
                throw new DomainException("Valor é obrigatório.");

            Valor = novoValor;
            DataAtualizacao = DateTime.UtcNow;
        }

        public decimal ObterComoDecimal() => decimal.Parse(Valor);
        public int ObterComoInt() => int.Parse(Valor);
    }
}