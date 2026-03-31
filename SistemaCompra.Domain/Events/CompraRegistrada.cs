using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Domain.Events
{
    public class CompraRegistrada
    {
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal ValorTotal { get; set; }
        public IList<ItemCompraRegistrada> Itens { get; set; }
    }

    public class ItemCompraRegistrada
    {
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }
    }
}
