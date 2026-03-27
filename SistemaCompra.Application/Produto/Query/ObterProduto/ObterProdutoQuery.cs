using MediatR;
using System;

namespace SistemaCompra.Application.Produto.Query.ObterProduto
{
    public class ObterProdutoQuery : IRequest<ViewModels.ObterProdutoViewModel>
    {
        public Guid Id { get; set; }
    }
}
