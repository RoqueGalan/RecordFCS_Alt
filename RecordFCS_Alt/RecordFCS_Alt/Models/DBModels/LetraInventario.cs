using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordFCS_Alt.Models
{
    [MetadataType(typeof(LetraInventarioMetadata))]
    public partial class LetraInventario
    {
        [Key]
        public int LetraInventarioID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Status { get; set; }

        //Virtuales
        public virtual ICollection<Obra> Obras { get; set; }
    }

    public class LetraInventarioMetadata
    {
        public int LetraInventarioID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido.")]
        [StringLength(2, ErrorMessage = "Máximo 2 letras")]
        [Display(Name = "Letra")]
        [Remote("EsUnico", "LetraInventario", HttpMethod = "POST", AdditionalFields = "LetraInventarioID", ErrorMessage = "Ya existe, intenta otra letra.")]
        public string Nombre { get; set; }
        [StringLength(128)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Estado")]
        public bool Status { get; set; }
    }
}