using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class MostrarAtributo
    {
        //Llaves Primarias
        [Key]
        [Column(Order = 1)]
        [ForeignKey("TipoMostrar")]
        public Guid TipoMostrarID{ get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Atributo")]
        public Guid AtributoID { get; set; }
        

        public int Orden { get; set; }
        public string InicioHTML { get; set; }
        public string FinHTML { get; set; }
        public bool Status { get; set; }


        //Virtuales
        public virtual TipoMostrar TipoMostrar { get; set; }
        public virtual Atributo Atributo { get; set; }
       
    }
}