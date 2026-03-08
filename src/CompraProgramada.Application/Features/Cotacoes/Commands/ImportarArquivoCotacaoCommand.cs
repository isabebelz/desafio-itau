using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompraProgramada.Application.Features.Cotacoes.Commands
{
    public record ImportarArquivoCotacaoCommand(string CaminhoArquivo) : IRequest<bool>;
}
