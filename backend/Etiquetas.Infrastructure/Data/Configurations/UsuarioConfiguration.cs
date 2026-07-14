using Etiquetas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Etiquetas.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(usuario => usuario.Id);

            builder.Property(usuario => usuario.Nome)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(usuario => usuario.Email).IsRequired().HasMaxLength(150);

            builder.HasIndex(usuario => usuario.Email).IsUnique();

            builder.Property(usuario => usuario.SenhaHash).IsRequired().HasMaxLength(500);

            builder.Property(usuario => usuario.Ativo).IsRequired().HasDefaultValue(true);
        }
    }
}