using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities
{
    /// <summary>
    /// Representa um ativo negociável (ação) no sistema.
    /// Contém os dados cadastrais da ação, não a posse pelo cliente.
    /// </summary>
    public class Acao : Entity
    {
        public string Codigo { get; private set; }
        public string NomeEmpresa { get; private set; }
        public decimal Preco { get; private set; }
        public bool Ativo { get; private set; }

        private Acao() 
        {
            Codigo = null!;
            NomeEmpresa = null!;
        }

        public Acao(string codigo, string nomeEmpresa, decimal preco)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new DomainException("Código é obrigatório.");

            if (string.IsNullOrWhiteSpace(nomeEmpresa))
                throw new DomainException("Nome da empresa é obrigatório.");

            if (preco < 0)
                throw new DomainException("Preço não pode ser negativo.");

            Codigo = codigo.ToUpperInvariant();
            NomeEmpresa = nomeEmpresa;
            Preco = preco;
            Ativo = true;
        }

        public void AtualizarPreco(decimal novoPreco)
        {
            if (novoPreco < 0)
                throw new DomainException("Preço não pode ser negativo.");

            Preco = novoPreco;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Ativar()
        {
            Ativo = true;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Retorna o ticker no mercado fracionário (sufixo F).
        /// </summary>
        public string ObterTickerFracionario()
        {
            return $"{Codigo}F";
        }
    }
}
