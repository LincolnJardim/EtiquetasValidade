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
            for (let posição = 0; posição < tamanho; posição++) {

                console.log(produtos)
                let linha = document.createElement('tr')
                
                linha.innerHTML = `
                    <td>${produtos[posição].id}</td>
                    <td>${produtos[posição].nome}</td>
                    <td>${produtos[posição].diasValidade}</td>
                    <td>
                        <button class="btn-editar">Editar</button>
                        <button class="btn-excluir">Excluir</button>
                    </td>
                `

                tabela.appendChild(linha)
            }
        }

    } catch (erro) {
            console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
            } 

}


/*function validarConexão() {
    window.alert('JavaScript conectado.')
}
*/