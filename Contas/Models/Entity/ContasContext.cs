using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Contas.Models.Entity
{
    public partial class ContasContext : DbContext
    {
        public virtual DbSet<Cartao> Cartao { get; set; }
        public virtual DbSet<Consolidado> Consolidado { get; set; }
        public virtual DbSet<GastoCartao> GastoCartao { get; set; }
        public virtual DbSet<Movimentacao> Movimentacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Contas;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cartao>(entity =>
            {
                entity.ToTable("cartao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Credito)
                    .HasColumnName("credito")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(20);

                entity.Property(e => e.Sigla)
                    .IsRequired()
                    .HasColumnName("sigla")
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<Consolidado>(entity =>
            {
                entity.ToTable("consolidado");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(20);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnName("valor")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<GastoCartao>(entity =>
            {
                entity.ToTable("gasto_cartao");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasColumnType("text");

                entity.Property(e => e.IdCartao).HasColumnName("id_cartao");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(20);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.GastoCartao)
                    .HasForeignKey<GastoCartao>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_gasto_cartao_cartao");
            });

            modelBuilder.Entity<Movimentacao>(entity =>
            {
                entity.ToTable("movimentacao");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("date");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasColumnType("text");

                entity.Property(e => e.IdCartao).HasColumnName("id_cartao");

                entity.Property(e => e.Loja)
                    .HasColumnName("loja")
                    .HasMaxLength(20);

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(20);

                entity.Property(e => e.Posicao).HasColumnName("posicao");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(20);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(20);

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.IdCartaoNavigation)
                    .WithMany(p => p.Movimentacao)
                    .HasForeignKey(d => d.IdCartao)
                    .HasConstraintName("FK_movimentacao_cartao");
            });
        }
    }
}
