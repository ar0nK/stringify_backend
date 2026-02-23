using System;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Models;

namespace stringify_backend.Models;

public partial class StringifyDbContext : DbContext
{
    public StringifyDbContext()
    {
    }

    public StringifyDbContext(DbContextOptions<StringifyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Jogok> Jogoks { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<EgyediGitar> EgyediGitarok { get; set; }
    public virtual DbSet<Termek> Termekek { get; set; }
    public virtual DbSet<TermekKepek> TermekKepek { get; set; }
    public virtual DbSet<KedvencTermek> KedvencTermekek { get; set; }
    public virtual DbSet<Rendeles> Rendelesek { get; set; }
    public virtual DbSet<RendelesTetel> RendelesTetelek { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Jogok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("jogok");
            entity.HasIndex(e => e.Nev, "Nev");
            entity.HasIndex(e => e.Szint, "Szint").IsUnique();
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Leiras).HasMaxLength(128);
            entity.Property(e => e.Nev).HasMaxLength(32);
            entity.Property(e => e.Szint).HasColumnType("int(1)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("felhasznalo");
            entity.HasIndex(e => e.Email, "Email").IsUnique();
            entity.HasIndex(e => e.Nev, "Nev");
            entity.HasIndex(e => e.Jogosultsag, "Jogosultsag");
            entity.Property(e => e.Id).HasColumnType("int(11)").HasColumnName("Id");
            entity.Property(e => e.Aktiv).HasColumnType("int(1)").HasColumnName("Aktiv");
            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("Email");
            entity.Property(e => e.Nev).HasMaxLength(60).HasColumnName("Nev");
            entity.Property(e => e.Jelszo).HasMaxLength(64).HasColumnName("Jelszo");
            entity.Property(e => e.Jogosultsag).HasColumnType("int(1)").HasColumnName("Jogosultsag");
            entity.Property(e => e.Salt).HasMaxLength(64).HasColumnName("SALT");
            entity.HasOne(d => d.JogosultsagNavigation)
                .WithMany(p => p.Users)
                .HasPrincipalKey(p => p.Szint)
                .HasForeignKey(d => d.Jogosultsag)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("felhasznalo_ibfk_1");
        });

        modelBuilder.Entity<EgyediGitar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("egyedi_gitar");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FelhasznaloId).HasColumnType("int(11)");
            entity.Property(e => e.TestformaId).HasColumnType("int(11)");
            entity.Property(e => e.NeckId).HasColumnType("int(11)");
            entity.Property(e => e.FinishId).HasColumnType("int(11)");
            entity.Property(e => e.PickguardId).HasColumnType("int(11)");
            entity.Property(e => e.Letrehozva).HasColumnType("datetime");
        });

        modelBuilder.Entity<Termek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("termek");
            entity.Property(e => e.Id).HasColumnType("int(11)").HasColumnName("Id");
            entity.Property(e => e.Nev).HasMaxLength(150).HasColumnName("Nev");
            entity.Property(e => e.Leiras).HasColumnType("text").HasColumnName("Leiras");
            entity.Property(e => e.RovidLeiras).HasMaxLength(255).HasColumnName("RovidLeiras");
            entity.Property(e => e.Ar).HasColumnType("int(11)").HasColumnName("Ar");
            entity.Property(e => e.Elerheto).HasColumnType("tinyint(1)").HasColumnName("Elerheto");
            entity.Property(e => e.GitarTipusId).HasColumnType("int(11)").HasColumnName("GitarTipusId");
            entity.Property(e => e.Letrehozva).HasColumnType("datetime").HasColumnName("Letrehozva");
            entity.HasOne(e => e.TermekKepek)
                .WithOne(k => k.Termek)
                .HasForeignKey<TermekKepek>(k => k.TermekId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("termek_kepek_ibfk_1");
        });

        modelBuilder.Entity<TermekKepek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("termek_kepek");
            entity.Property(e => e.Id).HasColumnType("int(11)").HasColumnName("Id");
            entity.Property(e => e.TermekId).HasColumnType("int(11)").HasColumnName("TermekId");
            entity.Property(e => e.Kep1).HasMaxLength(255).HasColumnName("kep1");
            entity.Property(e => e.Kep2).HasMaxLength(255).HasColumnName("kep2");
            entity.Property(e => e.Kep3).HasMaxLength(255).HasColumnName("kep3");
            entity.Property(e => e.Kep4).HasMaxLength(255).HasColumnName("kep4");
            entity.Property(e => e.Kep5).HasMaxLength(255).HasColumnName("kep5");
            entity.HasIndex(e => e.TermekId).HasDatabaseName("TermekId");
        });

        modelBuilder.Entity<KedvencTermek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("kedvenc_termek");
            entity.Property(e => e.Id).HasColumnType("int(11)").HasColumnName("Id");
            entity.Property(e => e.FelhasznaloId).HasColumnType("int(11)").HasColumnName("FelhasznaloId");
            entity.Property(e => e.TermekId).HasColumnType("int(11)").HasColumnName("TermekId");
            entity.Property(e => e.Letrehozva).HasColumnType("datetime").HasColumnName("Letrehozva");
            entity.HasIndex(e => new { e.FelhasznaloId, e.TermekId }).IsUnique().HasDatabaseName("unique_kedvenc");
            entity.HasIndex(e => e.FelhasznaloId).HasDatabaseName("FelhasznaloId");
            entity.HasIndex(e => e.TermekId).HasDatabaseName("fk_kedvenc_termek");
            entity.HasOne(e => e.Felhasznalo)
                .WithMany()
                .HasForeignKey(e => e.FelhasznaloId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_kedvenc_felhasznalo");
            entity.HasOne(e => e.Termek)
                .WithMany()
                .HasForeignKey(e => e.TermekId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_kedvenc_termek");
        });

        modelBuilder.Entity<Rendeles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("rendeles");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.FelhasznaloId).HasColumnType("int(11)");
            entity.Property(e => e.Osszeg).HasColumnType("int(11)");
            entity.Property(e => e.Status).HasMaxLength(32);
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.HasIndex(e => e.FelhasznaloId).HasDatabaseName("FelhasznaloId");
            entity.HasMany(e => e.Tetelek)
                .WithOne(t => t.Rendeles)
                .HasForeignKey(t => t.RendelesId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("rendeles_tetel_ibfk_1");
        });

        modelBuilder.Entity<RendelesTetel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("rendeles_tetel");
            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.RendelesId).HasColumnType("int(11)");
            entity.Property(e => e.TermekId).HasColumnType("int(11)");
            entity.Property(e => e.EgyediGitarId).HasColumnType("int(11)");
            entity.HasIndex(e => e.RendelesId).HasDatabaseName("RendelesId");
            entity.HasOne(e => e.Termek)
                .WithMany()
                .HasForeignKey(e => e.TermekId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("rendeles_tetel_ibfk_2");
            entity.HasOne(e => e.EgyediGitar)
                .WithMany()
                .HasForeignKey(e => e.EgyediGitarId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("rendeles_tetel_ibfk_3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}