using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace Etiquetas.Api.Exceptions
{
    /*
        Classe responsável por capturar exceções inesperadas
        que não foram tratadas pelos Controllers ou Services.
    */
    public sealed class GlobalExceptionHandler
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler>
            _logger;


        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger
        )
        {
            _logger = logger;
        }


        /*
            Método executado quando uma exceção não tratada
            percorre o pipeline da aplicação.
        */
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            /*
                Registra os detalhes técnicos nos logs da API,
                mas não os envia ao usuário.
            */
            _logger.LogError(
                exception,
                "Erro inesperado durante a requisição. TraceId: {TraceId}",
                httpContext.TraceIdentifier
            );

            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            /*
                Retorna uma mensagem segura e genérica para
                o frontend, sem expor banco, classes ou stack trace.
            */
            await httpContext.Response.WriteAsJsonAsync(
                new
                {
                    mensagem =
                        "Ocorreu um erro inesperado no sistema. Tente novamente mais tarde.",

                    traceId =
                        httpContext.TraceIdentifier
                },
                cancellationToken: cancellationToken
            );

            /*
                true informa que a exceção já foi tratada
                e não deve continuar pelo pipeline.
            */
            return true;
        }
    }
}