using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaCompra.Application.Produto.Command.AtualizarPreco
{
    public class AtualizarPrecoCommandHandler : IRequestHandler<AtualizarPrecoCommand>
    {
        private readonly IProdutoRepository _produtoRepository;

        public AtualizarPrecoCommandHandler(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        async Task<Unit> IRequestHandler<AtualizarPrecoCommand, Unit>.Handle(AtualizarPrecoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterAsync(request.Id);
            if (produto != null)
            {
                produto.AtualizarPreco(request.Preco);
                _produtoRepository.Atualizar(produto);
            }

            return Unit.Value;
        }

    }
}
