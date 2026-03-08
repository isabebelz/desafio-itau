using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Interfaces.Services;

namespace CompraProgramada.Infra.Data.Parses
{
    public class CotahistParser : ICotahistParser
    {
        public IEnumerable<Cotacao> Parse(string caminhoArquivo)
        {
            var cotacoes = new List<Cotacao>();

            var encoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            foreach (var linha in File.ReadLines(caminhoArquivo, encoding))
            {
                if (!linha.StartsWith("01")) continue;

                var dataBruta = linha.Substring(2, 8);
                var bdi = linha.Substring(10, 2);
                var ticker = linha.Substring(12, 12).Trim();
                var tipoMercado = int.Parse(linha.Substring(24, 3));
                var precoAbertura = long.Parse(linha.Substring(56, 13));
                var precoMax = long.Parse(linha.Substring(69, 13));
                var precoMin = long.Parse(linha.Substring(82, 13));
                var precoFechamento = long.Parse(linha.Substring(108, 13));

                var cotacao = Cotacao.CriarDeDadosBrutosB3(
                    ticker,
                    precoFechamento,
                    dataBruta,
                    bdi,
                    tipoMercado,
                    precoAbertura,
                    precoMax,
                    precoMin
                );

                cotacoes.Add(cotacao);
            }

            return cotacoes;
        }
    }
}
