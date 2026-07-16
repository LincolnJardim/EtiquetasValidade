// Manipulação do DOM para esperar a página HTML carregar por completo antes de chamar a função login()
document.addEventListener('DOMContentLoaded', function() {
    login()
})

function login() {
    // Captura do formulario.
    const formulario = document.getElementById('formulario-login')

    formulario.addEventListener('submit' , async function (evento) {
        evento.preventDefault()

        // Bloco para captura dos valores dos inputs.
        let email = document.getElementById('iemail').value
        let senha = document.getElementById('isenha').value
        let mensagemFalha = document.getElementById('mensagem-login')

        mensagemFalha.style.display = 'none'
        mensagemFalha.textContent = ''

        //console.log(`O login ${email} tem a senha${senha}`)

        // Montagem do objeto Json.
        let loginJson = {
            email: email,
            senha: senha
        }

        // Bloco para conexão com a rota da API e conversão do objeto JS para Json com await.
        try {
            const resposta = await fetch('https://localhost:7288/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(loginJson) //Conversão do objeto para JSON
            })

            if (resposta.ok) {
                let loginSucesso = await resposta.json()

                sessionStorage.setItem('tokenEtiquetaExpress', loginSucesso.token)

                window.location.href = 'index.html'

            } else {
                let loginFalho = await resposta.json()

                mensagemFalha.textContent = loginFalho.mensagem ?? 'Não foi possível realizar o login'

                mensagemFalha.style.display = 'block'

            }
        }
        catch (erro) {
            console.error("Não foi possível conectar ao sistema" , erro)

            mensagemFalha.textContent =
                'Não foi possível conectar ao sistema.'

            mensagemFalha.style.display = 'block'
        }
    })
}