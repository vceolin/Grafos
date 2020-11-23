using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ListaPratica_Grafos
{
    [Serializable] //permite clonar
    class Aresta
    {
        #region variaveis
        private static int proxID = 0; //Variavel estatica para atribuir IDs diferentes para cada aresta
        public int id; //id da aresta

        public int peso; //peso da aresta. inicia como 1 caso não haja peso
        public Vertice origem;
        public Vertice destino;
        #endregion
        #region construtores
        /// <summary>
        /// Construtor informando todos os campos
        /// </summary>
        /// <param name="origem">Vertice de origem</param>
        /// <param name="destino">Vertice de destino</param>
        /// <param name="peso">Peso da aresta</param>
        public Aresta(Vertice origem, Vertice destino, int peso)
        {
            this.peso = peso;
            this.origem = origem;
            this.destino = destino;

            //agora colocamos essa aresta nos vertices
            origem.arestas.Add(this);
            destino.arestas.Add(this);

            id = proxID;
            proxID++;
        }
        /// <summary>
        /// Construtor caso não deseje informar o peso
        /// Por padrão, o peso é definido como 1.
        /// </summary>
        /// <param name="origem">Vertice de origem</param>
        /// <param name="destino">Vertice de destino</param>
        public Aresta(Vertice origem, Vertice destino)
        {
            this.peso = 1;
            this.origem = origem;
            this.destino = destino;

            //agora colocamos essa aresta nos vertices
            origem.arestas.Add(this);
            destino.arestas.Add(this);

            id = proxID;
            proxID++;
        }
        #endregion

        /// <summary>
        /// Solução para retornar o outro vértice sem ser o que está agora em grafos não conexos
        /// </summary>
        /// <param name="vertice">vértice que você está</param>
        /// <returns>vértice de destino</returns>
        public Vertice getVerticeVizinho(Vertice vertice)
        {
            if (origem == vertice)
                return destino;
            else
                return origem;
        }
    }
}
