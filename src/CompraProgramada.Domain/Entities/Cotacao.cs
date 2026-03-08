using System.Globalization;

namespace CompraProgramada.Domain.Entities
{
    /// <summary>
    /// Representa uma Cotação Histórica da B3.
    /// </summary>
    public class Cotacao : Entity
    {
        public string Codigo { get; private set; }
        public decimal PrecoFechamento { get; private set; }
        public decimal PrecoAbertura { get; private set; }
        public decimal PrecoMaximo { get; private set; }
        public decimal PrecoMinimo { get; private set; }
        public DateTime DataPregao { get; private set; }
        public string CodigoBDI { get; private set; }
        public int TipoMercado { get; private set; }

        private Cotacao() 
        {
            Codigo = string.Empty;
            CodigoBDI = string.Empty;
        }

        public Cotacao(string codigo,
                       decimal precoFechamento,
                       DateTime dataPregao,
                       string codigoBdi,
                       int tipoMercado,
                       decimal precoAbertura = 0,
                       decimal precoMaximo = 0,
                       decimal precoMinimo = 0)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("O Código é obrigatório.");

            if (precoFechamento <= 0)
                throw new ArgumentException("O preço de fechamento deve ser maior que zero.");

            if (dataPregao > DateTime.Now)
                throw new ArgumentException("A data do pregão não pode ser uma data futura.");

            Codigo = codigo.Trim().ToUpper();
            PrecoFechamento = precoFechamento;
            DataPregao = dataPregao;
            CodigoBDI = codigoBdi.Trim();
            TipoMercado = tipoMercado;
            PrecoAbertura = precoAbertura;
            PrecoMaximo = precoMaximo;
            PrecoMinimo = precoMinimo;
        }

        /// <summary>
        /// Identifica se o ativo pertence ao mercado fracionário.
        /// Regra B3: Código BDI 96 OU Tipo de Mercado 020 OU código terminando com 'F'.
        /// </summary>
        public bool EhFracionario()
        {
            return CodigoBDI == "96" || TipoMercado == 20 || Codigo.EndsWith("F");
        }

        /// <summary>
        /// Retorna o codigo principal da ação. 
        /// Útil para consolidar PETR4 e PETR4F como o mesmo ativo no cálculo de carteira.
        /// </summary>
        public string ObterCodigoBase()
        {
            return (EhFracionario() && Codigo.EndsWith("F"))
                ? Codigo.Substring(0, Codigo.Length - 1)
                : Codigo;
        }

        /// <summary>
        /// Calcula o valor financeiro total baseado em uma quantidade de ações.
        /// </summary>
        public decimal CalcularValorTotal(long quantidade)
        {
            if (quantidade < 0) return 0;
            return PrecoFechamento * quantidade;
        }

        /// <summary>
        /// Factory Method para criar a entidade a partir dos dados brutos do arquivo COTAHIST.
        /// </summary>
        public static Cotacao CriarDeDadosBrutosB3(
            string tickerBruto,
            long precoFechamentoBruto,
            string dataBruta,
            string codigoBdiBruto,
            int tipoMercadoBruto,
            long precoAberturaBruto,
            long precoMaximoBruto,
            long precoMinimoBruto)
        {

            var data = DateTime.ParseExact(dataBruta, "yyyyMMdd", CultureInfo.InvariantCulture);

            return new Cotacao(
                tickerBruto,
                precoFechamentoBruto / 100m,
                data,
                codigoBdiBruto,
                tipoMercadoBruto,
                precoAberturaBruto / 100m,
                precoMaximoBruto / 100m,
                precoMinimoBruto / 100m
            );
        }
    }
}
