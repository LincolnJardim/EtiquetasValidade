using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Etiquetas.Application.Exceptions;

namespace Etiquetas.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        private readonly IProducaoRepository
            _producaoRepository;


        public ProdutoService(
            IProdutoRepository repository,
            IProducaoRepository producaoRepository
        )
        {
            _repository = repository;

            _producaoRepository =
                producaoRepository;
        }

        // Função responsável por validar e criar um novo produto.
        public Produto CriarProduto(CriarProdutoDto produtoDto)
        {
            var nomeNormalizado =
                ValidarENormalizarNome(produtoDto.Nome);

            ValidarDiasValidade(produtoDto.DiasValidade);

            var novoProduto = new Produto
            {
                Nome = nomeNormalizado,
                DiasValidade = produtoDto.DiasValidade
            };

            _repository.Adicionar(novoProduto);

            return novoProduto;
        }


        // Função responsável por buscar um produto pelo identificador.
        public Produto? ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }


        // Função responsável por listar todos os produtos cadastrados.
        public List<Produto> ListarTodos()
        {
            return _repository.ListarTodos();
        }


        // Função responsável por atualizar um produto existente.
        public Produto? AtualizarProduto(
            int id,
            AtualizarProdutoDto atualizarProdutoDto
        )
        {
            var produtoBanco =
                _repository.ObterPorId(id);

            // Retorna null para o Controller responder com 404.
            if (produtoBanco == null)
            {
                return null;
            }

            var nomeNormalizado =
                ValidarENormalizarNome(
                    atualizarProdutoDto.Nome
                );

            ValidarDiasValidade(
                atualizarProdutoDto.DiasValidade
            );

            produtoBanco.Nome = nomeNormalizado;

            produtoBanco.DiasValidade =
                atualizarProdutoDto.DiasValidade;

            _repository.AtualizarProduto(produtoBanco);

            return produtoBanco;
        }


        // Função responsável por excluir um produto existente.
        // Função responsável por excluir um produto existente.
        public bool DeletarProduto(int id)
        {
            var produtoBanco =
                _repository.ObterPorId(id);

            // Retorna false para o Controller responder com 404.
            if (produtoBanco == null)
            {
                return false;
            }

            /*
                Impede que o produto seja excluído quando existem
                produções relacionadas a ele.
            */
            var possuiProducoes =
                _producaoRepository
                    .ExisteProducaoParaProduto(id);

            if (possuiProducoes)
            {
                throw new ConflitoException(
                    "Não é possível excluir o produto porque existem produções vinculadas."
                );
            }

            _repository.DeletarProduto(
                produtoBanco
            );

            return true;
        }


        // Função responsável por normalizar e validar o nome do produto.
        private static string ValidarENormalizarNome(
            string nome
        )
        {
            var nomeNormalizado =
                nome?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nomeNormalizado))
            {
                throw new ValidationException(
                    "Informe o nome do produto."
                );
            }

            if (nomeNormalizado.Length < 2)
            {
                throw new ValidationException(
                    "O nome do produto deve possuir pelo menos 2 caracteres."
                );
            }

            if (nomeNormalizado.Length > 100)
            {
                throw new ValidationException(
                    "O nome do produto deve possuir no máximo 100 caracteres."
                );
            }

            return nomeNormalizado;
        }


        // Função responsável por validar os dias de validade.
        private static void ValidarDiasValidade(
            int diasValidade
        )
        {
            if (diasValidade <= 0)
            {
                throw new ValidationException(
                    "A validade deve ser maior que zero."
                );
            }
        }
    }
}