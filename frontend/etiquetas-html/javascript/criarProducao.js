// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função cadastrarProduto()
document.addEventListener('DOMContentLoaded', function () {
    carregarProduto()
    preencherDataAtual()
    cadastrarProducao()
})

async function carregarProduto() {
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

function preencherDataAtual() {
    let inputData = document.getElementById('idatafabricacao')
    let data = new Date()

    let ano = data.getFullYear()
    let mes = String(data.getMonth() + 1).padStart(2, '0')
    let dia = String(data.getDate()).padStart(2, '0')

    let dataFormatada = `${ano}-${mes}-${dia}`

    inputData.value = dataFormatada

}

function cadastrarProducao() {

    const formulario = document.getElementById('icadastro')

    formulario.addEventListener('submit', async function(evento) {
        evento.preventDefault()

        let dropdownProduto = document.getElementById('idrop-produto')
        let dataFabricacao = document.getElementById('idatafabricacao').value
        let quantidadeEtiquetas = document.getElementById('ietiquetas').value

        let produtoId = dropdownProduto.value
        let nomeProduto = dropdownProduto.options[dropdownProduto.selectedIndex].text

        

        let producaoJson = {
            produtoId: Number(produtoId),
            dataFabricacao: dataFabricacao,
            quantidadeEtiquetas: Number(quantidadeEtiquetas)
        }

        try {
            const resposta = await fetch('https://localhost:7288/Producao', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(producaoJson)
            })

            if (resposta.ok) {
                window.alert(`A produção ${nomeProduto} foi cadastrada com sucesso.`)

                formulario.reset()
            } else {
                window.alert('O servidor C# recebeu, mas retornou um erro.')
            }
        } catch (erro) {
            console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
        }

        preencherDataAtual()
    }) 
}