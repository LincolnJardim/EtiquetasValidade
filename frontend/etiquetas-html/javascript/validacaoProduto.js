// Função responsável por validar os dados informados no formulário de produto.
function validarFormularioProduto(
    nomeProduto,
    diasValidadeTexto,
    diasValidade
) {
    if (nomeProduto === '') {
        mostrarMensagem('Informe o nome do produto.')
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
        mostrarMensagem('Informe a validade do produto.')
        return false
    }

    if (Number.isNaN(diasValidade)) {
        mostrarMensagem('Informe uma validade válida.')
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