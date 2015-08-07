using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class TipoPieza
    {
        [Key]
        public Guid TipoPiezaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Status { get; set; }
        
        //Llaves Foraneas
        [ForeignKey("TipoObra")]
        public Guid TipoObraID { get; set; }

        [ForeignKey("TipoPiezaPadre")]
        public Guid? TipoPiezaPadreID { get; set; }



        //Anteriores
        public string Temp { get; set; }

        //Virtuales
        public virtual TipoObra TipoObra { get; set; }
        public virtual ICollection<Atributo> Atributos { get; set; }
        //public virtual ICollection<Pieza> Piezas { get; set; }

        public virtual TipoPieza TipoPiezaPadre { get; set; }
        public virtual ICollection<TipoPieza> TipoPiezasHijas { get; set; }


    }
}