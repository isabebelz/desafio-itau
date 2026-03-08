using CompraProgramada.Domain.DTOs.Events;
using CompraProgramada.Application.Events;
using CompraProgramada.Domain.Entities.CestaAggregate;
using CompraProgramada.Domain.Entities.ClienteAggregate;
using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using Moq;
using CompraProgramada.Application.Services;
using ParametroSistema = CompraProgramada.Domain.Entities.ParametroSistema;
using Acao = CompraProgramada.Domain.Entities.Acao;

namespace CompraProgramada.Application.Tests
{
    public class MotorDeCompraServiceTests
    {
        [Fact]
        public async Task ExecutarCicloAsync_DeveExecutarFluxoCompleto_QuandoExistemClientesECestaECPF()
        {
            var parametroRepoMock = new Mock<IParametroSistemaRepository>();
            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("VALOR_APORTE_MINIMO"))
                .ReturnsAsync(new ParametroSistema(
                    "VALOR_APORTE_MINIMO", "50", "Valor mínimo de aporte"));

            decimal valorAporteMinimo = 50m;
            var cliente = new Cliente("Cliente Teste", "35026501010", "mail@exemplo.com", 300, valorAporteMinimo);

            var idPropCliente = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropCliente?.SetValue(cliente, 1);

            var contaGrafica = cliente.CriarContaGrafica();

            var clientes = new List<Cliente> { cliente };

            var cestaAtiva = new CestaRecomendacao(DateTime.UtcNow);
            cestaAtiva.AdicionarItem(1, 100m);

            var acao = new Acao("PETR4", "Petrobras", 28.50m);

            var idPropAcao = typeof(Acao).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropAcao?.SetValue(acao, 1);

            var clienteRepoMock = new Mock<IClienteRepository>();
            clienteRepoMock.Setup(x => x.ObterTodosAtivosAsync()).ReturnsAsync(clientes);
            clienteRepoMock.Setup(x => x.ObterContaGraficaPorClienteIdAsync(cliente.Id)).ReturnsAsync(contaGrafica);
            clienteRepoMock.Setup(x => x.ObterCustodiaPorContaEAcaoAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((CustodiaFilhote?)null);

            var cestaRepoMock = new Mock<ICestaRecomendacaoRepository>();
            cestaRepoMock.Setup(x => x.ObterAtivaComItensAsync()).ReturnsAsync(cestaAtiva);

            var ordemRepoMock = new Mock<IOrdemCompraRepository>();
            ordemRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<OrdemCompra>())).Returns(Task.CompletedTask);
            ordemRepoMock.Setup(x => x.AdicionarDistribuicaoAsync(It.IsAny<Distribuicao>())).Returns(Task.CompletedTask);

            var acaoRepoMock = new Mock<IAcaoRepository>();
            acaoRepoMock.Setup(x => x.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync([acao]);

            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("ALIQUOTA_IR_DEDO_DURO"))
                .ReturnsAsync(new ParametroSistema(
                    "ALIQUOTA_IR_DEDO_DURO", "0.00005", "Aliquota IR dedo-duro"
                ));

            var kafkaProducerMock = new Mock<IKafkaProducer>();
            kafkaProducerMock.Setup(x => x.PublicarDistribuicaoIrDedoDuroAsync(It.IsAny<DistribuicaoIrDedoDuroKafkaEvent>())).Returns(Task.CompletedTask);

            var contaMasterRepoMock = new Mock<IContaMasterRepository>();
            contaMasterRepoMock.Setup(x => x.ObterAsync())
                .ReturnsAsync(new ContaMaster("Conta Master"));

            var service = new MotorCompraService(
                clienteRepoMock.Object,
                cestaRepoMock.Object,
                ordemRepoMock.Object,
                acaoRepoMock.Object,
                parametroRepoMock.Object,
                kafkaProducerMock.Object,
                contaMasterRepoMock.Object 
            );

            var resultado = await service.ExecutarCicloAsync();

