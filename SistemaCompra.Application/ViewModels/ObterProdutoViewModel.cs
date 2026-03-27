using System;

namespace SistemaCompra.Application.ViewModels
{
    public class ObterProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public string Situacao { get; set; }
    }
}
