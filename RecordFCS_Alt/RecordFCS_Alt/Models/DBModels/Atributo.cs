using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class Atributo
    {
        [Key]
        public Guid AtributoID { get; set; }
        public string NombreAlterno { get; set; }
        public bool Status { get; set; }

        //Llaves Foraneas
        [ForeignKey("TipoPieza")]
        public Guid TipoPiezaID { get; set; }

        [ForeignKey("TipoAtributo")]
        public Guid TipoAtributoID { get; set; }


        //virtuales
        public virtual TipoPieza TipoPieza { get; set; }
        public virtual TipoAtributo TipoAtributo { get; set; }
        public virtual ICollection<MostrarAtributo> MostrarAtributos { get; set; }

        //public virtual ICollection<AtributoPieza> AtributoPiezas { get; set; }

    }
}