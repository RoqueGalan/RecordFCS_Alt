using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class TipoMostrar
    {
        [Key]
        public Guid TipoMostrarID { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; }

        //virtual
        public virtual ICollection<MostrarAtributo> MostrarAtributos { get; set; }
    } 

}