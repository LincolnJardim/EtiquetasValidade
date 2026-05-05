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

        public Producao CriarProducao(int produtoId, DateTime dataFabricacao, int quantidade)
        {

            
            if (dataFabricacao > DateTime.Now)
                throw new Exception("Data de fabricação não pode ser no futuro");
            
            if (dataFabricacao < DateTime.Now.AddYears(-1))
                throw new Exception("Data de fabricação muito antiga");

            if (dataFabricacao == default)
                throw new Exception("Data de fabricação inválida");
            
            var produto = _produtoRepository.ObterPorId(produtoId);

            if (produto == null)
                throw new Exception("Produto não encontrado");
            
            if (quantidade <= 0)
                throw new Exception("Quantidade inválida");
            
            var producao = new Producao
            {
                Produto = produto,
                DataFabricacao = dataFabricacao,
                QuantidadeEtiquetas = quantidade
            };

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