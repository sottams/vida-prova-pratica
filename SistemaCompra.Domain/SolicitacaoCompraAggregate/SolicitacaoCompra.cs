using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        private const decimal LimiteTotalParaPagamentoTrintaDias = 50000;
        private const int CondicaoPagamentoTrintaDias = 30;
        private const int CondicaoPagamentoPadrao = 0;

        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public IList<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public CondicaoPagamento CondicaoPagamento { get; private set; }
        public Situacao Situacao { get; private set; }

        private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            Itens = new List<Item>();
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
            Itens.Add(new Item(produto, qtde));
        }

        public void RegistrarCompra()
        {
            if (!Itens.Any()) throw new BusinessRuleException("A solicitação de compra deve conter ao menos um item.");

            TotalGeral = new Money(Itens.Sum(i => i.Subtotal.Value));

            CondicaoPagamento = TotalGeral.Value > LimiteTotalParaPagamentoTrintaDias
                ? new CondicaoPagamento(CondicaoPagamentoTrintaDias)
                : new CondicaoPagamento(CondicaoPagamentoPadrao);
        }
    }
}
