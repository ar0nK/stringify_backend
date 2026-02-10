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
            entity.HasIndex(e => e.Telefonszam, "Telefonszam");
            entity.HasIndex(e => e.Nev, "Nev");
            entity.HasIndex(e => e.Jogosultsag, "Jogosultsag");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("Id");

            entity.Property(e => e.Aktiv)
                .HasColumnType("int(1)")
                .HasColumnName("Aktiv");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("Email");

            entity.Property(e => e.Telefonszam)
                .HasMaxLength(30)
                .HasColumnName("Telefonszam");

            entity.Property(e => e.Nev)
                .HasMaxLength(60)
                .HasColumnName("Nev");

            entity.Property(e => e.Jelszo)
                .HasMaxLength(64)
                .HasColumnName("Jelszo");

            entity.Property(e => e.Jogosultsag)
                .HasColumnType("int(1)")
                .HasColumnName("Jogosultsag");

            entity.Property(e => e.Salt)
                .HasMaxLength(64)
                .HasColumnName("SALT");

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
            entity.Property(e => e.BodyShapeId).HasColumnType("int(11)");
            entity.Property(e => e.BodyWoodId).HasColumnType("int(11)");
            entity.Property(e => e.NeckWoodId).HasColumnType("int(11)");
            entity.Property(e => e.NeckPickupId).HasColumnType("int(11)");
            entity.Property(e => e.MiddlePickupId).HasColumnType("int(11)");
            entity.Property(e => e.BridgePickupId).HasColumnType("int(11)");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
