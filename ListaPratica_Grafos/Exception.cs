using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// Exeção customizada caso o usuário tente fazer operações no tipo de grafo errado
/// </summary>
namespace ListaPratica_Grafos
{
    class TipoGrafoException : Exception
    {
        /// <summary>
        /// Apenas cria uma exceção
        /// </summary>
        public TipoGrafoException()
          : base()
        {
        }

        /// <summary>
        /// Cria uma exceção com mensagem de erro customizada
        /// </summary>
        /// <param name="isDirigido">Informa se o grafo é dirigido ou não</param>
        public TipoGrafoException(bool isDirigido) : base(CriarMensagem(isDirigido))
        {
        }

        /// <summary>
        /// Trata a mensagem de erro de acordo com o tipo de grafo
        /// </summary>
        /// <param name="isDirigido"></param>
        /// <returns></returns>
        private static string CriarMensagem(bool isDirigido)
        {
            string erro;
            if (isDirigido)
            {
                erro = "Você tentou executar um método para grafos dirigidos, sendo que o grafo não é dirigido";
            }
            else
            {
                erro = "Você tentou executar um método para grafos não dirigidos, sendo que o grafo é dirigido";
            }
            return erro;
        }

    }
}
