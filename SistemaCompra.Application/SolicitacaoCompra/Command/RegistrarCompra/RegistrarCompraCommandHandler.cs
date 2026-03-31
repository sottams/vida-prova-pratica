using MediatR;
using SistemaCompra.Application.SolicitacaoCompra.Events;
using SistemaCompra.Domain.Events;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;


namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler : CommandHandler, IRequestHandler<RegistrarCompraCommand>
    {
        private readonly ISolicitacaoCompraRepository _solicitacaoCompraRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IDomainEventDispatcher _dispatcher;

        public RegistrarCompraCommandHandler(
            ISolicitacaoCompraRepository solicitacaoCompraRepository,
            IProdutoRepository produtoRepository,
            IDomainEventDispatcher dispatcher,
            IUnitOfWork uow) : base(uow)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
            _produtoRepository = produtoRepository;
            _dispatcher = dispatcher;
        }

        public async Task<Unit> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var solicitacao = new SolicitacaoAgg.SolicitacaoCompra(request.UsuarioSolicitante, request.NomeFornecedor);
            foreach (var item in request.Itens)
            {
                var produto = await _produtoRepository.ObterAsync(item.IdProduto);
                solicitacao.AdicionarItem(produto, item.Quantidade);
            }
            solicitacao.RegistrarCompra();

            _solicitacaoCompraRepository.RegistrarCompra(solicitacao);

            Commit();

            await _dispatcher.Dispatch(
                new CompraRegistrada
                {
                    UsuarioSolicitante = solicitacao.UsuarioSolicitante.Nome,
                    NomeFornecedor = solicitacao.NomeFornecedor.Nome,
                    DataRegistro = solicitacao.Data,
                    ValorTotal = solicitacao.TotalGeral.Value,
                    Itens = solicitacao.Itens.Select(i => new ItemCompraRegistrada
                    {
                        NomeProduto = i.Produto.Nome,
                        Quantidade = i.Qtde,
                        Subtotal = i.Subtotal.Value
                    }).ToList()
                });

            return Unit.Value;
        }
    }
}
