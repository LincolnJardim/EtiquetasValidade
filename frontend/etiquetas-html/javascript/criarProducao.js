// Manipulação do DOM para esperar a página HTML carregar por completo antes de iniciar o cadastro de uma produção.
document.addEventListener('DOMContentLoaded', async function () {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    // Aguarda o carregamento dos produtos e armazena o resultado da operação.
    const produtosCarregados = await carregarProdutos()

    // Impede que o formulário continue caso os produtos não tenham sido carregados.
    if (!produtosCarregados) {
        return
    }

    preencherDataAtual()
    cadastrarProducao()
})


// Função responsável por buscar os produtos cadastrados na API e preencher o campo de seleção.
async function carregarProdutos() {
    const dropdownProdutos =
        document.getElementById('idrop-produto')

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
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

        // Impede o cadastro de uma produção quando não existem produtos cadastrados.
        if (listaProdutos.length === 0) {
            window.alert(
                'Nenhum produto cadastrado. Cadastre um produto antes de criar uma produção.'
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


// Função responsável por preencher automaticamente o campo com a data atual.
function preencherDataAtual() {
    const inputData =
        document.getElementById('idatafabricacao')

    const dataAtualFormatada =
        obterDataAtualFormatada()

    // Preenche o campo com a data atual.
    inputData.value = dataAtualFormatada

    // Impede a seleção de uma data posterior à data atual.
    inputData.max = dataAtualFormatada
}


// Função responsável por capturar os dados do formulário e enviar uma nova produção para a API.
function cadastrarProducao() {
    const formulario =
        document.getElementById('icadastro')

    // Criação do evento submit e função assíncrona com alteração no comportamento padrão de recarregar a página.
    formulario.addEventListener(
        'submit',
        async function (evento) {
            evento.preventDefault()

            // Verifica novamente a autenticação no momento do envio do formulário.
            const token = exigirAutenticacao()

            // Interrompe a execução caso a sessão não exista mais.
            if (!token) {
                return
            }

            // Captura dos elementos do formulário.
            const dropdownProduto =
                document.getElementById('idrop-produto')

            const inputDataFabricacao =
                document.getElementById('idatafabricacao')

            const inputQuantidadeEtiquetas =
                document.getElementById('ietiquetas')

            // Captura dos valores informados pelo usuário.
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

            // Interrompe o cadastro quando algum campo for inválido.
            if (!formularioValido) {
                return
            }

            // Captura o nome do produto para apresentar na mensagem de sucesso.
            const nomeProduto =
                dropdownProduto.options[
                    dropdownProduto.selectedIndex
                ].text

            // Montagem do objeto JavaScript que será convertido para JSON.
            const producaoJson = {
                produtoId: produtoId,
                dataFabricacao: dataFabricacao,
                quantidadeEtiquetas: quantidadeEtiquetas
            }

            try {
                // Envia os dados da produção para a API.
                const resposta = await fetch(
                    'https://localhost:7288/Producao',
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': `Bearer ${token}`
                        },
                        body: JSON.stringify(producaoJson)
                    }
                )

                // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
                if (tratarNaoAutorizado(resposta)) {
                    return
                }

                // Interrompe a execução caso a API não consiga cadastrar a produção.
                if (!resposta.ok) {
                    await mostrarErroDaApi(
                        resposta,
                        'Não foi possível cadastrar a produção.'
                    )

                    return
                }

                // A execução somente chega aqui quando a produção foi cadastrada com sucesso.
                window.alert(
                    `A produção ${nomeProduto} foi cadastrada com sucesso.`
                )

                // Limpa o formulário somente depois que o cadastro for concluído.
                formulario.reset()

                // Como o reset limpa a data, o campo é novamente preenchido com a data atual.
                preencherDataAtual()
            } catch (erro) {
                console.error(
                    'Erro de rede: a API pode estar desligada ou fora do ar.',
                    erro
                )

                window.alert(
                    'Não foi possível conectar ao sistema para cadastrar a produção.'
                )
            }
        }
    )
}