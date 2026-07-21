// Manipulação do DOM para esperar a página HTML carregar por completo antes de iniciar a edição da produção.
document.addEventListener('DOMContentLoaded', async function () {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    // Aguarda o carregamento dos produtos e armazena o resultado da operação.
    const produtosCarregados = await carregarProdutos()

    // Impede que a página continue caso os produtos não tenham sido carregados.
    if (!produtosCarregados) {
        return
    }

    await editarProducao()
})


// Função responsável por buscar os produtos cadastrados na API e preencher o campo de seleção.
async function carregarProdutos() {
    const dropdownProdutos =
        document.getElementById('idrop-produto')

    const token = exigirAutenticacao()

    // Interrompe o carregamento caso a sessão não exista mais.
    if (!token) {
        return false
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

        // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
        if (tratarNaoAutorizado(resposta)) {
            return false
        }

        // Impede a continuação caso a API não consiga carregar os produtos.
        if (!resposta.ok) {
            window.alert(
                'Não foi possível carregar os produtos disponíveis.'
            )

            return false
        }

        const listaProdutos = await resposta.json()

        // Impede a continuação quando nenhum produto for encontrado.
        if (listaProdutos.length === 0) {
            window.alert(
                'Nenhum produto cadastrado foi encontrado.'
            )

            return false
        }

        // Percorre a lista de produtos recebida e adiciona cada produto ao campo select.
        for (const produto of listaProdutos) {
            const item = document.createElement('option')

            item.text =
                `${produto.nome} (${produto.diasValidade} dias)`

            item.value = produto.id

            dropdownProdutos.appendChild(item)
        }

        // Informa que os produtos foram carregados corretamente.
        return true
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para carregar os produtos.'
        )

        return false
    }
}


// Função responsável por carregar os dados da produção e enviar as alterações para a API.
async function editarProducao() {
    const parametros =
        new URLSearchParams(window.location.search)

    const idUrl = parametros.get('id')

    const idProducao = Number(idUrl)

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    // Impede a edição quando o identificador da produção não foi informado ou é inválido.
    if (
        !idUrl ||
        Number.isNaN(idProducao) ||
        !Number.isInteger(idProducao) ||
        idProducao <= 0
    ) {
        window.alert(
            'Produção inválida. Voltando para a lista.'
        )

        window.location.href =
            'listaProducoes.html'

        return
    }

    try {
        // Busca os dados da produção que será editada.
        const resposta = await fetch(
            `https://localhost:7288/Producao/obterProducaoPorId${idUrl}`,
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

        // Impede que o formulário de edição seja liberado quando a produção não for carregada.
        if (!resposta.ok) {
            window.alert(
                'Não foi possível carregar a produção.'
            )

            window.location.href =
                'listaProducoes.html'

            return
        }

        const producaoBanco =
            await resposta.json()

        // Remove as informações de horário recebidas da API para preencher o input do tipo date.
        const dataFabricacaoFormatada =
            producaoBanco.dataFabricacao.split('T')[0]

        // Captura dos elementos do formulário.
        const dropdownProduto =
            document.getElementById('idrop-produto')

        const inputDataFabricacao =
            document.getElementById('idatafabricacao')

        const inputQuantidadeEtiquetas =
            document.getElementById('ietiquetas')

        const formulario =
            document.getElementById('icadastro')

        // Preenche os campos com os dados da produção recebidos da API.
        dropdownProduto.value =
            producaoBanco.produto.id

        // Impede a continuação caso o produto relacionado à produção não tenha sido encontrado.
        if (dropdownProduto.selectedIndex === -1) {
            window.alert(
                'O produto relacionado a esta produção não foi encontrado.'
            )

            window.location.href =
                'listaProducoes.html'

            return
        }

        /*
            O produto não pode ser alterado durante a edição da produção,
            por isso o campo permanece desabilitado.
        */
        dropdownProduto.disabled = true

        inputDataFabricacao.value =
            dataFabricacaoFormatada

        // Impede a seleção de uma data futura no campo.
        inputDataFabricacao.max =
            obterDataAtualFormatada()

        inputQuantidadeEtiquetas.value =
            producaoBanco.quantidadeEtiquetas

        // Criação do evento submit e função assíncrona com alteração no comportamento padrão de recarregar a página.
        formulario.addEventListener(
            'submit',
            async function (evento) {
                evento.preventDefault()

                // Verifica novamente a autenticação no momento do envio do formulário.
                const tokenAtual =
                    exigirAutenticacao()

                // Interrompe a execução caso a sessão não exista mais.
                if (!tokenAtual) {
                    return
                }

                // Captura dos valores informados no formulário.
                const produtoIdTexto =
                    dropdownProduto.value

                const produtoId =
                    Number(produtoIdTexto)

                const dataFabricacao =
                    inputDataFabricacao.value

                const quantidadeEtiquetasTexto =
                    inputQuantidadeEtiquetas.value.trim()

                const quantidadeEtiquetas =
                    Number(quantidadeEtiquetasTexto)

                // Valida os dados antes de montar o objeto e enviar para a API.
                const formularioValido =
                    validarFormularioProducao(
                        produtoIdTexto,
                        produtoId,
                        dataFabricacao,
                        quantidadeEtiquetasTexto,
                        quantidadeEtiquetas
                    )

                // Interrompe a atualização quando algum campo for inválido.
                if (!formularioValido) {
                    return
                }

                // Captura o nome do produto selecionado para apresentar na mensagem de sucesso.
                const nomeProduto =
                    dropdownProduto.options[
                        dropdownProduto.selectedIndex
                    ].text

                // Montagem do objeto JavaScript que será convertido para JSON.
                const producaoJson = {
                    id: idProducao,
                    dataFabricacao: dataFabricacao,
                    quantidadeEtiquetas: quantidadeEtiquetas
                }

                try {
                    // Envia os dados atualizados da produção para a API.
                    const respostaAtualizacao =
                        await fetch(
                            `https://localhost:7288/Producao/atualizarProducao${idUrl}`,
                            {
                                method: 'PUT',
                                headers: {
                                    'Content-Type': 'application/json',
                                    'Authorization':
                                        `Bearer ${tokenAtual}`
                                },
                                body: JSON.stringify(
                                    producaoJson
                                )
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

                    // Impede a continuação caso a API não consiga atualizar a produção.
                    if (!respostaAtualizacao.ok) {
                        window.alert(
                            'O servidor recebeu a solicitação, mas não conseguiu atualizar a produção.'
                        )

                        return
                    }

                    // A execução somente chega aqui quando a produção foi atualizada com sucesso.
                    window.alert(
                        `A produção ${nomeProduto} foi atualizada com sucesso.`
                    )

                    window.location.href =
                        'listaProducoes.html'
                } catch (erro) {
                    console.error(
                        'Erro de rede: a API pode estar desligada ou fora do ar.',
                        erro
                    )

                    window.alert(
                        'Não foi possível conectar ao sistema para atualizar a produção.'
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
            'Não foi possível conectar ao sistema para carregar a produção.'
        )
    }
}