using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Application.SolicitacaoCompra.Events
{
    public class CompraRegistrada
    {
        public Guid IdSolicitacao { get; set; }
        public string UsuarioSolicitante { get; set; }
        public string NomeFornecedor { get; set; }
        public DateTime DataRegistro { get; set; }
    }
}
