using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MTBMusics.Models;

namespace MTBMusics.Context;

public partial class MTBMusicsContext : DbContext
{
    public MTBMusicsContext()
    {
    }

    public MTBMusicsContext(DbContextOptions<MTBMusicsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Musica> Musica { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=D09S20-1252894\\SQLEXPRESS2;Database=MTBMusics;User Id=sa;Password=Senai@134;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PK__Genero__0F8349882A4831A9");
        });

        modelBuilder.Entity<Musica>(entity =>
        {
            entity.HasKey(e => e.IdMusica).HasName("PK__Musica__F026ACA72DF40EEA");

            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(getdate())");

            entity.HasMany(d => d.IdGenero).WithMany(p => p.IdMusica)
                .UsingEntity<Dictionary<string, object>>(
                    "MusicaGenero",
                    r => r.HasOne<Genero>().WithMany()
                        .HasForeignKey("IdGenero")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MusicaGenero_Genero"),
                    l => l.HasOne<Musica>().WithMany()
                        .HasForeignKey("IdMusica")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MusicaGenero_Musica"),
                    j =>
                    {
                        j.HasKey("IdMusica", "IdGenero");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF973C8BE4E6");

            entity.ToTable(tb => tb.HasTrigger("TR_UsuarioSoftDelete"));

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DataCadastro).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
