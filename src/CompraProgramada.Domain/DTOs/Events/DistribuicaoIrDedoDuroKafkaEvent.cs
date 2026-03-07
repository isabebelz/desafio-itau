namespace CompraProgramada.Domain.DTOs.Events
{
    public class DistribuicaoIrDedoDuroKafkaEvent
    {
        public string Tipo { get; set; } = string.Empty;           
        public int ClienteId { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string TipoOperacao { get; set; } = string.Empty;      
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorOperacao { get; set; }
        public decimal Aliquota { get; set; }
        public decimal ValorIR { get; set; }         
        public DateTime DataOperacao { get; set; }
    }
}