document.addEventListener('DOMContentLoaded', function() {
    listarProdutos()
})

async function listarProdutos() {
    const tabela = document.querySelector('#itabela tbody')
    
    tabela.innerHTML = ''

    try {
        const resposta = await fetch('https://localhost:7288/Produto/listarProdutosCadastrados', {
            method: 'GET'
        })

        if (resposta.ok) {
            let produtos = await resposta.json()
            let tamanho = produtos.length
            console.log(produtos)
            for (let posição = 0; posição < tamanho; posição++) {

                let linha = document.createElement('tr')
                
                linha.innerHTML = `
                    <td>${produtos[posição].id}</td>
                    <td>${produtos[posição].nome}</td>
                    <td>${produtos[posição].diasValidade}</td>
                    <td>
                        <button class="btn-editar" data-produto-id="${produtos[posição].id}">Editar</button>
                        <button class="btn-excluir" data-produto-id="${produtos[posição].id}">Excluir</button>
                    </td>
                `
                const botaoEditar = linha.querySelector('.btn-editar')
                const botaoExcluir = linha.querySelector('.btn-excluir')

                botaoEditar.addEventListener('click', function(evento) {
                    const elementoClicado = evento.target

                    const idProduto = elementoClicado.dataset.produtoId

                    window.location.href = `editarProdutos.html?id=${idProduto}`
                })

                
                botaoExcluir.addEventListener('click', function(evento) {
                    
                    const elementoClicado = evento.target

                    const idProduto = elementoClicado.dataset.produtoId

                    deletarProduto(idProduto)
                })

                tabela.appendChild(linha)
            }
        }
        
    } catch (erro) {
            console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
            } 

}

async function deletarProduto(id) {
    let confirmarExclusao = window.confirm('Você deseja excluir esse produto? Essa ação é permanente')

    if (!confirmarExclusao) {
        window.alert("Produto não será excluido!")
        return 
    }

    try {
    const resposta = await fetch(`https://localhost:7288/Produto/${id}`, {
        method: 'DELETE'
    })

    if (resposta.ok) {
        window.alert('Produto excluido com sucesso!')

        await listarProdutos()
    } else {
        window.alert('Não foi possível excluir produto')
    }
    } catch (erro) {
        console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
    }
}


/*function validarConexão() {
    window.alert('JavaScript conectado.')
}
*/