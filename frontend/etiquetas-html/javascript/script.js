// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função cadastrarProduto()
document.addEventListener('DOMContentLoaded', function() {
    cadastrarProduto()
})

// Função responsável em capturar dados do formulario e enviar para API criar um produto na tabela produto, sendo chamada pelo form "cadastrar produto".
function cadastrarProduto() {
    // Captura do formulario.
    const formulario = document.getElementById('icadastro')
    // Criação do evento submit e função assíncrona com alteração no evento padrão de recarregar a página.
    formulario.addEventListener('submit', async function(evento) {
        evento.preventDefault()

        // Bloco para captura dos valores dos inputs.
        let nomeProduto = document.getElementById('iproduto').value
        let diasValidade = document.getElementById('ivalidade').value

        //console.log(`O produto ${nomeProduto} tem a validade de ${diasValidade}`)

        // Montagem do objeto Json.
        let produtoJson = {
            Nome: nomeProduto,
            DiasValidade: Number(diasValidade)
        }

        // Bloco para conexão com a rota da API e conversão do objeto JS para Json com await.
        try {
            // Await pausa a função até a API C# responder.
            const resposta = await fetch('https://localhost:7288/Produto', { // Conexão com a rota, ainda irei colocar o endereço correto.
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(produtoJson) //Conversão do objeto para JSON
            })

            // Bloco If para verificar se API retornou sucesso após processar requisição
            if (resposta.ok) {
            window.alert(`O produto ${produtoJson.Nome} foi cadastrado com sucesso.`)
            } else {
            window.alert('O servidor C# recebeu, mas retornou um erro.')
            }
            } catch (erro) {
            console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
            } 
            
            formulario.reset()
        }
    )
}

/*function validarConexão() {
    window.alert('JavaScript conectado.')
}
*/