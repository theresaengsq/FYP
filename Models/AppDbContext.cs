using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace P06.Models
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<CakeOrder> CakeOrder { get; set; }
        public virtual DbSet<MugOrder> MugOrder { get; set; }
        public virtual DbSet<Pokedex> Pokedex { get; set; }
        public virtual DbSet<ShirtOrder> ShirtOrder { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CakeOrder>(entity =>
            {
                entity.Property(e => e.Flavor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Greeting)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pokedex)
                    .WithMany(p => p.CakeOrder)
                    .HasForeignKey(d => d.PokedexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CakeOrder__Poked__2F10007B");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany(p => p.CakeOrder)
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CakeOrder__UserC__300424B4");
            });

            modelBuilder.Entity<MugOrder>(entity =>
            {
                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pokedex)
                    .WithMany(p => p.MugOrder)
                    .HasForeignKey(d => d.PokedexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MugOrder__Pokede__276EDEB3");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany(p => p.MugOrder)
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MugOrder__UserCo__286302EC");
            });

            modelBuilder.Entity<Pokedex>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ShirtOrder>(entity =>
            {
                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Pokedex)
                    .WithMany(p => p.ShirtOrder)
                    .HasForeignKey(d => d.PokedexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShirtOrde__Poked__2B3F6F97");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany(p => p.ShirtOrder)
                    .HasForeignKey(d => d.UserCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShirtOrde__UserC__2C3393D0");
            });
        }
    }
}
