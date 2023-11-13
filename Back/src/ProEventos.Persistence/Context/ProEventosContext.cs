using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Context;
public class ProEventosContext : DbContext
{
    public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

    public DbSet<EventoModel> Eventos { get; set; }
    public DbSet<LoteModel> Lotes { get; set; }
    public DbSet<PalestranteModel> Palestrantes { get; set; }
    public DbSet<PalestranteEventoModel> PalestratesEventos { get; set; }
    public DbSet<RedeSocialModel> RedeSociais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PalestranteEventoModel>().HasKey(PE => new { PE.EventoId, PE.PalestranteId }); //n:n

        //keep the related entities when deleting
        // modelBuilder.Entity<PalestranteEventoModel>().HasOne(pe => pe.Evento).WithMany(e => e.PalestratesEventos).HasForeignKey(pe => pe.EventoId).OnDelete(DeleteBehavior.Restrict);

        //delete cascade
        modelBuilder.Entity<EventoModel>().HasMany(e => e.RedesSociais).WithOne(rs => rs.Evento).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EventoModel>().HasMany(e => e.Lotes).WithOne(l => l.Evento).OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PalestranteModel>().HasMany(p => p.RedesSociais).WithOne(rs => rs.Palestrante).OnDelete(DeleteBehavior.Cascade);
    }
}