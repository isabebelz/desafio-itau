using CompraProgramada.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompraProgramada.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;
    private static readonly int[] DiasCompra = [5, 15, 25];

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(RunMotorDeCompra, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private async void RunMotorDeCompra(object? state)
    {
        try
        {
            var hoje = DateTime.Today;

            if (DeveExecutarMotorHoje(hoje))
            {
                using var scope = _serviceProvider.CreateScope();
                var motor = scope.ServiceProvider.GetRequiredService<IMotorCompraService>();
                _logger.LogInformation("Executando MotorCompraService em {Data}.", hoje);

                await motor.ExecutarCicloAsync();
                _logger.LogInformation("MotorCompraService executado com sucesso.");
            }
            else
            {
                _logger.LogInformation("Hoje ({Dia}) não é dia de execução do motor.", hoje);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar MotorCompraService worker.");
        }
    }

    private static bool DeveExecutarMotorHoje(DateTime hoje)
    {
        if (DiasCompra.Contains(hoje.Day) &&
            hoje.DayOfWeek >= DayOfWeek.Monday && hoje.DayOfWeek <= DayOfWeek.Friday)
            return true;

        if (hoje.DayOfWeek == DayOfWeek.Monday)
        {
            var sabadoAnterior = hoje.AddDays(-2);
            var domingoAnterior = hoje.AddDays(-1);

            if (sabadoAnterior.Month == hoje.Month && DiasCompra.Contains(sabadoAnterior.Day))
                return true;
            if (domingoAnterior.Month == hoje.Month && DiasCompra.Contains(domingoAnterior.Day))
                return true;
        }

        return false;
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}