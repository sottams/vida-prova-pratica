using Microsoft.Extensions.Logging;
using SistemaCompra.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Application.SolicitacaoCompra.Events
{
    public class EnviarEmailCompraRegistradaHandler : IDomainEventHandler<CompraRegistrada>
    {
        private readonly ILogger<EnviarEmailCompraRegistradaHandler> _logger;
        public EnviarEmailCompraRegistradaHandler(ILogger<EnviarEmailCompraRegistradaHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CompraRegistrada evento)
        {

            // Simulação do envio de email cliente
            _logger.LogInformation(
                "Enviando email de compra registrada para {UsuarioSolicitante} referente à compra com valor total de {ValorTotal} registrada em {DataRegistro}.",
                evento.UsuarioSolicitante,
                evento.ValorTotal,
                evento.DataRegistro
            );

            // Simulação do envio de email para o fornecedor
            _logger.LogInformation(
                "Detalhes da compra: Fornecedor - {NomeFornecedor}, Itens - {Itens}",
                evento.NomeFornecedor,
                evento.Itens != null ? string.Join(", ", evento.Itens.Select(i => $"{i.NomeProduto}, Quantidade: {i.Quantidade}")) : "Nenhum item registrado"
            );

            return Task.CompletedTask;
        }
    }
}
