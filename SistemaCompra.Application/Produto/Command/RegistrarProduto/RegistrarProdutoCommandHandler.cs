using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data.UoW;
using System.Threading;
using System.Threading.Tasks;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
namespace SistemaCompra.Application.Produto.Command.RegistrarProduto
{
    public class RegistrarProdutoCommandHandler : CommandHandler, IRequestHandler<RegistrarProdutoCommand>
    {
        private readonly IProdutoRepository _produtoRepository;

        public RegistrarProdutoCommandHandler(
            IProdutoRepository produtoRepository, 
            IUnitOfWork uow) : base(uow) 
        {
            _produtoRepository = produtoRepository;
        }
         public async Task<Unit> Handle(RegistrarProdutoCommand request, CancellationToken cancellationToken)

        {
            var produto = new ProdutoAgg.Produto(
                             request.Nome,
                             request.Descricao,
                             request.Categoria,
                             request.Preco
                         );

            await _produtoRepository.RegistrarAsync(produto);

            Commit();
            return Unit.Value;
        }
    }
}
