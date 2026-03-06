using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Infrastructure.Data
{
    public class EtiquetaDbContext : DbContext
    {
        public EtiquetaDbContext(DbContextOptions<EtiquetaDbContext> options) : base(options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Producao> Producoes { get; set; }
    }
}