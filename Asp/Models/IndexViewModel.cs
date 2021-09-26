using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Models
{
    public class IndexViewModel : BaseModelo
    {
        public List<usuario> Usuarios { get; set; }

        public List<proveedor> Proveedores { get; set; }

        public List<cliente> Clientes { get; set; }

        public List<producto> Productos { get; set; }

        public List<roles> Roles { get; set; }

    }
}