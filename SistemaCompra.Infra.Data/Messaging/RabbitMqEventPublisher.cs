using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaCompra.Infra.Data.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        public RabbitMqEventPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync<T>(T evento)
        {
            using var channel = _connection.CreateModel();
            var nomeFila = typeof(T).Name;

            channel.QueueDeclare(nomeFila, durable: true, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: nomeFila, body: body);

            await Task.CompletedTask;
        }

    }
}
