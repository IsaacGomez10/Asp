using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
namespace Asp.Models
{
    public class BaseModelo
    {
        public int paginaActual { get; set; }
        public int totalRegistros { get; set; }
        public int registrosPorPagina { get; set; }
        public RouteValueDictionary valueQueryString { get; set; }
    }
}