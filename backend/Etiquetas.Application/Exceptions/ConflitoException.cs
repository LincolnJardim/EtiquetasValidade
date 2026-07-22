using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.Exceptions
{
    /*
        Exceção utilizada quando uma operação não pode
        ser concluída devido ao estado atual dos dados.
    */
    public sealed class ConflitoException : Exception
    {
        public ConflitoException(string mensagem)
            : base(mensagem)
        {
        }
    }
}