using AutoMapper;
using MediatR;
using SistemaCompra.Application.ViewModels;
using SistemaCompra.Domain.ProdutoAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaCompra.Application.Produto.Query.ObterProduto
{
    public class ObterProdutoQueryHandler : IRequestHandler<ObterProdutoQuery, ObterProdutoViewModel>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ObterProdutoQueryHandler(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public async Task<ObterProdutoViewModel> Handle(ObterProdutoQuery request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterAsync(request.Id);
            return _mapper.Map<ObterProdutoViewModel>(produto);
        }
    }
}
