using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Models
{
    public class Reporte //Producto
    {
        public string nombreProveedor { get; set; }
        public string direccionProveedor { get; set; }
        public string telefonoProveedor { get; set; }

        public string nombreProducto { get; set; }
        public int precioProducto { get; set; }


    }
}