using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class ItemCommand
    {
        public Guid IdProduto { get; set; }
        public int Quantidade { get; set; }
    }
}
