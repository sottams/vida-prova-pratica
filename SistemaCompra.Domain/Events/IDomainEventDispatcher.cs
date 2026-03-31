using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Domain.Events
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch<T>(T evento);
    }
}
