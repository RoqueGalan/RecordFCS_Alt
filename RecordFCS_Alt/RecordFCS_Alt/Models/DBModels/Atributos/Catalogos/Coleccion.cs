using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordFCS_Alt.Models
{
    [MetadataType(typeof(ColeccionMetadata))]
    public class Coleccion
    {
        [Key]
        public Guid ColeccionID { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; }

        //Anteriores
        public string Temp { get; set; }

        //Virtuales
        public virtual ICollection<Obra> Obras { get; set; }
    }

    public class ColeccionMetadata
    {
        public Guid ColeccionID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido.")]
        [StringLength(255)]
        [Display(Name = "Colección")]
        [Remote("EsUnico", "Coleccion", HttpMethod = "POST", AdditionalFields = "ColeccionID", ErrorMessage = "Ya existe, intenta otro nombre.")]
        public string Nombre { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [StringLength(63)]
        public string Temp { get; set; }

    }
}