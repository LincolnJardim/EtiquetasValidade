using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Repositories;

namespace Etiquetas.Application.Services
{
    public class ProdutoService
    {
        private readonly ProdutoRepository _repository;

        public ProdutoService(ProdutoRepository repository)
        {
            _repository = repository;
        }
    }
}