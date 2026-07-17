// Manipulação do DOM para esperar a página HTML carregar por completo antes de verificar a autenticação e chamar a função cadastrarProduto().
document.addEventListener('DOMContentLoaded', function () {
    const token = exigirAutenticacao()

    // Interrompe a execução caso não exista uma sessão autenticada.
    if (!token) {
        return
    }

    cadastrarProduto()
})

// Função responsável por capturar dados do formulário e enviar para a API criar um produto na tabela Produto, sendo chamada pelo formulário "Cadastrar produto".
function cadastrarProduto() {
    // Captura do formulário.
    const formulario = document.getElementById('icadastro')

    // Criação do evento submit e função assíncrona com alteração no evento padrão de recarregar a página.
    formulario.addEventListener('submit', async function (evento) {
        evento.preventDefault()

        // Verifica novamente a autenticação no momento do envio do formulário.
        const token = exigirAutenticacao()

        // Interrompe o cadastro caso a sessão não exista mais.
        if (!token) {
            return
        }

        // Bloco para captura dos valores dos inputs.
        const nomeProduto = document.getElementById('iproduto').value.trim()

        const inputDiasValidade =
            document.getElementById('ivalidade')

        const diasValidadeTexto =
            inputDiasValidade.value.trim()

        const diasValidade =
            Number(diasValidadeTexto)

        // Bloco para validações do formulário
        const formularioValido = validarFormulario(
            nomeProduto,
            diasValidadeTexto,
            diasValidade
        )

        if (!formularioValido) {
            return
        }

        // console.log(`O produto ${nomeProduto} tem a validade de ${diasValidade}`)

        // Montagem do objeto JavaScript que será convertido para JSON.
        const produtoJson = {
            nome: nomeProduto,
            diasValidade: diasValidade
        }

        // Bloco para conexão com a rota da API e conversão do objeto JavaScript para JSON.
        try {
            // Await pausa a função até a API C# responder.
            const resposta = await fetch(
                'https://localhost:7288/Produto',
                {
                    method: 'POST',

                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },

                    // Conversão do objeto JavaScript para JSON.
                    body: JSON.stringify(produtoJson)
                }
            )

            // Verifica se a API retornou 401 por token ausente, inválido ou expirado.
            if (tratarNaoAutorizado(resposta)) {
                return
            }

            // Bloco para verificar se a API retornou algum erro após processar a requisição.
            if (!resposta.ok) {
                window.alert(
                    'O servidor recebeu a solicitação, mas não conseguiu cadastrar o produto.'
                )

                return
            }

            // A execução somente chega aqui quando o produto foi cadastrado com sucesso.
            window.alert(
                `O produto ${produtoJson.nome} foi cadastrado com sucesso.`
            )

            // Limpa o formulário somente depois que o cadastro for concluído com sucesso.
            formulario.reset()
        } catch (erro) {
            // Esse bloco é executado quando ocorre uma falha de rede ou quando a API está indisponível.
            console.error(
                'Erro de rede: a API pode estar desligada ou fora do ar.',
                erro
            )
        }
    })
}

// Função responsável por validar os dados informados no formulário.
function validarFormulario(
    nomeProduto,
    diasValidadeTexto,
    diasValidade
) {
    if (nomeProduto === '') {
        mostrarMensagem(
            'Informe o nome do produto.'
        )

        return false
    }

    if (nomeProduto.length < 2) {
        mostrarMensagem(
            'O nome do produto deve possuir pelo menos 2 caracteres.'
        )

        return false
    }

    if (nomeProduto.length > 100) {
        mostrarMensagem(
            'O nome do produto deve possuir no máximo 100 caracteres.'
        )

        return false
    }

    if (diasValidadeTexto === '') {
        mostrarMensagem(
            'Informe a validade do produto.'
        )

        return false
    }

    if (Number.isNaN(diasValidade)) {
        mostrarMensagem(
            'Informe uma validade válida.'
        )

        return false
    }

    if (!Number.isInteger(diasValidade)) {
        mostrarMensagem(
            'A validade deve ser informada em dias inteiros.'
        )

        return false
    }

    if (diasValidade <= 0) {
        mostrarMensagem(
            'A validade deve ser maior que zero.'
        )

        return false
    }

    return true
}

// Função responsável por apresentar uma mensagem de validação ao usuário.
function mostrarMensagem(mensagem) {
    window.alert(mensagem)
}

/*
function validarConexao() {
    window.alert('JavaScript conectado.')
}
*/