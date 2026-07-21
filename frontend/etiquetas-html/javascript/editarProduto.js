// Espera o HTML carregar antes de iniciar a edição do produto.
document.addEventListener('DOMContentLoaded', function () {
    editarProduto()
})

// Função responsável por carregar os dados do produto e enviar as alterações para a API.
async function editarProduto() {
    const parametros =
        new URLSearchParams(window.location.search)

    const idUrl = parametros.get('id')

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    // Impede a edição quando o identificador do produto não foi informado ou é inválido.
    if (
        !idUrl ||
        Number.isNaN(Number(idUrl)) ||
        Number(idUrl) <= 0
    ) {
        window.alert(
            'Produto inválido. Voltando para a lista.'
        )

        window.location.href = 'lista_produtos.html'

        return
    }

    try {
        // Busca na API os dados do produto que será editado.
        const resposta = await fetch(
            `https://localhost:7288/Produto/obterProdutoPorId${idUrl}`,
            {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }
        )

        // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
        if (tratarNaoAutorizado(resposta)) {
            return
        }

        // Impede que o formulário seja liberado quando o produto não for carregado.
        if (!resposta.ok) {
            window.alert(
                'Não foi possível carregar o produto.'
            )

            window.location.href = 'lista_produtos.html'

            return
        }

        const produtoBanco = await resposta.json()

        // Captura dos elementos do formulário.
        const inputNomeProduto =
            document.getElementById('iproduto')

        const inputDiasValidade =
            document.getElementById('ivalidade')

        const formulario =
            document.getElementById('icadastro')

        // Preenche os campos com os dados recebidos da API.
        inputNomeProduto.value = produtoBanco.nome

        inputDiasValidade.value =
            produtoBanco.diasValidade

        // Criação do evento submit e função assíncrona com alteração no comportamento padrão de recarregar a página.
        formulario.addEventListener(
            'submit',
            async function (evento) {
                evento.preventDefault()

                // Verifica novamente a autenticação no momento da atualização.
                const tokenAtual = exigirAutenticacao()

                // Interrompe a execução caso a sessão não exista mais.
                if (!tokenAtual) {
                    return
                }

                /*
                    Captura os valores somente no momento do envio,
                    garantindo que as alterações feitas pelo usuário
                    sejam utilizadas.
                */
                const nomeProduto =
                    inputNomeProduto.value.trim()

                const diasValidadeTexto =
                    inputDiasValidade.value.trim()

                const diasValidade =
                    Number(diasValidadeTexto)

                // Valida os dados antes de montar o objeto e chamar a API.
                const formularioValido =
                    validarFormularioProduto(
                        nomeProduto,
                        diasValidadeTexto,
                        diasValidade
                    )

                if (!formularioValido) {
                    return
                }

                // Montagem do objeto JavaScript que será convertido para JSON.
                const produtoJson = {
                    nome: nomeProduto,
                    diasValidade: diasValidade
                }

                try {
                    // Envia os dados atualizados para a API.
                    const respostaAtualizacao = await fetch(
                        `https://localhost:7288/Produto/atualizarProduto${idUrl}`,
                        {
                            method: 'PUT',
                            headers: {
                                'Content-Type': 'application/json',
                                'Authorization':
                                    `Bearer ${tokenAtual}`
                            },
                            body: JSON.stringify(produtoJson)
                        }
                    )

                    // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
                    if (
                        tratarNaoAutorizado(
                            respostaAtualizacao
                        )
                    ) {
                        return
                    }

                    // Impede a continuação caso a API não consiga atualizar o produto.
                    if (!respostaAtualizacao.ok) {
                        window.alert(
                            'O servidor recebeu a solicitação, mas não conseguiu atualizar o produto.'
                        )

                        return
                    }

                    // A execução somente chega aqui quando o produto foi atualizado com sucesso.
                    window.alert(
                        `O produto ${produtoJson.nome} foi atualizado com sucesso.`
                    )

                    window.location.href =
                        'lista_produtos.html'
                } catch (erro) {
                    console.error(
                        'Erro de rede: a API pode estar desligada ou fora do ar.',
                        erro
                    )

                    window.alert(
                        'Não foi possível conectar ao sistema para atualizar o produto.'
                    )
                }
            }
        )
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para carregar o produto.'
        )
    }
}