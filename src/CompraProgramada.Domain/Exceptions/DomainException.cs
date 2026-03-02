namespace CompraProgramada.Domain.Exceptions
{
    /// <summary>
    /// Exceção lançada quando uma regra de negócio do domínio é violada.
    /// Exemplos: valor de aporte abaixo do mínimo, cesta com soma ≠ 100%, 
    /// cliente inativo tentando alterar aporte.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
