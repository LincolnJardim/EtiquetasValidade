// Função responsável por extrair e apresentar a mensagem de erro retornada pela API.
async function mostrarErroDaApi(
    resposta,
    mensagemPadrao
) {
    let mensagemErro = mensagemPadrao

    try {
        const tipoConteudo =
            resposta.headers.get('content-type') || ''

        // Verifica se a resposta da API contém um objeto JSON.
        if (tipoConteudo.includes('application/json')) {
            const dadosErro = await resposta.json()

            /*
                Trata mensagens personalizadas retornadas pelos Controllers.

                Exemplo:
                {
                    "mensagem": "Produto não encontrado."
                }
            */
            if (
                typeof dadosErro.mensagem === 'string' &&
                dadosErro.mensagem.trim() !== ''
            ) {
                mensagemErro = dadosErro.mensagem
            }

            /*
                Trata erros automáticos gerados pelas Data Annotations.

                Exemplo:
                {
                    "errors": {
                        "Nome": [
                            "O nome do produto deve possuir entre 2 e 100 caracteres."
                        ]
                    }
                }
            */
            else if (
                dadosErro.errors &&
                typeof dadosErro.errors === 'object'
            ) {
                const mensagensValidacao =
                    Object.values(dadosErro.errors)
                        .flat()
                        .filter(function (mensagem) {
                            return (
                                typeof mensagem === 'string' &&
                                mensagem.trim() !== ''
                            )
                        })

                if (mensagensValidacao.length > 0) {
                    mensagemErro =
                        mensagensValidacao.join('\n')
                }
            }

            /*
                Utiliza o título da resposta somente quando não foi
                encontrada uma mensagem mais específica.
            */
            else if (
                typeof dadosErro.title === 'string' &&
                dadosErro.title.trim() !== ''
            ) {
                mensagemErro = dadosErro.title
            }
        } else {
            // Tenta utilizar uma mensagem textual retornada pela API.
            const textoErro = await resposta.text()

            if (textoErro.trim() !== '') {
                mensagemErro = textoErro
            }
        }
    } catch (erro) {
        /*
            Caso a resposta não possa ser interpretada,
            mantém a mensagem padrão informada pela página.
        */
        console.error(
            'Não foi possível interpretar a mensagem retornada pela API.',
            erro
        )
    }

    window.alert(mensagemErro)
}