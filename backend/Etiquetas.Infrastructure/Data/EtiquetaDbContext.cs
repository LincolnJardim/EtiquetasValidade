using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data.Configurations;

namespace Etiquetas.Infrastructure.Data
{
    public class EtiquetaDbContext : DbContext
    {
        public EtiquetaDbContext(DbContextOptions<EtiquetaDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Producao> Producoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }
    }
}