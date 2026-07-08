// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função cadastrarProduto()
document.addEventListener('DOMContentLoaded', async function () {
    await carregarEtiquetas()
})

async function carregarEtiquetas() {
    const parametros = new URLSearchParams(window.location.search)
    const idUrl = parametros.get('id')

    const containerEtiquetas = document.getElementById('etiquetas-container')

    if (idUrl === null) {
        window.alert('Producao inválida. Voltando para a lista')
        window.location.href = 'listaProducoes.html'
    } else {

        try {
            const resposta = await fetch(`https://localhost:7288/Producao/gerarEtiqueta${idUrl}`, {
                method: 'GET'
            })

            if (resposta.ok) {
                let listaEtiquetas = await resposta.json()

                console.log(listaEtiquetas)

                for (let etiqueta of listaEtiquetas) {

                    const dataFabricacao = formatarData(etiqueta.dataProducao)

                    const dataValidade = formatarData(etiqueta.dataValidade)

                    let elementoEtiqueta = document.createElement('div')

                    elementoEtiqueta.innerHTML = `
                        <p>Produto: ${etiqueta.nomeProduto}</p>
                        <p>Fabricação: ${dataFabricacao}</p>
                        <p>Validade: ${dataValidade}</p>
                    `

                    containerEtiquetas.appendChild(elementoEtiqueta)
                }
            }

        } catch (erro) {
            console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
        }
    }
}

function formatarData(dataApi) {
    let data = new Date(dataApi)

    let ano = data.getFullYear()
    let mes = String(data.getMonth() + 1).padStart(2, '0')
    let dia = String(data.getDate()).padStart(2, '0')

    let dataFormatada = `${dia}/${mes}/${ano}`

    return dataFormatada
}