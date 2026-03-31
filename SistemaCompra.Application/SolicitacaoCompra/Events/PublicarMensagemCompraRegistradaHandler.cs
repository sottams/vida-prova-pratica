using RabbitMQ.Client;
using SistemaCompra.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaCompra.Application.SolicitacaoCompra.Events
{
    public class PublicarMensagemCompraRegistradaHandler : IDomainEventHandler<CompraRegistrada>
    {
        private readonly IConnection _connection;
        public PublicarMensagemCompraRegistradaHandler(IConnection connection)
        {
            _connection = connection;
        }
        public async Task Handle(CompraRegistrada evento)
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "compra_registrada", durable: false, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "compra_registrada", body: body);
            await Task.CompletedTask;
        }
    }
}
