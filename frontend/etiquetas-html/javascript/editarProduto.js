// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função cadastrarProduto()
document.addEventListener('DOMContentLoaded', function () {
    editarProduto()
})

async function editarProduto() {
    const parametros = new URLSearchParams(window.location.search)

    const idUrl = parametros.get('id')

    if (idUrl === null) {
        window.alert('Produto inválido. Voltando para a lista')
        window.location.href = 'listarProduto.html'
    } else {

        try {
            const resposta = await fetch(`https://localhost:7288/Produto/obterProdutoPorId${idUrl}`, {
                method: 'GET'
            })

            if (resposta.ok) {
                let produtoBanco = await resposta.json()

                let nomeProduto = document.getElementById('iproduto')
                let validadeProduto = document.getElementById('ivalidade')

                nomeProduto.value = `${produtoBanco.nome}`
                validadeProduto.value = `${produtoBanco.diasValidade}`
            }

            const formulario = document.getElementById('icadastro')

            formulario.addEventListener('submit', async function (evento) {
                evento.preventDefault()

                // Bloco para captura dos valores dos inputs.
                let nomeProduto = document.getElementById('iproduto').value
                let diasValidade = document.getElementById('ivalidade').value

                //console.log(`O produto ${nomeProduto} tem a validade de ${diasValidade}`)

                // Montagem do objeto Json.
                let produtoJson = {
                    id: Number(idUrl),
                    Nome: nomeProduto,
                    DiasValidade: Number(diasValidade)
                }

                try {
                    const resposta = await fetch(`https://localhost:7288/Produto/atualizarProduto${idUrl}`, {
                        method: "PUT",
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(produtoJson)
                    })

                    if (resposta.ok) {
                        window.alert(`O produto ${produtoJson.Nome} foi atualizado com sucesso.`)

                        window.location.href = 'lista_produtos.html'
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
