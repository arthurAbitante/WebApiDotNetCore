using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CondicaoPagamento> CondicoesPagamentos { get; set; }
        public DbSet<HistoricoPreco> HistoricoPrecos { get; set; }
        public DbSet<RelatorioPagamento> RelatorioPagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Produto>()
            .Property(p => p.Descricao)
            .HasMaxLength(45);

            modelBuilder.Entity<Produto>()
            .Property(p => p.Sku)
            .HasMaxLength(45);


            modelBuilder.Entity<Cliente>()
            .Property(c => c.cnpj)
            .HasMaxLength(45);

            modelBuilder.Entity<Cliente>()
            .Property(c => c.razaoSocial)
            .HasMaxLength(45);


            modelBuilder.Entity<CondicaoPagamento>()
            .Property(cp => cp.descricao)
            .HasMaxLength(45);

            modelBuilder.Entity<CondicaoPagamento>()
            .Property(cp => cp.dias)
            .HasMaxLength(45);

            modelBuilder.Entity<HistoricoPreco>().HasOne(p => p.Produto);
            modelBuilder.Entity<Cliente>().HasMany(c => c.RelatorioPagamento);
            modelBuilder.Entity<RelatorioPagamento>().HasOne(rp => rp.HistorioPreco);
            modelBuilder.Entity<RelatorioPagamento>().HasOne(rp => rp.CondicaoPagamento);
        }
    }
}
