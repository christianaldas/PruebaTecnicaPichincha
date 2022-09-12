using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Data
{
    public class PruebaTecnicaPichinchaContext : DbContext
    {
        public DbSet<PersonaEntity>? Personas { get; set; }
        public DbSet<ClienteEntity>? Clientes { get; set; }
        public DbSet<CuentaEntity>? Cuentas { get; set; }
        public DbSet<MovimientoEntity>? Movimientos { get; set; }



        public PruebaTecnicaPichinchaContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClienteEntity>()
          .HasOne(p => p.Persona)
          .WithMany(b => b.Clientes);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PruebaTecnicaBP;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

    }
}
