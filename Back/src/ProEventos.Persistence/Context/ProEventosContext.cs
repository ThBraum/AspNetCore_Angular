using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Context;
public class ProEventosContext : DbContext
{
    public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

    public DbSet<EventoModel> Eventos { get; set; }
    public DbSet<LoteModel> Lotes { get; set; }
    public DbSet<PalestranteModel> Palestrantes { get; set; }
    public DbSet<PalestrateEventoModel> PalestratesEventos { get; set; }
    public DbSet<RedeSocialModel> RedeSociais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PalestrateEventoModel>().HasKey(PE => new {PE.EventoId, PE.PalestranteId}); //n:n
    }
}