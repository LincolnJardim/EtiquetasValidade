document.addEventListener('DOMContentLoaded', function () {
    listarProducao()
})

async function listarProducao() {
    const tabela = document.querySelector('#itabela tbody')

    tabela.innerHTML = ''

    try {
        const resposta = await fetch('https://localhost:7288/Producao/listarProducoesCadastradas', {
            method: 'GET'
        })

        if (resposta.ok) {
            let listaProducao = await resposta.json()

            for (let posicao = 0; posicao < listaProducao.length; posicao++) {

                const dataFabricacao = formatarData(listaProducao[posicao].dataFabricacao)

                const dataValidade = formatarData(listaProducao[posicao].dataValidade)

                let linha = document.createElement('tr')

                linha.innerHTML = `
                <td>${listaProducao[posicao].id}</td>
                <td>${listaProducao[posicao].produto.nome}</td>
                <td>${dataFabricacao}</td>
                <td>${dataValidade}</td>
                <td>${listaProducao[posicao].quantidadeEtiquetas}</td>
                <td>
                    <button class="btn-editar"      data-producao-id="${listaProducao[posicao].id}">Editar</button>  

                    <button class="btn-excluir" data-producao-id="${listaProducao[posicao].id}">Excluir</button>

                    <button class="btn-etiqueta" data-producao-id="${listaProducao[posicao].id}">Etiqueta</button>
                
                </td>
                `

                const botaoEditar = linha.querySelector('.btn-editar')
                const botaoExcluir = linha.querySelector('.btn-excluir')
                const botaoEtiqueta = linha.querySelector('.btn-etiqueta')

                botaoEditar.addEventListener('click', function (evento) {
                    const elementoClicado = evento.target

                    const idProducao = elementoClicado.dataset.producaId

                    window.location.href = `editarProducao.html?id=${idProducao}`
                })


                botaoExcluir.addEventListener('click', function (evento) {

                    const elementoClicado = evento.target

                    const idProducao = elementoClicado.dataset.producaoId

                    deletarProducao(idProducao)
                })

                botaoEtiqueta.addEventListener('click', function (evento) {

                    const elementoClicado = evento.target

                    const idProducao = elementoClicado.dataset.producaId

                    window.location.href = `gerarEtiqueta.html?id=${idProducao}`
                })


                tabela.appendChild(linha)
            }
        }
    } catch (erro) {
        console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
    }
}

async function deletarProducao(id) {
    let confirmarExclusao = window.confirm('Você deseja excluir essa produção? Essa ação é permanente')

    if (!confirmarExclusao) {
        window.alert("Produção não será excluido!")
        return
    }

    try {
        const resposta = await fetch(`https://localhost:7288/producao/${id}`,
            {
                method: "DELETE"
            })

        if (resposta.ok) {
            window.alert('Produção excluida com sucesso!')

            await listarProducao()
        } else {
            window.alert('Não foi possível excluir produção')
        }
    } catch (erro) {
        console.error('Erro de rede: A API pode estar desligada ou fora do ar.', erro)
    }
}

function formatarData(dataApi) {
    let data = new Date(dataApi)

    let ano = data.getFullYear()
    let mes = String(data.getMonth() + 1).padStart(2, '0')
    let dia = String(data.getDate()).padStart(2, '0')

    let dataFormatada = `${dia}/${mes}/${ano}`

    return dataFormatada
}