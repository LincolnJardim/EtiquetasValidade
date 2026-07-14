using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task CriarNovoUsuarioAsync(Usuario usuario);
        Task<Usuario?> BuscarPorEmailAsync(string email);
    }
}