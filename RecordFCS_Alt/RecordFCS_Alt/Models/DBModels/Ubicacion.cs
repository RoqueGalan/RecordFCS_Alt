﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordFCS_Alt.Models.DBModels
{
    [MetadataType(typeof(UbicacionMetadata))]

    public partial class Ubicacion
    {
        [Key]
        public Guid UbicacionID { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Status { get; set; }


        //Anteriores
        public string Temp { get; set; }

        //Virtuales
        public virtual ICollection<Obra> Obras { get; set; }
        //public virtual ICollection<Pieza> Piezas { get; set; }
    }

    public class UbicacionMetadata
    {
        public Guid UbicacionID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido.")]
        [StringLength(64)]
        [Display(Name = "Ubicación")]
        [Remote("EsUnico", "Ubicacion", HttpMethod = "POST", AdditionalFields = "UbicacionID", ErrorMessage = "Ya existe, intenta otro nombre.")]
        public string Nombre { get; set; }

        [StringLength(128)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [StringLength(32)]
        public string Temp { get; set; }

    }
}