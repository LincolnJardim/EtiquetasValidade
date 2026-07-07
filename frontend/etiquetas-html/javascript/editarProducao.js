// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função cadastrarProduto()
document.addEventListener('DOMContentLoaded', async function () {
    await carregarProdutos()
    editarProducao()
})

async function carregarProdutos() {
    const dropdownProdutos = document.getElementById('idrop-produto')

    try {
        const resposta = await fetch('https://localhost:7288/Produto/listarProdutosCadastrados',
            {
                method: 'GET'
            }
        )

        if (resposta.ok) {
            let listaProdutos = await resposta.json()



            for (let produto of listaProdutos) {

                let item = document.createElement('option')
                item.text = `${produto.nome} (${produto.diasValidade} dias)`
                item.value = produto.id


                dropdownProdutos.appendChild(item)

            }
        }
    } catch (erro) {
        console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
    }
}

async function editarProducao() {
    const parametros = new URLSearchParams(window.location.search)

    const idUrl = parametros.get('id')

    if (idUrl === null) {
        window.alert('Producao inválida. Voltando para a lista')
        window.location.href = 'listaProducoes.html'
    } else {

        try {
            const resposta = await fetch(`https://localhost:7288/Producao/obterProducaoPorId${idUrl}`, {
                method: 'GET'
            }
            )

            if (resposta.ok) {
                let producaoBanco = await resposta.json()

                const dataFabricacaoFormatada = producaoBanco.dataFabricacao.split('T')[0]

                console.log(producaoBanco)

                let nomeProduto = document.getElementById('idrop-produto')
                let dataFabricacao = document.getElementById('idatafabricacao')
                let etiquetas = document.getElementById('ietiquetas')

                nomeProduto.value = producaoBanco.produto.id
                nomeProduto.disabled = true
                dataFabricacao.value = `${dataFabricacaoFormatada}`
                etiquetas.value = `${producaoBanco.quantidadeEtiquetas}`
            }

            const formulario = document.getElementById('icadastro')

            formulario.addEventListener('submit', async function (evento) {

                evento.preventDefault()

                let nomeProduto = document.getElementById('idrop-produto').text
                let dataFabricacao = document.getElementById('idatafabricacao').value
                let etiquetas = document.getElementById('ietiquetas').value

                // Montagem do objeto Json.
                let producaoJson = {
                    id: Number(idUrl),
                    dataFabricacao: dataFabricacao,
                    quantidadeEtiquetas: Number(etiquetas)
                }

                try {
                    const resposta = await fetch(`https://localhost:7288/Producao/atualizarProducao${idUrl}`, {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(producaoJson)
                    })

                    if (resposta.ok) {
                        window.alert(`A produção ${nomeProduto} foi atualizada com sucesso.`)

                        window.location.href = 'listaProducoes.html'
                    } else {
                        window.alert('O servidor C# recebeu, mas retornou um erro.')
                    }

                } catch (erro) {
                    console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
                }

            })

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