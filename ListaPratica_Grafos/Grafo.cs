using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.ComponentModel;

namespace ListaPratica_Grafos
{
    [Serializable] //permite clonar
    class Grafo
    {
        #region variaveis
        public List<Vertice> vertices = new List<Vertice>(); //Contém todos os vértices do grafo
        private bool isDirigido;
        public bool IsDirigido { get; } //Diz se o grafo é ou não dirigido
        #endregion

        #region construtores
        /// <summary>
        /// Construtor da classe Grafo vazio
        /// </summary>
        public Grafo()
        {
        }
        /// <summary>
        /// Construtor da classe Grafo. Cria o grafo informando se ele é dirigido ou não
        /// </summary>
        /// <param name="isDirigido">True se o grafo for dirigido, false senão</param>
        public Grafo(bool isDirigido)
        {
            this.IsDirigido = isDirigido;
        }

        /// <summary>
        /// Construtor da classe Grafo. Cria o grafo informando se ele é dirigido ou não e o tamanho 
        /// </summary>
        /// <param name="isDirigido">True se o grafo for dirigido, false senão</param>
        /// <param name="tamanho">Inicializa com o tamanho do grafo</param>
        public Grafo(bool isDirigido, int tamanho)
        {
            this.IsDirigido = isDirigido;
            for (int i = 0; i < tamanho; i++)
                vertices.Add(new Vertice());
        }
        #endregion

        #region clonar
        public Grafo clone()
        {
            return ObjectCopier.CloneJson<Grafo>(this);
        }
        #endregion

        #region metodos

        #region grafos nao dirigidos
        /// <summary>
        /// Checa se o grafo é regular
        /// grafo regular é um grafo onde cada vértice tem o mesmo número de adjacências
        /// </summary>
        /// <returns>true se é regular, false senão</returns>
        public bool isRegular()
        {
            int grau = vertices.First().getGrau(); //pega o grau do primeiro vértice para testes
            foreach (Vertice vertice in vertices)
            {
                if (vertice.getGrau() != grau) //se o grau de algum vertice for diferente, retorna falso
                {
                    return false;
                }
            }
            return true; //senão, retorna true
        }

        /// <summary>
        /// Retorna se o grafo é nulo
        /// O grafo nulo ou o grafo vazio é o grafo sem arestas.
        /// </summary>
        /// <returns>true se for nulo, falso senão</returns>
        public bool isNulo()
        {
            foreach (Vertice vertice in vertices)
            {
                if (!vertice.isIsolado()) //se algum vertice não for isolado, retorna falso
                {
                    return false;
                }
            }
            return true; //senão, retorna true
        }
        /// <summary>
        /// Retorna se o grafo é completo
        /// Um grafo completo é um grafo simples em que todo vértice é adjacente a todos os outros vértices.
        /// Não se pode checar apenas se a quantidade de vértices é igual ao tamanho correto
        /// </summary>
        /// <returns>True se for completo, false senão</returns>
        public bool isCompleto()
        {
            foreach (Vertice vertice in vertices)
            {
                foreach (Vertice vertice2 in vertices)
                {
                    if(vertice != vertice2) //tem que ser diferente pois não se testa se o vértice é adjacente a ele mesmo.
                    { 
                        if (!vertice.isAdjacente(vertice2)) //se algum vertice não for adjacente do outro, retorna falso
                        {
                            return false;
                        }
                    }
                }
            }
            return true; //senão, retorna true
        }
        /// <summary>
        /// Retorna se o grafo é conexo
        /// Um grafo é dito conexo se todos os pares de vértices estão ligados por um caminho.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção, já que o método foi feito para grafos não dirigidos.</exception>
        public bool isConexo()
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);

