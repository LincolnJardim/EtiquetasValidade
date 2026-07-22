// Manipulação do DOM para esperar a página HTML carregar por completo antes de iniciar a listagem das produções.
document.addEventListener('DOMContentLoaded', function () {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    listarProducao()
})


// Função responsável por buscar as produções cadastradas na API e preencher a tabela.
async function listarProducao() {
    const tabela = document.querySelector('#itabela tbody')

    // Limpa a tabela antes de adicionar os registros retornados pela API.
    tabela.innerHTML = ''

    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    try {
        const resposta = await fetch(
            'https://localhost:7288/Producao/listarProducoesCadastradas',
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

        // Impede a continuação caso a API não consiga carregar as produções.
        if (!resposta.ok) {
            await mostrarErroDaApi(
                resposta,
                'Não foi possível carregar a produção.'
            )

            window.location.href = 'listaProducoes.html'
            return
        }

        const listaProducao = await resposta.json()

        // Exibe uma mensagem dentro da tabela quando nenhuma produção estiver cadastrada.
        if (listaProducao.length === 0) {
            const linha = document.createElement('tr')

            linha.innerHTML = `
                <td colspan="6">
                    Nenhuma produção cadastrada.
                </td>
            `

            tabela.appendChild(linha)

            return
        }

        // Percorre a lista retornada pela API e cria uma linha para cada produção.
        for (const producao of listaProducao) {
            const dataFabricacao =
                formatarData(producao.dataFabricacao)

            const dataValidade =
                formatarData(producao.dataValidade)

            const linha = document.createElement('tr')

            linha.innerHTML = `
                <td>${producao.id}</td>

                <td>${producao.produto.nome}</td>

                <td>${dataFabricacao}</td>

                <td>${dataValidade}</td>

                <td>${producao.quantidadeEtiquetas}</td>

                <td>
                    <button
                        class="btn-editar"
                        data-producao-id="${producao.id}"
                    >
                        Editar
                    </button>

                    <button
                        class="btn-excluir"
                        data-producao-id="${producao.id}"
                    >
                        Excluir
                    </button>

                    <button
                        class="btn-etiqueta"
                        data-producao-id="${producao.id}"
                    >
                        Gerar Etiqueta
                    </button>
                </td>
            `

            const botaoEditar =
                linha.querySelector('.btn-editar')

            const botaoExcluir =
                linha.querySelector('.btn-excluir')

            const botaoEtiqueta =
                linha.querySelector('.btn-etiqueta')

            // Evento responsável por redirecionar o usuário para a página de edição.
            botaoEditar.addEventListener(
                'click',
                function (evento) {
                    /*
                        currentTarget representa o botão em que
                        o evento foi registrado.
                    */
                    const elementoClicado = evento.currentTarget

                    const idProducao =
                        elementoClicado.dataset.producaoId

                    window.location.href =
                        `editarProducao.html?id=${idProducao}`
                }
            )

            // Evento responsável por solicitar a exclusão da produção.
            botaoExcluir.addEventListener(
                'click',
                function (evento) {
                    const elementoClicado = evento.currentTarget

                    const idProducao =
                        elementoClicado.dataset.producaoId

                    deletarProducao(idProducao)
                }
            )

            // Evento responsável por redirecionar para a geração das etiquetas.
            botaoEtiqueta.addEventListener(
                'click',
                function (evento) {
                    const elementoClicado = evento.currentTarget

                    const idProducao =
                        elementoClicado.dataset.producaoId

                    window.location.href =
                        `gerarEtiqueta.html?id=${idProducao}`
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
            'Não foi possível conectar ao sistema para carregar as produções.'
        )
    }
}


// Função responsável por excluir uma produção pelo identificador recebido.
async function deletarProducao(id) {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    const confirmarExclusao = window.confirm(
        'Você deseja excluir essa produção? Essa ação é permanente.'
    )

    // Interrompe a exclusão caso o usuário não confirme a operação.
    if (!confirmarExclusao) {
        window.alert('A produção não será excluída.')

        return
    }

    try {
        const resposta = await fetch(
            `https://localhost:7288/Producao/${id}`,
            {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            }
        )

        // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
        if (tratarNaoAutorizado(resposta)) {
            return
        }

        // Impede a continuação caso a API não consiga excluir a produção.
        if (!resposta.ok) {
            await mostrarErroDaApi(
                resposta,
                'Não foi possível excluir a produção.'
            )

            return
        }

        // A execução somente chega aqui quando a produção foi excluída com sucesso.
        window.alert(
            'Produção excluída com sucesso!'
        )

        // Atualiza a tabela sem precisar recarregar a página inteira.
        await listarProducao()
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para excluir a produção.'
        )
    }
}


// Função responsável por converter a data recebida da API para o formato brasileiro.
function formatarData(dataApi) {
    const data = new Date(dataApi)

    const ano = data.getFullYear()
    const mes = String(data.getMonth() + 1).padStart(2, '0')
    const dia = String(data.getDate()).padStart(2, '0')

    const dataFormatada = `${dia}/${mes}/${ano}`

    return dataFormatada
}