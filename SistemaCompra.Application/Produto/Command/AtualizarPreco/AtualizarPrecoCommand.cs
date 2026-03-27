using MediatR;
using System;

namespace SistemaCompra.Application.Produto.Command.AtualizarPreco
{
    public class AtualizarPrecoCommand : IRequest
    {
        public Guid Id { get; set; }
        public decimal Preco { get; set; }
    }
}
