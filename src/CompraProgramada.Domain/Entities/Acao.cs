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
            Codigo = codigo;
            NomeEmpresa = nomeEmpresa;
            Preco = preco;
            Ativo = true;
        }

        public Acao(int id, string codigo, string nomeEmpresa, decimal preco)
        {
            Id = id;
            Codigo = codigo;
            NomeEmpresa = nomeEmpresa;
            Preco = preco;
            Ativo = true;
        }
    }
}
