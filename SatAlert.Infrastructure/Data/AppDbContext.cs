using Microsoft.EntityFrameworkCore;
using SatAlert.Domain.Entities;

namespace SatAlert.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("SAT_Usuarios");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnType("RAW(16)");
            e.Property(u => u.Nome).HasMaxLength(100).IsRequired();
            e.Property(u => u.Email).HasMaxLength(255).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Telefone).HasMaxLength(20).IsRequired();
            e.Property(u => u.RegiaoInteresse).HasMaxLength(100).IsRequired();
            e.Property(u => u.Ativo).HasColumnType("NUMBER(1)");
            e.Property(u => u.CriadoEm).HasColumnType("TIMESTAMP");
            e.HasMany(u => u.Notificacoes)
                .WithOne(n => n.Usuario)
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notificacao>(e =>
        {
            e.ToTable("SAT_Notificacoes");
            e.HasKey(n => n.Id);
            e.Property(n => n.Id).HasColumnType("RAW(16)");
            e.Property(n => n.UsuarioId).HasColumnType("RAW(16)");
            e.Property(n => n.Mensagem).HasMaxLength(1000).IsRequired();
            e.Property(n => n.DataEnvio).HasColumnType("TIMESTAMP");
            e.Property(n => n.Lida).HasColumnType("NUMBER(1)");
        });
    }
}
