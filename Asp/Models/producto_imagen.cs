//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class producto_imagen
    {
        public int id { get; set; }

        [Required]
        [RegularExpression(@"^.{40,80}$", ErrorMessage = "sobrepaso el limite")]
        public string imagen { get; set; }

        [Required]
        public int id_producto { get; set; }
    
        public virtual producto producto { get; set; }
    }
}
