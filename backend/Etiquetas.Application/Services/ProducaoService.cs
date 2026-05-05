using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Services
{
    public class ProducaoService
    {
        private readonly IProducaoRepository _repository;
        private readonly IProdutoRepository _produtoRepository;


        public ProducaoService(
        IProducaoRepository repository,
        IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository;
        }

        public Producao CriarProducao(Producao producao)
        {
            if (producao.DataFabricacao > DateTime.Now)
                throw new Exception("Data de fabricação não pode ser no futuro");
            
            if (producao.DataFabricacao < DateTime.Now.AddYears(-1))
                throw new Exception("Data de fabricação muito antiga");

            if (producao.DataFabricacao == default)
                throw new Exception("Data de fabricação inválida");
            
            var produto = _produtoRepository.ObterPorId(producao.Produto.Id);

            if (produto == null)
                throw new Exception("Produto não encontrado");
            
            producao.CalcularValidade();
            
            _repository.AdicionarProducao(producao);

            return producao;
        }

        public Producao ObterProduçoesPorId(int id)
        {
            return _repository.ObterProducaoPorId(id);
        }

        public List<Producao> ListarProducoes()
        {
            return _repository.ListarProducoes();
        }
    }
}