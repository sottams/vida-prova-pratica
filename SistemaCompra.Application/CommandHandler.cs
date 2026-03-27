using MediatR;
using SistemaCompra.Domain.Core;
using SistemaCompra.Infra.Data.UoW;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Application
{
    public abstract class CommandHandler
    {
        private readonly IUnitOfWork _uow;

        public CommandHandler(IUnitOfWork uow)
        {            
            _uow = uow;
        }

        public bool Commit()
        {
            if (_uow.Commit()) return true;

            return false;
        }
    }
}
