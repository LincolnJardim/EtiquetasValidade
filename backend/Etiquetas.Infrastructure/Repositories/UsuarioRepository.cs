using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Etiquetas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EtiquetaDbContext _context;

        public UsuarioRepository(EtiquetaDbContext context)
        {
            _context = context;
        }

        public async Task CriarNovoUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .SingleOrDefaultAsync(usuario => usuario.Email == email);
        }
    }
}