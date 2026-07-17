// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função logout().
document.addEventListener('DOMContentLoaded', function () {
    logout()
})

// Função responsável por configurar o botão que encerra a sessão do usuário.
function logout() {
    const botaoSair = document.getElementById('botao-sair')

    // Interrompe a execução caso o botão não exista na página.
    if (!botaoSair) {
        return
    }

    // Executa a função de encerramento da sessão ao clicar no botão.
    botaoSair.addEventListener('click', encerrarSessao)
}