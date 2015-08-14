using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    [MetadataType(typeof(ObraMetadata))]
    public partial class Obra
    {
        [Key]
        public Guid ObraID { get; set; }

        public string NoInventario { get; set; }
        public Int64 NumeroConsecutivo { get; set; }
        
        public string FechaCreacion { get; set; }




        //Llaves Foraneas
        [ForeignKey("LetraInventario")]
        public int LetraInventarioID { get; set; }





        //Anteriores
        public string Temp { get; set; }

        //Virtuales
        public virtual LetraInventario LetraInventario { get; set; }
    }


    public class ObraMetadata
    {

    }

}