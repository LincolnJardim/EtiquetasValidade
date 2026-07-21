// Função responsável por validar os dados informados no formulário de produção.
function validarFormularioProducao(
    produtoIdTexto,
    produtoId,
    dataFabricacao,
    quantidadeEtiquetasTexto,
    quantidadeEtiquetas
) {
    // Verifica se algum produto foi selecionado.
    if (produtoIdTexto === '') {
        mostrarMensagemProducao(
            'Selecione um produto.'
        )

        return false
    }

    // Verifica se o identificador do produto é um número válido.
    if (
        Number.isNaN(produtoId) ||
        !Number.isInteger(produtoId) ||
        produtoId <= 0
    ) {
        mostrarMensagemProducao(
            'O produto selecionado é inválido.'
        )

        return false
    }

    // Verifica se a data de fabricação foi informada.
    if (dataFabricacao === '') {
        mostrarMensagemProducao(
            'Informe a data de fabricação.'
        )

        return false
    }

    // Impede o cadastro ou edição de uma produção com data futura.
    if (dataFabricacao > obterDataAtualFormatada()) {
        mostrarMensagemProducao(
            'A data de fabricação não pode ser futura.'
        )

        return false
    }

    // Verifica se a quantidade de etiquetas foi informada.
    if (quantidadeEtiquetasTexto === '') {
        mostrarMensagemProducao(
            'Informe a quantidade de etiquetas.'
        )

        return false
    }

    // Verifica se a quantidade informada é um número válido.
    if (Number.isNaN(quantidadeEtiquetas)) {
        mostrarMensagemProducao(
            'Informe uma quantidade válida de etiquetas.'
        )

        return false
    }

    // Impede o cadastro de quantidades decimais.
    if (!Number.isInteger(quantidadeEtiquetas)) {
        mostrarMensagemProducao(
            'A quantidade de etiquetas deve ser um número inteiro.'
        )

        return false
    }

    // Impede o cadastro de quantidades iguais ou menores que zero.
    if (quantidadeEtiquetas <= 0) {
        mostrarMensagemProducao(
            'A quantidade de etiquetas deve ser maior que zero.'
        )

        return false
    }

    return true
}


// Função responsável por retornar a data atual no formato utilizado pelo input date.
function obterDataAtualFormatada() {
    const dataAtual = new Date()

    const ano = dataAtual.getFullYear()

    const mes = String(
        dataAtual.getMonth() + 1
    ).padStart(2, '0')

    const dia = String(
        dataAtual.getDate()
    ).padStart(2, '0')

    return `${ano}-${mes}-${dia}`
}


// Função responsável por apresentar mensagens de validação ao usuário.
function mostrarMensagemProducao(mensagem) {
    window.alert(mensagem)
}