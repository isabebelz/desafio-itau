namespace CompraProgramada.Domain.Entities
{
    public abstract class Entity
    {
        public int Id { get; protected set; }

        public DateTime DataCriacao { get; protected set; }

        public DateTime? DataAtualizacao { get; set; }

        protected Entity()
        {
            DataCriacao = DateTime.UtcNow;
        }
    }
}
