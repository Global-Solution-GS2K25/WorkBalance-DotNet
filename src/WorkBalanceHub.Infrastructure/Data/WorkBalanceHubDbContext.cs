using Microsoft.EntityFrameworkCore;
using WorkBalanceHub.Domain.Entities;

namespace WorkBalanceHub.Infrastructure.Data;

public class WorkBalanceHubDbContext : DbContext
{
    public WorkBalanceHubDbContext(DbContextOptions<WorkBalanceHubDbContext> options) : base(options)
    {
    }

    public DbSet<Colaborador> Colaboradores { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<CheckIn> CheckIns { get; set; }
    public DbSet<EstacaoTrabalho> EstacoesTrabalho { get; set; }
    public DbSet<LeituraAmbiente> LeiturasAmbiente { get; set; }
    public DbSet<PlanoAcao> PlanosAcao { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações de Colaborador
        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.ToTable("Colaboradores");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
            entity.Property(c => c.Cargo).IsRequired().HasMaxLength(100);
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasOne(c => c.Equipe)
                  .WithMany(e => e.Colaboradores)
                  .HasForeignKey(c => c.EquipeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurações de Equipe
        modelBuilder.Entity<Equipe>(entity =>
        {
            entity.ToTable("Equipes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
        });

        // Configurações de CheckIn
        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.ToTable("CheckIns");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Humor).IsRequired();
            entity.Property(c => c.NivelEstresse).IsRequired();
            entity.Property(c => c.QualidadeSono).IsRequired();
            entity.Property(c => c.SintomasFisicos).HasMaxLength(500);
            entity.Property(c => c.Observacoes).HasMaxLength(1000);
            entity.HasOne(c => c.Colaborador)
                  .WithMany(c => c.CheckIns)
                  .HasForeignKey(c => c.ColaboradorId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(c => new { c.ColaboradorId, c.DataCheckIn.Date });
        });

        // Configurações de EstacaoTrabalho
        modelBuilder.Entity<EstacaoTrabalho>(entity =>
        {
            entity.ToTable("EstacoesTrabalho");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Localizacao).HasMaxLength(200);
            entity.Property(e => e.Descricao).HasMaxLength(500);
        });

        // Configurações de LeituraAmbiente
        modelBuilder.Entity<LeituraAmbiente>(entity =>
        {
            entity.ToTable("LeiturasAmbiente");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Temperatura).HasPrecision(5, 2);
            entity.Property(l => l.NivelRuido).HasPrecision(5, 2);
            entity.Property(l => l.Luminosidade).HasPrecision(8, 2);
            entity.HasOne(l => l.EstacaoTrabalho)
                  .WithMany(e => e.LeiturasAmbiente)
                  .HasForeignKey(l => l.EstacaoTrabalhoId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(l => new { l.EstacaoTrabalhoId, l.DataLeitura });
        });

        // Configurações de PlanoAcao
        modelBuilder.Entity<PlanoAcao>(entity =>
        {
            entity.ToTable("PlanosAcao");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Descricao).IsRequired().HasMaxLength(1000);
            entity.Property(p => p.Observacoes).HasMaxLength(1000);
            entity.Property(p => p.Status).HasConversion<int>();
            entity.HasOne(p => p.Equipe)
                  .WithMany(e => e.PlanosAcao)
                  .HasForeignKey(p => p.EquipeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

