using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Services
{
    public class ProducaoService
    {
        private readonly IProducaoRepository _repository;

        private readonly IProdutoRepository
            _produtoRepository;


        public ProducaoService(
            IProducaoRepository repository,
            IProdutoRepository produtoRepository
        )
        {
            _repository = repository;
            _produtoRepository = produtoRepository;
        }


        // Função responsável por validar e criar uma nova produção.
        public Producao CriarProducao(
            CriarProducaoDto criarProducaoDto
        )
        {
            ValidarProdutoId(
                criarProducaoDto.ProdutoId
            );

            var dataFabricacao =
                ValidarDataFabricacao(
                    criarProducaoDto.DataFabricacao
                );

            ValidarQuantidadeEtiquetas(
                criarProducaoDto.QuantidadeEtiquetas
            );

            var produto =
                _produtoRepository.ObterPorId(
                    criarProducaoDto.ProdutoId
                );

            if (produto == null)
            {
                throw new KeyNotFoundException(
                    "Produto não encontrado."
                );
            }

            var producao = new Producao
            {
                Produto = produto,

                DataFabricacao =
                    dataFabricacao,

                QuantidadeEtiquetas =
                    criarProducaoDto
                        .QuantidadeEtiquetas
            };

            producao.CalcularValidade();

            _repository.AdicionarProducao(producao);

            return producao;
        }


        // Função responsável por buscar uma produção pelo identificador.
        public Producao? ObterProducaoPorId(int id)
        {
            return _repository.ObterProducaoPorId(id);
        }


        // Função responsável por listar as produções cadastradas.
        public List<Producao> ListarProducoes()
        {
            return _repository.ListarProducoes();
        }


        // Função responsável por atualizar uma produção existente.
        public Producao? AtualizarProducao(
            int id,
            AtualizarProducaoDto atualizarProducaoDto
        )
        {
            var producaoBanco =
                _repository.ObterProducaoPorId(id);

            if (producaoBanco == null)
            {
                return null;
            }

            var dataFabricacao =
                ValidarDataFabricacao(
                    atualizarProducaoDto
                        .DataFabricacao
                );

            ValidarQuantidadeEtiquetas(
                atualizarProducaoDto
                    .QuantidadeEtiquetas
            );

            producaoBanco.DataFabricacao =
                dataFabricacao;

            producaoBanco.QuantidadeEtiquetas =
                atualizarProducaoDto
                    .QuantidadeEtiquetas;

            producaoBanco.CalcularValidade();

            _repository.AtualizarProducao(
                producaoBanco
            );

            return producaoBanco;
        }


        // Função responsável por excluir uma produção existente.
        public bool DeletarProducao(int id)
        {
            var producaoBanco =
                _repository.ObterProducaoPorId(id);

            if (producaoBanco == null)
            {
                return false;
            }

            _repository.DeletarProducao(
                producaoBanco
            );

            return true;
        }


        // Função responsável por gerar as etiquetas relacionadas a uma produção.
        public List<EtiquetaResponseDto>?
            GerarEtiqueta(int id)
        {
            var producaoBanco =
                _repository.ObterProducaoPorId(id);

            if (producaoBanco == null)
            {
                return null;
            }

            var etiquetasGeradas =
                new List<EtiquetaResponseDto>();

            for (
                var contador = 0;
                contador <
                    producaoBanco.QuantidadeEtiquetas;
                contador++
            )
            {
                var etiqueta =
                    new EtiquetaResponseDto
                    {
                        NomeProduto =
                            producaoBanco
                                .Produto
                                .Nome,

                        DataProducao =
                            producaoBanco
                                .DataFabricacao,

                        DataValidade =
                            producaoBanco
                                .DataValidade
                    };

                etiquetasGeradas.Add(etiqueta);
            }

            return etiquetasGeradas;
        }


        // Função responsável por validar o identificador do produto.
        private static void ValidarProdutoId(
            int produtoId
        )
        {
            if (produtoId <= 0)
            {
                throw new ValidationException(
                    "Selecione um produto válido."
                );
            }
        }


        // Função responsável por validar e normalizar a data de fabricação.
        private static DateTime
            ValidarDataFabricacao(
                DateTime? dataFabricacao
            )
        {
            if (!dataFabricacao.HasValue)
            {
                throw new ValidationException(
                    "Informe a data de fabricação."
                );
            }

            /*
                Remove eventuais informações de horário,
                pois a regra da produção trabalha somente
                com a data.
            */
            var dataNormalizada =
                dataFabricacao.Value.Date;

            if (dataNormalizada > DateTime.Today)
            {
                throw new ValidationException(
                    "A data de fabricação não pode ser futura."
                );
            }

            return dataNormalizada;
        }


        // Função responsável por validar a quantidade de etiquetas.
        private static void
            ValidarQuantidadeEtiquetas(
                int quantidadeEtiquetas
            )
        {
            if (quantidadeEtiquetas <= 0)
            {
                throw new ValidationException(
                    "A quantidade de etiquetas deve ser maior que zero."
                );
            }
        }
    }
}