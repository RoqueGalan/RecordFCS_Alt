using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class TipoAtributo
    {
        [Key]
        public Guid TipoAtributoID { get; set; }

        [StringLength(64)]
        public string Nombre { get; set; }
        
        [StringLength(128)]
        public string Descripcion { get; set; }
        public string Dato { get; set; }
        public bool EsLista { get; set; }
        public bool EsMultipleValor { get; set; }
        public string MyProperty { get; set; }
        public string PerteneceA { get; set; }
        public string Tabla { get; set; }
        public string HTMLAtributos { get; set; }
        public bool EstaEnBuscador { get; set; }
        public int Orden { get; set; }
        public bool Status { get; set; }

        //Anteriores
        public string Temp { get; set; }


        //Virtuales
        public virtual ICollection<Atributo> Atributos { get; set; }



    }
}