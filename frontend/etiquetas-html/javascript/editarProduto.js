// Espera o HTML carregar antes de iniciar a edição do produto.
document.addEventListener('DOMContentLoaded', function () {
    editarProduto()
})

async function editarProduto() {
    const parametros = new URLSearchParams(window.location.search)
    const idUrl = parametros.get('id')

    const token = exigirAutenticacao()

    if (!token) {
        return
    }

    if (idUrl === null) {
        window.alert('Produto inválido. Voltando para a lista.')
        window.location.href = 'lista_produtos.html'
        return
    }

    try {
        const resposta = await fetch(
            `https://localhost:7288/Produto/obterProdutoPorId${idUrl}`,
            {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }
        )

        if (tratarNaoAutorizado(resposta)) {
            return
        }

        if (!resposta.ok) {
            window.alert('Não foi possível carregar o produto.')
            window.location.href = 'lista_produtos.html'
            return
        }

        const produtoBanco = await resposta.json()

        const nomeProdutoInput = document.getElementById('iproduto')
        const validadeProdutoInput = document.getElementById('ivalidade')

        nomeProdutoInput.value = produtoBanco.nome
        validadeProdutoInput.value = produtoBanco.diasValidade

        const formulario = document.getElementById('icadastro')

        formulario.addEventListener('submit', async function (evento) {
            evento.preventDefault()

            const tokenAtual = exigirAutenticacao()

            if (!tokenAtual) {
                return
            }

            const nomeProduto =
                document.getElementById('iproduto').value

            const diasValidade =
                document.getElementById('ivalidade').value

            const produtoJson = {
                id: Number(idUrl),
                nome: nomeProduto,
                diasValidade: Number(diasValidade)
            }

            try {
                const respostaAtualizacao = await fetch(
                    `https://localhost:7288/Produto/atualizarProduto${idUrl}`,
                    {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': `Bearer ${tokenAtual}`
                        },
                        body: JSON.stringify(produtoJson)
                    }
                )

                if (tratarNaoAutorizado(respostaAtualizacao)) {
                    return
                }

                if (!respostaAtualizacao.ok) {
                    window.alert(
                        'O servidor recebeu a solicitação, mas não conseguiu atualizar o produto.'
                    )
                    return
                }

                window.alert(
                    `O produto ${produtoJson.nome} foi atualizado com sucesso.`
                )

                window.location.href = 'lista_produtos.html'
            } catch (erro) {
                console.error(
                    'Erro de rede: a API pode estar desligada ou fora do ar.',
                    erro
                )
            }
        })
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )
    }
}