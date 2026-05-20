using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
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

        public Producao CriarProducao(CriarProducaoDto criarProducaoDto)
        {

            
            if (criarProducaoDto.DataFabricacao > DateTime.Now)
                throw new Exception("Data de fabricação não pode ser no futuro");
            
            if (criarProducaoDto.DataFabricacao < DateTime.Now.AddYears(-1))
                throw new Exception("Data de fabricação muito antiga");

            if (criarProducaoDto.DataFabricacao == default)
                throw new Exception("Data de fabricação inválida");
            
            var produto = _produtoRepository.ObterPorId(criarProducaoDto.ProdutoId);

            if (produto == null)
                throw new Exception("Produto não encontrado");
            
            if (criarProducaoDto.QuantidadeEtiquetas <= 0)
                throw new Exception("Quantidade inválida");
            
            var producao = new Producao
            {
                Produto = produto,
                DataFabricacao = criarProducaoDto.DataFabricacao,
                QuantidadeEtiquetas = criarProducaoDto.QuantidadeEtiquetas
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

        public Producao AtualizarProducao(AtualizarProducaoDto atualizarProducaoDto)
        {
            var producaoBanco = _repository.ObterProducaoPorId(atualizarProducaoDto.Id);

            if (producaoBanco == null)
                throw new Exception("Produção não encontrado");
            
            producaoBanco.DataFabricacao = atualizarProducaoDto.DataFabricacao;
            producaoBanco.QuantidadeEtiquetas = atualizarProducaoDto.QuantidadeEtiquetas;

            producaoBanco.CalcularValidade();

            _repository.AtualizarProducao(producaoBanco);

            return producaoBanco;
        }

        public Producao DeletarProducao(int id)
        {
            var producaoBanco = _repository.ObterProducaoPorId(id);

            if (producaoBanco == null)
                throw new Exception("Produto não encontrado");
            
            _repository.DeletarProducao(producaoBanco);

            return producaoBanco;
        }

        public List<EtiquetaResponseDto> GerarEtiqueta(int id)
        {
            var producaoBanco = _repository.ObterProducaoPorId(id);

            if (producaoBanco == null)
                throw new Exception("Produção não encontrado");

            int gerarQuantidade = producaoBanco.QuantidadeEtiquetas;

            List<EtiquetaResponseDto> etiquetasgeradas = new List<EtiquetaResponseDto>();

            for (int contador = 0; contador < gerarQuantidade; contador++)
            {
                var etiqueta = new EtiquetaResponseDto
                {
                    NomeProduto = producaoBanco.Produto.Nome,

                    DataProducao = producaoBanco.DataFabricacao,

                    DataValidade = producaoBanco.DataValidade
                };

                etiquetasgeradas.Add(etiqueta);
            }
        
            return etiquetasgeradas;
        }
    }
}