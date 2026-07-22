// Aguarda o carregamento completo da página antes de listar os produtos.
document.addEventListener('DOMContentLoaded', function () {
    listarProdutos()
})


// Função responsável por buscar os produtos na API e exibi-los na tabela.
async function listarProdutos() {
    const tabela =
        document.querySelector('#itabela tbody')

    // Limpa a tabela antes de carregar novamente os produtos.
    tabela.innerHTML = ''

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    try {
        const resposta = await fetch(
            'https://localhost:7288/Produto/listarProdutosCadastrados',
            {
                method: 'GET',

                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }
        )

        // Verifica se o token está ausente, inválido ou expirado.
        if (tratarNaoAutorizado(resposta)) {
            return
        }

        // Apresenta a mensagem retornada pela API caso a consulta falhe.
        if (!resposta.ok) {
            await mostrarErroDaApi(
                resposta,
                'Não foi possível carregar os produtos.'
            )

            return
        }

        /*
            Este bloco precisa ficar fora do if (!resposta.ok),
            pois somente deve ser executado quando a resposta
            da API for bem-sucedida.
        */
        const produtos = await resposta.json()

        console.log(produtos)

        // Exibe uma mensagem na tabela caso não existam produtos.
        if (produtos.length === 0) {
            const linha = document.createElement('tr')

            linha.innerHTML = `
                <td colspan="4">
                    Nenhum produto cadastrado.
                </td>
            `

            tabela.appendChild(linha)
            return
        }

        // Percorre a lista retornada pela API e cria uma linha para cada produto.
        for (
            let posicao = 0;
            posicao < produtos.length;
            posicao++
        ) {
            const produto = produtos[posicao]

            const linha =
                document.createElement('tr')

            linha.innerHTML = `
                <td>${produto.id}</td>
                <td>${produto.nome}</td>
                <td>${produto.diasValidade}</td>
                <td>
                    <button
                        class="btn-editar"
                        data-produto-id="${produto.id}"
                    >
                        Editar
                    </button>

                    <button
                        class="btn-excluir"
                        data-produto-id="${produto.id}"
                    >
                        Excluir
                    </button>
                </td>
            `

            const botaoEditar =
                linha.querySelector('.btn-editar')

            const botaoExcluir =
                linha.querySelector('.btn-excluir')


            // Direciona o usuário para a página de edição do produto.
            botaoEditar.addEventListener(
                'click',
                function (evento) {
                    const elementoClicado =
                        evento.currentTarget

                    const idProduto =
                        elementoClicado.dataset.produtoId

                    window.location.href =
                        `editarProdutos.html?id=${idProduto}`
                }
            )


            // Solicita a exclusão do produto selecionado.
            botaoExcluir.addEventListener(
                'click',
                function (evento) {
                    const elementoClicado =
                        evento.currentTarget

                    const idProduto =
                        elementoClicado.dataset.produtoId

                    deletarProduto(idProduto)
                }
            )

            tabela.appendChild(linha)
        }
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para carregar os produtos.'
        )
    }
}


// Função responsável por excluir um produto.
async function deletarProduto(id) {
    const confirmarExclusao = window.confirm(
        'Você deseja excluir este produto? Essa ação é permanente.'
    )

    // Interrompe a exclusão caso o usuário cancele a confirmação.
    if (!confirmarExclusao) {
        window.alert(
            'O produto não será excluído.'
        )

        return
    }

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    try {
        const resposta = await fetch(
            `https://localhost:7288/Produto/${id}`,
            {
                method: 'DELETE',

                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }
        )

        // Verifica se o token está ausente, inválido ou expirado.
        if (tratarNaoAutorizado(resposta)) {
            return
        }

        // Apresenta a mensagem retornada pela API caso a exclusão falhe.
        if (!resposta.ok) {
            await mostrarErroDaApi(
                resposta,
                'Não foi possível excluir o produto.'
            )

            return
        }

        // Este trecho somente será executado após uma exclusão bem-sucedida.
        window.alert(
            'Produto excluído com sucesso.'
        )

        /*
            Atualiza a tabela sem precisar recarregar
            completamente a página.
        */
        await listarProdutos()
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para excluir o produto.'
        )
    }
}


/*
function validarConexao() {
    window.alert('JavaScript conectado.')
}
*/