using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RecordFCS_Alt.Models
{
    public class RecordFCSContext : DbContext
    {
        public RecordFCSContext() : base("name=DefaultConnection") { }

        //Configuracion de Estructura de Obras y Piezas
        public DbSet<TipoObra> TipoObras { get; set; }
        public DbSet<TipoPieza> TipoPiezas { get; set; }
        public DbSet<Atributo> Atributos { get; set; }
        public DbSet<TipoAtributo> TipoAtributos { get; set; }
        public DbSet<MostrarAtributo> MostrarAtributos { get; set; }
        public DbSet<TipoMostrar> TipoMostarlos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}