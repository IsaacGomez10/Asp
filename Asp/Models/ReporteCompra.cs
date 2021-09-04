using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Models
{
    public class ReporteCompra
    {
        public string nombreCliente { get; set; }
        public int cantidadProdctComp { get; set; }
        public string nombreUsuario { get; set; }

        public DateTime fechaCompra { get; set; }
        public int totalCompra { get; set; }

    }
}