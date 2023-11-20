using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Context;
public class ProEventosContext : IdentityDbContext<User, Role, int, 
                                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }

    public DbSet<EventoModel> Eventos { get; set; }
    public DbSet<LoteModel> Lotes { get; set; }
    public DbSet<PalestranteModel> Palestrantes { get; set; }
    public DbSet<PalestranteEventoModel> PalestratesEventos { get; set; }
    public DbSet<RedeSocialModel> RedeSociais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PalestranteEventoModel>().HasKey(PE => new { PE.EventoId, PE.PalestranteId }); //n:n
        modelBuilder.Entity<UserRole>(userRole => 
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });  
            userRole.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
            userRole.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
        });

        //keep the related entities when deleting
        // modelBuilder.Entity<PalestranteEventoModel>().HasOne(pe => pe.Evento).WithMany(e => e.PalestratesEventos).HasForeignKey(pe => pe.EventoId).OnDelete(DeleteBehavior.Restrict);

        //Relação entre Evento e RedeSocial 1:n
        modelBuilder.Entity<EventoModel>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Evento)
            .HasForeignKey(rs => rs.EventoId)
            .OnDelete(DeleteBehavior.Cascade); //delete cascade

        //Relação entre Evento e Lote 1:n
        modelBuilder.Entity<EventoModel>()
            .HasMany(e => e.Lotes)
            .WithOne(l => l.Evento)
            .HasForeignKey(l => l.EventoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PalestranteModel>().HasMany(p => p.RedesSociais).WithOne(rs => rs.Palestrante).OnDelete(DeleteBehavior.Cascade);
    }
}