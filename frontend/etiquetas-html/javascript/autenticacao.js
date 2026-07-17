function obterToken() {
    return sessionStorage.getItem('tokenEtiquetaExpress')
}

function encerrarSessao() {
    sessionStorage.removeItem('tokenEtiquetaExpress')

    window.location.replace('login.html')
}

function exigirAutenticacao() {
    const token = obterToken()

    if (!token) {

        encerrarSessao()

        return null
    } else {
        return token
    }

}

function tratarNaoAutorizado(resposta) {
    if (resposta.status === 401) {
        encerrarSessao()
        return true
    }

    return false
}