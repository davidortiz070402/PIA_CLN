using Microsoft.EntityFrameworkCore;
using CargoBox.Domain;

namespace CargoBox.Infrastructure
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Paquete> Paquetes { get; set; }

        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.HasKey(p => p.Guia);
                entity.Property(p => p.Guia)
                      .HasConversion(g => g.Valor, v => new GuiaRastreo(v));
                
                entity.Property(p => p.Destino).IsRequired();
                entity.Property(p => p.Tarifa).HasColumnType("decimal(18,2)");
                
                // Mapeo del Value Object Evidencia como Owned Type
                entity.OwnsOne(p => p.Evidencia, e =>
                {
                    e.Property(ev => ev.FirmaDigital).HasColumnName("FirmaDigital");
                    e.Property(ev => ev.FotoUrl).HasColumnName("FotoUrl");
                    e.Property(ev => ev.Timestamp).HasColumnName("EvidenciaTimestamp");
                });
            });
        }
    }
}