            clienteRepoMock.Verify(x => x.ObterTodosAtivosAsync(), Times.Once);
            cestaRepoMock.Verify(x => x.ObterAtivaComItensAsync(), Times.Once);
            acaoRepoMock.Verify(x => x.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>()), Times.Once);
            ordemRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<OrdemCompra>()), Times.Once);
            ordemRepoMock.Verify(x => x.AdicionarDistribuicaoAsync(It.IsAny<Distribuicao>()), Times.AtLeastOnce);
            kafkaProducerMock.Verify(x => x.PublicarDistribuicaoIrDedoDuroAsync(It.IsAny<DistribuicaoIrDedoDuroKafkaEvent>()), Times.AtLeastOnce);

            Assert.Contains("Ordem consolidada criada", resultado);
        }

        [Fact]
        public async Task ExecutarCicloAsync_DeveDistribuirCorretamenteEntreDoisClientesEDuasAcoes()
        {
            var parametroRepoMock = new Mock<IParametroSistemaRepository>();
            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("VALOR_APORTE_MINIMO"))
                .ReturnsAsync(new ParametroSistema("VALOR_APORTE_MINIMO", "50", "Valor mínimo de aporte"));

            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("ALIQUOTA_IR_DEDO_DURO"))
                .ReturnsAsync(new ParametroSistema("ALIQUOTA_IR_DEDO_DURO", "0.00005", "Aliquota IR dedo-duro"));

            var clienteA = new Cliente("Cliente A", "60361972091", "a@a.com", 3000, 50m);

            var idPropClienteA = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropClienteA?.SetValue(clienteA, 1);

            var contaGraficaA = clienteA.CriarContaGrafica();

            var clienteB = new Cliente("Cliente B", "79756435054", "b@b.com", 6000, 50m);

            var idPropClienteB = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropClienteB?.SetValue(clienteB, 2);

            var contaGraficaB = clienteB.CriarContaGrafica();

            var clientes = new List<Cliente> { clienteA, clienteB };

            var cestaAtiva = new CestaRecomendacao(DateTime.UtcNow);
            cestaAtiva.AdicionarItem(1, 60m);
            cestaAtiva.AdicionarItem(2, 40m);

            var acao1 = new Acao("PETR4", "Petrobras", 38.50m);
            var idPropAcao1 = typeof(Acao).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropAcao1?.SetValue(acao1, 1);

            var acao2 = new Acao("VALE3", "Vale", 62.00m);
            var idPropAcao2 = typeof(Acao).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropAcao2?.SetValue(acao2, 2);

            var clienteRepoMock = new Mock<IClienteRepository>();
            clienteRepoMock.Setup(x => x.ObterTodosAtivosAsync()).ReturnsAsync(clientes);
            clienteRepoMock.Setup(x => x.ObterContaGraficaPorClienteIdAsync(1)).ReturnsAsync(contaGraficaA);
            clienteRepoMock.Setup(x => x.ObterContaGraficaPorClienteIdAsync(2)).ReturnsAsync(contaGraficaB);
            clienteRepoMock.Setup(x => x.ObterCustodiaPorContaEAcaoAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((CustodiaFilhote?)null);

            var cestaRepoMock = new Mock<ICestaRecomendacaoRepository>();
            cestaRepoMock.Setup(x => x.ObterAtivaComItensAsync()).ReturnsAsync(cestaAtiva);

            var ordemRepoMock = new Mock<IOrdemCompraRepository>();
            ordemRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<OrdemCompra>())).Returns(Task.CompletedTask);
            ordemRepoMock.Setup(x => x.AdicionarDistribuicaoAsync(It.IsAny<Distribuicao>())).Returns(Task.CompletedTask);

            var acaoRepoMock = new Mock<IAcaoRepository>();
            acaoRepoMock.Setup(x => x.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync([acao1, acao2]);

            var kafkaProducerMock = new Mock<IKafkaProducer>();
            kafkaProducerMock.Setup(x => x.PublicarDistribuicaoIrDedoDuroAsync(It.IsAny<DistribuicaoIrDedoDuroKafkaEvent>())).Returns(Task.CompletedTask);

            var contaMasterRepoMock = new Mock<IContaMasterRepository>();
            contaMasterRepoMock.Setup(x => x.ObterAsync())
                .ReturnsAsync(new ContaMaster("Conta Master"));

            var service = new MotorCompraService(
                clienteRepoMock.Object,
                cestaRepoMock.Object,
                ordemRepoMock.Object,
                acaoRepoMock.Object,
                parametroRepoMock.Object,
                kafkaProducerMock.Object,
                contaMasterRepoMock.Object
            );

            var resultado = await service.ExecutarCicloAsync();

            clienteRepoMock.Verify(x => x.ObterTodosAtivosAsync(), Times.Once);
            cestaRepoMock.Verify(x => x.ObterAtivaComItensAsync(), Times.Once);
            acaoRepoMock.Verify(x => x.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>()), Times.Once);
            ordemRepoMock.Verify(x => x.AdicionarAsync(It.IsAny<OrdemCompra>()), Times.Once);
            ordemRepoMock.Verify(x => x.AdicionarDistribuicaoAsync(It.IsAny<Distribuicao>()), Times.AtLeast(2));

            kafkaProducerMock.Verify(x => x.PublicarDistribuicaoIrDedoDuroAsync(It.IsAny<DistribuicaoIrDedoDuroKafkaEvent>()), Times.AtLeast(2));

            Assert.Contains("Ordem consolidada criada", resultado);

            ordemRepoMock.Verify(x => x.AdicionarDistribuicaoAsync(It.Is<Distribuicao>(d => d.ContaGraficaId == contaGraficaA.Id && (d.AcaoId == 1 || d.AcaoId == 2))), Times.AtLeastOnce);
            ordemRepoMock.Verify(x => x.AdicionarDistribuicaoAsync(It.Is<Distribuicao>(d => d.ContaGraficaId == contaGraficaB.Id && (d.AcaoId == 1 || d.AcaoId == 2))), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecutarCicloAsync_DistribuicaoExataPorClienteEAcao()
        {
            var parametroRepoMock = new Mock<IParametroSistemaRepository>();

            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("VALOR_APORTE_MINIMO"))
                .ReturnsAsync(new ParametroSistema("VALOR_APORTE_MINIMO", "50", "Valor mínimo de aporte"));

            parametroRepoMock.Setup(x => x.ObterPorChaveAsync("ALIQUOTA_IR_DEDO_DURO"))
                .ReturnsAsync(new ParametroSistema("ALIQUOTA_IR_DEDO_DURO", "0.00005", "Aliquota IR dedo-duro"));

            var clienteA = new Cliente("Cliente A", "90043728065", "a@a.com", 3000, 50m);
            var idPropClienteA = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropClienteA?.SetValue(clienteA, 1);

            var contaGraficaA = clienteA.CriarContaGrafica();
            var idPropContaGraficaA = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropContaGraficaA?.SetValue(contaGraficaA, 1);

            var clienteB = new Cliente("Cliente B", "90043728065", "b@b.com", 6000, 50m);
            var idPropClienteB = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropClienteB?.SetValue(clienteB, 2);

            var contaGraficaB = clienteB.CriarContaGrafica();
            var idPropContaGraficaB = typeof(Cliente).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropContaGraficaB?.SetValue(contaGraficaB, 2);

            var clientes = new List<Cliente> { clienteA, clienteB };

            var cestaAtiva = new CestaRecomendacao(DateTime.UtcNow);
            cestaAtiva.AdicionarItem(1, 60m);
            cestaAtiva.AdicionarItem(2, 40m);

            var acao1 = new Acao("PETR4", "Petrobras", 38.50m);
            var idPropAcao1 = typeof(Acao).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropAcao1?.SetValue(acao1, 1);

            var acao2 = new Acao("VALE3", "Vale", 62.00m);
            var idPropAcao2 = typeof(Acao).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropAcao2?.SetValue(acao2, 2);

            var clienteRepoMock = new Mock<IClienteRepository>();
            clienteRepoMock.Setup(x => x.ObterTodosAtivosAsync()).ReturnsAsync(clientes);
            clienteRepoMock.Setup(x => x.ObterContaGraficaPorClienteIdAsync(1)).ReturnsAsync(contaGraficaA);
            clienteRepoMock.Setup(x => x.ObterContaGraficaPorClienteIdAsync(2)).ReturnsAsync(contaGraficaB);
            clienteRepoMock.Setup(x => x.ObterCustodiaPorContaEAcaoAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((CustodiaFilhote?)null);

            var cestaRepoMock = new Mock<ICestaRecomendacaoRepository>();
            cestaRepoMock.Setup(x => x.ObterAtivaComItensAsync()).ReturnsAsync(cestaAtiva);

            var ordemRepoMock = new Mock<IOrdemCompraRepository>();
            ordemRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<OrdemCompra>())).Returns(Task.CompletedTask);

            var distribuicoes = new List<Distribuicao>();
            ordemRepoMock.Setup(x => x.AdicionarDistribuicaoAsync(Capture.In(distribuicoes))).Returns(Task.CompletedTask);

            var acaoRepoMock = new Mock<IAcaoRepository>();
            acaoRepoMock.Setup(x => x.ObterPorIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new List<Acao> { acao1, acao2 });

            var kafkaProducerMock = new Mock<IKafkaProducer>();
            kafkaProducerMock.Setup(x => x.PublicarDistribuicaoIrDedoDuroAsync(It.IsAny<DistribuicaoIrDedoDuroKafkaEvent>())).Returns(Task.CompletedTask);

            var contaMasterRepoMock = new Mock<IContaMasterRepository>();
            contaMasterRepoMock.Setup(x => x.ObterAsync())
                .ReturnsAsync(new ContaMaster("Conta Master"));

            var service = new MotorCompraService(
                clienteRepoMock.Object,
                cestaRepoMock.Object,
                ordemRepoMock.Object,
                acaoRepoMock.Object,
                parametroRepoMock.Object,
                kafkaProducerMock.Object,
                contaMasterRepoMock.Object
            );

            var resultado = await service.ExecutarCicloAsync();

            Assert.Contains(distribuicoes, d => d.ContaGraficaId == 1 && d.AcaoId == 1 && d.Quantidade == 15);
            Assert.Contains(distribuicoes, d => d.ContaGraficaId == 2 && d.AcaoId == 1 && d.Quantidade == 30);
            Assert.Contains(distribuicoes, d => d.ContaGraficaId == 1 && d.AcaoId == 2 && d.Quantidade == 6);
            Assert.Contains(distribuicoes, d => d.ContaGraficaId == 2 && d.AcaoId == 2 && d.Quantidade == 12);

            Assert.Contains("Ordem consolidada criada", resultado);
        }
    }
}