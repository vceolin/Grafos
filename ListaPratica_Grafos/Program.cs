using System;
using System.IO;

namespace ListaPratica_Grafos
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream teste = File.Open("grafo.txt", FileMode.Open);//Leitura do arquivo com os dados
            StreamReader file = new StreamReader(teste); //Instancia do objeto de leitura
            Leitura(file);
        }

        public static void Leitura(StreamReader arq)
        {
            int op =1;
            int nv1 = 0;
            int nv2 = 0;
            string[] Linha = new string[4];//Vetor que vai receber os dados do arquivo
            int vertices = int.Parse(arq.ReadLine());//Numero dos vertices
            string linha = arq.ReadLine();
            Linha = linha.Split(';');
            Console.WriteLine("-Trabalho Pratico Algoritmo em grafos-");
            Console.WriteLine("Integrantes: Vitor Ceolin & Fernando Bretz");

            Grafo g = new Grafo();

            if (Linha.Length == 3)
            {   //Não é dirigido
                g = new Grafo(false, vertices);
                while (linha != null)
                {
                    Linha = linha.Split(';');//Pegando dados da linha
                    Aresta aresta = new Aresta(g.vertices[int.Parse(Linha[0]) - 1], g.vertices[int.Parse(Linha[1]) - 1], int.Parse(Linha[2]));            
                    linha = arq.ReadLine();//Pula linha
                }

            }
            else if (Linha.Length == 4) 
            {   //É dirigido 
                g = new Grafo(true, vertices);
                while (linha != null)
                {
                    Linha = linha.Split(';');//Pegando dados da linha
                    if (Linha[3].Equals("1"))
                    {
                        Aresta aresta = new Aresta(g.vertices[int.Parse(Linha[0]) - 1], g.vertices[int.Parse(Linha[1]) - 1], int.Parse(Linha[2]));
                    }
                    else
                    {
                        Aresta aresta = new Aresta(g.vertices[int.Parse(Linha[1]) - 1], g.vertices[int.Parse(Linha[0]) - 1], int.Parse(Linha[2]));
                    }
                        linha = arq.ReadLine();//Pula linha
                }

            }
            if (!g.IsDirigido)
            {   
                
                while (op != 0)

                { //Menu de Metodos para grafo não dirigido
                    Console.WriteLine("Escolha um metodo (grafo não dirigido):\n0-Sair\n1-isAdjacente()\n2-getGrau()\n3-isIsolado()\n4-isPendente()" +
                    "\n5-isRegular()\n6-isNulo()\n7-isCompleto()\n8-isConexo()\n9-isEuleriano()\n10-isUnicursal()" +
                    "\n11-getAGMPrim()\n12-getAGMKruskal()\n13-getCutVertices()");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("Qual será o primeiro vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            Console.WriteLine("Qual será o segundo vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv2 = int.Parse(Console.ReadLine());
                            if (g.vertices[nv1 - 1].isAdjacente(g.vertices[nv2 - 1]))
                            {
                                Console.WriteLine("O vértice é adjacente.");
                            }
                            else
                            {
                                Console.WriteLine("O vértice não é adjacente.");

                            }
                            break;
                        case 2:
                            Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            Console.WriteLine("O grau é: " + g.vertices[nv1 - 1].getGrau());
                            break;
                        case 3:
                            Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            if (g.vertices[nv1 - 1].isIsolado())
                            {
                                Console.WriteLine("O vértice é isolado.");
                            }
                            else
                            {
                                Console.WriteLine("O vértice não é isolado.");

                            }
                            break;
                        case 4:
                            Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            if (g.vertices[nv1 - 1].isPendente())
                            {
                                Console.WriteLine("O vértice é pendente.");
                            }
                            else
                            {
                                Console.WriteLine("O vértice não é pendente.");

                            }
                            break;
                        case 5:

                            if (g.isRegular())
                            {
                                Console.WriteLine("O grafo é regular.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é regular.");

                            }
                            break;
                        case 6:

                            if (g.isNulo())
                            {
                                Console.WriteLine("O grafo é nulo.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é nulo.");

                            }
                            break;
                        case 7:

                            if (g.isCompleto())
                            {
                                Console.WriteLine("O grafo é completo.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é completo.");

                            }
                            break;
                        case 8:

                            if (g.isConexo())
                            {
                                Console.WriteLine("O grafo é conexo.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é conexo.");

                            }
                            break;
                        case 9:

                            if (g.isEuleriano())
                            {
                                Console.WriteLine("O grafo é Euleriano.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é Euleriano.");

                            }
                            break;
                        case 10:

                            if (g.isUnicursal())
                            {
                                Console.WriteLine("O grafo é Unicursal.");
                            }
                            else
                            {
                                Console.WriteLine("O grafo não é Unicursal.");

                            }
                            break;
                        case 11:
                            Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            g.getAGMPrim(g.vertices[nv1 - 1]);

                            break;
                        case 12:
                            Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                            nv1 = int.Parse(Console.ReadLine());
                            g.getAGMKruskal(g.vertices[nv1 - 1]);
                            break;
                        case 13:
                            Console.WriteLine("O número de cut-vértices é: " + g.getCutVertices());
                            break;

                        default:
                            Console.WriteLine("Opção invalida :(");
                            break;
                    }
                }
            }
            else
            {
               
                while (op!=0) {
                    //Menu de Metodos para grafo dirigido 
                    Console.WriteLine("Escolha um metodo (grafo dirigido):\n0-Sair\n1-getGrauEntrada()\n2-getGrauSaida()\n3-hasCiclo()");
                    op = int.Parse(Console.ReadLine());
                    switch (op)
                     {
                    case 1:
                        Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                        nv1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("O grau de entrada do vértice é: " + g.getGrauEntrada(g.vertices[nv1 - 1]));
                        
                        break;
                    case 2:
                        Console.WriteLine("Qual será o  vértice? (Escolha um numero entre 1-" + (g.vertices.Count) + ")");
                        nv1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("O grau de saida do vértice é: " + g.getGrauSaida(g.vertices[nv1 - 1]));
                        break;
                    case 3:
                        if (g.hasCiclo())
                        {
                            Console.WriteLine("O grafo possui ciclo.");
                        }
                        else
                        {
                            Console.WriteLine("O grafo não possui ciclo.");

                        }
                        break;
                    default:
                        Console.WriteLine("Opção invalida :(");
                        break;
                    }
                }
            }
        }
    }

}
