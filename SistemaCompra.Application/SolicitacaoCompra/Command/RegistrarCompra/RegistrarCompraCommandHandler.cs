using MediatR;
using SistemaCompra.Application.SolicitacaoCompra.Events;
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
        private readonly IEventPublisher _eventPublisher;

        public RegistrarCompraCommandHandler(
            ISolicitacaoCompraRepository solicitacaoCompraRepository,
            IProdutoRepository produtoRepository,
            IEventPublisher eventPublisher,
            IUnitOfWork uow) : base(uow)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
            _produtoRepository = produtoRepository;
            _eventPublisher = eventPublisher;
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

            await _eventPublisher.PublishAsync(new CompraRegistrada
            {
                IdSolicitacao = solicitacao.Id,
                UsuarioSolicitante = request.UsuarioSolicitante,
                NomeFornecedor = request.NomeFornecedor,
                DataRegistro = DateTime.UtcNow
            });

            return Unit.Value;
        }
    }
}