            //primeiro cria uma lista para armazenar os vertices percorridos
            List<Vertice> verticesPercorridos = new List<Vertice>(); //inicializando a variavel pois não pode mandar como parâmetro sem inicializar
            //Faz a chamada do metodo recursivo com o primeiro vertice do grafo
            verticesPercorridos = isConexo(vertices.First(), verticesPercorridos);
            foreach (Vertice vertice in vertices)
            {
                if (!verticesPercorridos.Contains(vertice)) //Se algum dos vértices não estiver na lista vertices percorridos, o grafo não é conexo
                {
                    return false; //portanto, retornamos falso.
                }
            }
            return true; //senão, retornamos true
        }
        private List<Vertice> isConexo(Vertice verticeAtual, List<Vertice> verticesPercorridos)
        {
            verticesPercorridos.Add(verticeAtual); //coloca o vertice atual na lista dos vertices percorridos
            foreach (Aresta aresta in verticeAtual.arestas)
            {
                if (!verticesPercorridos.Contains(aresta.getVerticeVizinho(verticeAtual))) //se o vertice vizinho ja esta no percorrido não vai até ele
                    verticesPercorridos = isConexo(aresta.getVerticeVizinho(verticeAtual), verticesPercorridos);
            }
            return verticesPercorridos; //retorna se não houver mais vértices vizinhos que não estão na lista de vertices percorridos
        }
        /// <summary>
        /// Retorna se o grafo é ou não eulariano
        /// Um grafo conexo,não orientado é euleriano se,e somente se,todos os seus vértices tiverem grau par
        /// </summary>
        /// <returns>true se for euleriano, false senão</returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção</exception>
        public bool isEuleriano()
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);

            if (!isConexo()) //primeiro verificamos se o grafo é conexo, se não for não é euleriano
                return false;//portanto, se não for conexo, retornamos false

            foreach (Vertice vertice in vertices)//checamos se todos os vertices tem grau par
            {
                if (vertice.getGrau() % 2 != 0) //se o grau do vertice não for par, retorna false
                    return false;
            }
            return true; // se as duas condições foram alcançadas, o grafo é euleriando, então retornamos true
        }

        /// <summary>
        /// Retorna se o grafo é ou não unicursal
        /// Um grafo é unicursal se e somente se ele possuir exatamente 2 vértices de grau ímpar
        /// </summary>
        /// <returns>true se for unicursal, false senão</returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção</exception>
        public bool isUnicursal()
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);


            if (!isConexo()) //primeiro verificamos se o grafo é conexo, se não for não é unicursal
                return false;//portanto, se não for conexo, retornamos false

            int qtdImpares = 0; //variavel para armazenar a quantidade de vertices de grau impar

            foreach (Vertice vertice in vertices)//percorremos todos os vertices para contar quantos tem grau ímpar
            {
                if (vertice.getGrau() % 2 != 0) //se o grau do vertice for ímpar adiciona um ao somador
                {
                    qtdImpares++;
                    if (qtdImpares > 2) //se tivermos mais que 2 vertices impares, o grafo não é unicursal
                        return false;   //portanto retornamos false
                }
            }
            if (qtdImpares == 2) //se as duas condições foram alcançadas, o grafo é unicursal, então retornamos true
                return true;
            else
                return false; //senão, retornamos false
        }

        /// <summary>
        /// Retorna a quantidade de cut vertices do grafo
        /// Uma cut vertice é uma vértice que, se removida, torna o grafo desconexo
        /// Assume-se que o grafo é conexo, se não o método é inútil e irá retornar a quantidade de vértices do grafo.
        /// </summary>
        /// <returns>int com a quantidade de cut vertices no grafo</returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção</exception>
        public int getCutVertices()
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);


            Grafo grafoCut; //objeto para armazenar os grafos clonados usados para testes
            int result = 0; //quantidade de cut vertices

            for (int i = 0; i < vertices.Count; i++) //processo sera repetido para cada vertice do grafo
            {
                grafoCut = this.clone(); //primeiramente clonamos o grafo para poder deletar um vertice dele
                grafoCut.vertices[i] = null; //removemos o vertice
                if (!grafoCut.isConexo()) //testamos se o grafo ficou desconexo
                    result++; //se ficou desconexo somamos
            }
            return result;
        }

        #region AGM
        /// <summary>
        /// Método que gera a árvore geradora mínima a partir do algoritmo de Prim
        /// Infelizmente esse método acopla a classe ao console, para que seja registrado o passo a passo da geração da árvore.
        /// </summary>
        /// <param name="v1">Vertice cabeça da árvore</param>
        /// <returns>Árvore Geradora mínima</returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção</exception>
        public Grafo getAGMPrim(Vertice v1)
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);

            int custo = 0;
            Grafo AGM = new Grafo(false); //cria o grafo da arvore geradora minima
            List<Vertice> verticesPercorridos = new List<Vertice>();
            List<Aresta> arestasPercorridas = new List<Aresta>();

            AGM.vertices.Add(new Vertice()); //adiciona o primeiro vertice
            verticesPercorridos.Add(v1);

            while(arestasPercorridas.Count() < this.vertices.Count()) //deve parar assim que adicionar n-1 arestas (n= num de vertices)
            {
                int menorArestaTamanho = int.MaxValue; //placeholder com o valor maximo
                Aresta menorAresta = null; //variavel que ira possuir a menor aresta
                foreach(Vertice vertice in verticesPercorridos) //para cada vertice percorrido vamos percorrer suas arestas
                {
                    foreach(Aresta aresta in vertice.arestas) //para cada aresta de cada vertice vamos escolher a com menor tamanho
                    {
                        if (!arestasPercorridas.Contains(aresta)) //primeiro verificamos se a aresta ja foi percorrida
                        {
                            if (aresta.peso < menorArestaTamanho) //verificamos se achamos uma aresta com peso menor
                            {
                                menorArestaTamanho = aresta.peso;
                                menorAresta = aresta;
                            }
                        }
                    }
                }
                //apos acharmos a aresta com menor tamanho, vamos adicionar ela e o vertice ao grafo
                if (verticesPercorridos.Contains(menorAresta.origem)) //pequeno teste para saber qual o certo, origem ou destino
                {    
                    verticesPercorridos.Add(menorAresta.destino);
                    Console.WriteLine("Vértice " + menorAresta.destino.id + " adicionado.");
                }
                else
                { 
                    verticesPercorridos.Add(menorAresta.origem);
                    Console.WriteLine("Vértice " + menorAresta.origem.id + " adicionado.");
                }
                //adicionando a aresta...
                arestasPercorridas.Add(menorAresta);
                Console.WriteLine("Aresta " + menorAresta.id + " adicionada, seu peso é " + menorAresta.peso);
                custo += menorAresta.peso; //adiciona ao custo
                Console.WriteLine();
            }
            Console.WriteLine("Custo total:" + custo);
            return AGM;
        }

        /// <summary>
        /// Método que gera a árvore geradora mínima a partir do algoritmo de Kruskal
        /// Infelizmente esse método acopla a classe ao console, para que seja registrado o passo a passo da geração da árvore.
        /// </summary>
        /// <param name="v1">Vertice cabeça da árvore</param>
        /// <returns>Árvore Geradora mínima</returns>
        /// <exception cref="TipoGrafoException">Se o grafo for dirigido será lançada uma exceção</exception>
        public Grafo getAGMKruskal(Vertice v1)
        {
            if (this.IsDirigido)
                throw new TipoGrafoException(false);

            int custo = 0;
            Grafo AGM = new Grafo(false); //cria o grafo da arvore geradora minima
            List<Vertice> verticesPercorridos = new List<Vertice>();
            List<Aresta> arestasPercorridas = new List<Aresta>();

            AGM.vertices.Add(new Vertice()); //adiciona o primeiro vertice
            int menorArestaTamanho = int.MaxValue;
            Aresta menorAresta = v1.arestas.First();

            //primeiro faz do vertice v1
            foreach (Aresta aresta in v1.arestas) //para cada aresta de cada vertice vamos escolher a com menor tamanho
            {
                if (aresta.peso < menorArestaTamanho) //verificamos se achamos uma aresta com peso menor
                {
                    menorArestaTamanho = aresta.peso;
                    menorAresta = aresta;
                }
            }
            verticesPercorridos.Add(v1);
            Console.WriteLine("Vértice " + v1.id + " adicionado.");
            //adicionando a aresta...
            arestasPercorridas.Add(menorAresta);
            Console.WriteLine("Aresta " + menorAresta.id + " adicionada, seu peso é " + menorAresta.peso);
            custo += menorAresta.peso; //adiciona ao custo
            Console.WriteLine();

            //agora faz do resto

            while (arestasPercorridas.Count() < this.vertices.Count()) //deve parar assim que adicionar n-1 arestas (n= num de vertices)
            {
                menorArestaTamanho = int.MaxValue; //placeholder com o valor maximo
                menorAresta = null; //variavel que ira possuir a menor aresta
                //NO FOR ABAIXO QUE SE DIFERE DO PRIM
                foreach (Vertice vertice in vertices) //para cada vertice vamos percorrer suas arestas
                {
                    foreach (Aresta aresta in vertice.arestas) //para cada aresta de cada vertice vamos escolher a com menor tamanho
                    {
                        if (!arestasPercorridas.Contains(aresta)) //primeiro verificamos se a aresta ja foi percorrida
                        {
                            if (!verticesPercorridos.Contains(aresta.getVerticeVizinho(vertice))) //agora checamos se o vertice vizinho já não foi percorrido
                            { 
                                if (aresta.peso < menorArestaTamanho) //verificamos se achamos uma aresta com peso menor
                                {
                                    menorArestaTamanho = aresta.peso;
                                    menorAresta = aresta;
                                }
                            }
                        }
                    }
                }
                //apos acharmos a aresta com menor tamanho, vamos adicionar ela e o vertice ao grafo
                if (verticesPercorridos.Contains(menorAresta.origem)) //pequeno teste para saber qual o certo, origem ou destino
                {
                    verticesPercorridos.Add(menorAresta.destino);
                    Console.WriteLine("Vértice " + menorAresta.destino.id + " adicionado.");
                }
                else
                {
                    verticesPercorridos.Add(menorAresta.origem);
                    Console.WriteLine("Vértice " + menorAresta.origem.id + " adicionado.");
                }
                //adicionando a aresta...
                arestasPercorridas.Add(menorAresta);
                Console.WriteLine("Aresta " + menorAresta.id + " adicionada, seu peso é " + menorAresta.peso);
                custo += menorAresta.peso; //adiciona ao custo
                Console.WriteLine();
            }
            Console.WriteLine("Custo total:" + custo);
            return AGM;
        }
        #endregion

        #endregion

        #region grafos dirigidos
        /// <summary>
        /// Retorna o grau de entrada do vértice escolhido
        /// Esse método está na classe Grafo pois é aqui que sabemos se o grafo é dirigido ou não
        /// </summary>
        /// <param name="v1">Vértice que se deseja o grau de entrada</param>
        /// <returns>Valor do grau de entrada</returns>
        /// <exception cref="TipoGrafoException">Se o grafo não for dirigido será lançada uma exceção</exception>
        public int getGrauEntrada(Vertice v1)
        {
            if (!this.IsDirigido)
                throw new TipoGrafoException(true);

            int result = 0;

            foreach (Aresta aresta in v1.arestas) //Será verificada todas as arestas
            {
                if (aresta.destino == v1) //Se o vértice de destino da aresta for v1, então essa aresta é de entrada
                    result++;
            }
            return result;
        }

        /// <summary>
        /// Retorna o grau de saída do vértice escolhido
        /// Esse método está na classe Grafo pois é aqui que sabemos se o grafo é dirigido ou não
        /// </summary>
        /// <param name="v1">Vértice que se deseja o grau de saída</param>
        /// <returns>Valor do grau de entrada</returns>
        /// <exception cref="TipoGrafoException">Se o grafo não for dirigido será lançada uma exceção</exception>
        public int getGrauSaida(Vertice v1)
        {
            if (!this.IsDirigido)
                throw new TipoGrafoException(true);

            int result = 0;

            foreach (Aresta aresta in v1.arestas) //Será verificada todas as arestas
            {
                if (aresta.origem == v1) //Se o vértice de origem da aresta for v1, então essa aresta é de saída
                    result++;
            }
            return result;
        }

        /// <summary>
        /// Retorna se o grafo tem um ciclo
        /// </summary>
        /// <returns>True se o grafo tem ciclo, false senão</returns>
        /// <exception cref="TipoGrafoException">Se o grafo não for dirigido será lançada uma exceção</exception>
        public bool hasCiclo()
        {
            if (!this.IsDirigido)
                throw new TipoGrafoException(true);

            //primeiro cria uma lista para armazenar os vertices percorridos, para que não 
            List<Vertice> verticesPercorridos = new List<Vertice>(); //inicializando a variavel pois não pode mandar como parâmetro sem inicializar
            foreach(Vertice vertice in vertices) //repete o processo para todos os vertices
            {
                if (hasCiclo(vertice, verticesPercorridos)) //testa se foi encontrado um ciclo
                    return true; //se foi encontrado, retorna true
            }
            return false; //se não foi encontrado nenhum ciclo, retornamos false
        }
        private bool hasCiclo(Vertice verticeAtual, List<Vertice> verticesPercorridos)
        {
            verticesPercorridos.Add(verticeAtual); //coloca o vertice atual na lista dos vertices percorridos
            foreach (Aresta aresta in verticeAtual.arestas)
            {
                if (verticesPercorridos.Contains(aresta.getVerticeVizinho(verticeAtual))) //se o vertice vizinho ja esta no percorrido, então há ciclo
                    return true;
            }
            return false; //retorna false se não encontrou nenhum ciclo
        }
        #endregion

        #endregion
    }
}
