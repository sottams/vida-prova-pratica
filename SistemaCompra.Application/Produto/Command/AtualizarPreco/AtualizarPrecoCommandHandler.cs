using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data.UoW;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaCompra.Application.Produto.Command.AtualizarPreco
{
    public class AtualizarPrecoCommandHandler : CommandHandler, IRequestHandler<AtualizarPrecoCommand>
    {
        private readonly IProdutoRepository _produtoRepository;

        public AtualizarPrecoCommandHandler(
            IProdutoRepository produtoRepository, 
            IUnitOfWork uow) : base(uow)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Unit> Handle(AtualizarPrecoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterAsync(request.Id);
            if (produto != null)
            {
                produto.AtualizarPreco(request.Preco);
                _produtoRepository.Atualizar(produto);
                Commit();
            }

            return Unit.Value;
        }

    }
}
