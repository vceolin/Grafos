using System;
using System.Collections.Generic;
using System.Text;

namespace ListaPratica_Grafos
{
    [Serializable] //permite clonar
    class Vertice
    {
        #region variaveis
        private static int proxID = 0; //Variavel estatica para atribuir IDs diferentes para cada vertice
        public int id; //id do vertice

        public List<Aresta> arestas = new List<Aresta>(); //Arestas do vértice
        #endregion

        #region construtores
        /// <summary>
        /// Construtor de um vértice vazio, atribui um id para ele
        /// </summary>
        public Vertice()
        {
            id = proxID;
            proxID++;
        }
        #endregion

        #region metodos
        /// <summary>
        /// Retorna se o vértice v1 é adjacente desse vértice
        /// </summary>
        /// <param name="v1">Vértice a ser verificado</param>
        /// <returns>true se for adjacente, false senão</returns>
        public bool isAdjacente(Vertice v1)
        {
            foreach(Aresta aresta in arestas) //percorre todas as arestas do grafo
            {
                if (aresta.getVerticeVizinho(this) == v1)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna o grau desse vértice (quantidade de vértices adjacentes a ele)
        /// </summary>
        /// <returns>Grau do vértice</returns>
        public int getGrau()
        {
            return this.arestas.Count;
        }

        /// <summary>
        /// Retorna se esse vértice possui ou não vizinhos
        /// </summary>
        /// <returns>True se o vértice for isolado, false senão</returns>
        public bool isIsolado()
        {
            return this.arestas.Count == 0 ? true : false; //Se o vértice não possuir adjacentes, é isolado.
        }

        /// <summary>
        /// Retorna se esse vértice é pendente
        /// Um vértice folha (também vértice pendente) é um vértice de grau um.
        /// </summary>
        /// <returns>True se o vértice for pendente, false senão</returns>
        public bool isPendente()
        {
            return this.arestas.Count == 1 ? true : false; //Se o vértice possuir apenas um adjacente, é pendente.
        }
        #endregion
    }
}
