namespace CompraProgramada.Domain.Entities.ClienteAggregate
{
    public class ContaGrafica : Entity
    {
        public int ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }

        public ICollection<CustodiaFilhote> Custodias { get; private set; }

        private ContaGrafica()
        {
            Custodias = [];
        }

        public ContaGrafica(int clienteId)
        {
            ClienteId = clienteId;
            Custodias = [];
        }
    }
}
