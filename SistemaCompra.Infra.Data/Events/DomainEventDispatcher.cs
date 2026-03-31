using Microsoft.Extensions.DependencyInjection;
using SistemaCompra.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Infra.Data.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch<T>(T evento)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<T>>();
            foreach(var handler in handlers)
                await handler.Handle(evento);
        }
    }
}
