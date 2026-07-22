// Manipulação do DOM para esperar a página HTML carregar por completo antes de iniciar a geração das etiquetas.
document.addEventListener('DOMContentLoaded', async function () {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    // Aguarda a geração das etiquetas e armazena o resultado da operação.
    const etiquetasCarregadas = await carregarEtiquetas(token)

    // Impede que o botão de impressão seja configurado quando as etiquetas não forem carregadas.
    if (!etiquetasCarregadas) {
        return
    }

    configurarBotaoImprimir()
})


// Função responsável por buscar as etiquetas na API e adicioná-las ao HTML.
async function carregarEtiquetas(token) {
    const parametros =
        new URLSearchParams(window.location.search)

    const idUrl = parametros.get('id')

    const containerEtiquetas =
        document.getElementById('etiquetas-container')

    // Impede a geração quando o identificador da produção não foi informado ou é inválido.
    if (
        !idUrl ||
        Number.isNaN(Number(idUrl)) ||
        Number(idUrl) <= 0
    ) {
        window.alert(
            'Produção inválida. Voltando para a lista.'
        )

        window.location.href = 'listaProducoes.html'

        return false
    }

    // Limpa o container antes de adicionar as etiquetas recebidas da API.
    containerEtiquetas.innerHTML = ''

    try {
        // Busca na API as etiquetas relacionadas à produção informada pela URL.
        const resposta = await fetch(
            `https://localhost:7288/Producao/gerarEtiqueta${idUrl}`,
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

        // Impede a continuação caso a API não consiga gerar as etiquetas.
        if (!resposta.ok) {
            await mostrarErroDaApi(
                resposta,
                'Não foi possível gerar as etiquetas.'
            )

            return false
        }

        const listaEtiquetas = await resposta.json()

        // Impede a impressão quando nenhuma etiqueta for retornada pela API.
        if (listaEtiquetas.length === 0) {
            window.alert(
                'Nenhuma etiqueta foi encontrada para esta produção.'
            )

            window.location.href = 'listaProducoes.html'

            return false
        }

        // Percorre a lista recebida da API e cria cada etiqueta no HTML.
        for (const etiqueta of listaEtiquetas) {
            const dataFabricacao =
                formatarData(etiqueta.dataProducao)

            const dataValidade =
                formatarData(etiqueta.dataValidade)

            const elementoEtiqueta =
                document.createElement('div')

            elementoEtiqueta.classList.add('etiqueta')

            elementoEtiqueta.innerHTML = `
                <h2>Etiqueta Express</h2>

                <p>Produto</p>
                <strong>${etiqueta.nomeProduto}</strong>

                <hr>

                <p>Fabricação</p>
                <strong>${dataFabricacao}</strong>

                <p>Validade</p>
                <strong>${dataValidade}</strong>
            `

            containerEtiquetas.appendChild(elementoEtiqueta)
        }

        // Informa que as etiquetas foram carregadas corretamente.
        return true
    } catch (erro) {
        console.error(
            'Erro de rede: a API pode estar desligada ou fora do ar.',
            erro
        )

        window.alert(
            'Não foi possível conectar ao sistema para gerar as etiquetas.'
        )

        return false
    }
}


// Função responsável por configurar o evento de impressão das etiquetas.
function configurarBotaoImprimir() {
    const botaoImprimir =
        document.getElementById('imprimir')

    botaoImprimir.addEventListener('click', function () {
        window.print()
    })
}


// Função responsável por converter a data recebida da API para o formato brasileiro.
function formatarData(dataApi) {
    /*
        Separa somente a parte da data, evitando alterações causadas
        pelo fuso horário ao utilizar new Date().
    */
    const dataSemHorario = dataApi.split('T')[0]

    const partesData = dataSemHorario.split('-')

    const ano = partesData[0]
    const mes = partesData[1]
    const dia = partesData[2]

    return `${dia}/${mes}/${ano}`
}