using MediatR;
using SistemaCompra.Domain.Core;
using SistemaCompra.Infra.Data.UoW;
using System;
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

        public void Commit()
        {
            if (!_uow.Commit())
                throw new InvalidOperationException("Ocorreu um erro ao salvar os dados.");
        }
    }
}
