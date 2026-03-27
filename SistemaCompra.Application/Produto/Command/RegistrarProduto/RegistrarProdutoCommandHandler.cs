using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaCompra.Application.Produto.Command.RegistrarProduto
{
    public class RegistrarProdutoCommandHandler : IRequestHandler<RegistrarProdutoCommand>
    {
        private readonly IProdutoRepository _produtoRepository;

        public RegistrarProdutoCommandHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        async Task<Unit> IRequestHandler<RegistrarProdutoCommand, Unit>.Handle(RegistrarProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = new Domain.ProdutoAggregate.Produto(
                             request.Nome,
                             request.Descricao,
                             request.Categoria,
                             request.Preco
                         );

            await _produtoRepository.RegistrarAsync(produto);

            return Unit.Value;
        }
    }
}
