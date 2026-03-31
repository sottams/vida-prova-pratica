using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Domain.Events
{
    public interface IDomainEventHandler<T>
    {
        Task Handle(T evento);
    }
}
