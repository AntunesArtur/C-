using System.ComponentModel.Design;
using System.IO;
using System;
using System.Runtime.ConstrainedExecution;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;

int nivel_jogo = 0, contador, contador_jogador1 = 0, contador_jogador2 = 0, l = 1, opcao_nivel_jogo = 0, opcao_jogo = 0;
String jogador1, jogador2 = null, vez_jogador;
bool terminar, carater_num = false;
string valida_numero = null;


DirectoryInfo pasta = new DirectoryInfo(@"c:\ficheiros");

FileInfo[] todos_ficheiros = pasta.GetFiles("*.*");


if ((todos_ficheiros.Where(x => x.Name == "highscores_batalha_naval.txt").Count()) == 0) //verificar se ficheiro já existe para não ciar um novo e eliminar os dados que existiam no anterior
{
    FileInfo ficheiro1 = new FileInfo(@"c:\ficheiros\highscores_batalha_naval.txt"); // criação de ficheiro para gravar highscores
    FileStream ficheiro = ficheiro1.Create();
    ficheiro.Close();
}


do
{
    Console.WriteLine();
    Console.WriteLine("                    REGRAS DE JOGO:");                              // mostrar regras de jogo
    Console.WriteLine();
    Console.WriteLine("- Estão disponíveis duas versões de jogo, Jogador 1 Vs Jogador 2 ou Jogador 1 Vs Computador.");
    Console.WriteLine("- Na escolha da dimensão do campo de batalha tem 3 níveis para opção, 10x10, 15x15 e 20x20 linhas(L) x colunas(C).");
    Console.WriteLine("- As linhas e as colunas estão numeradas de 0 a 9, 0 a 14 e 0 a 19 de acordo com o nível escolhido.");
    Console.WriteLine("- Em cada jogada o jogador tem direito a um tiro definido pelas coordenadas (L, C).");
    Console.WriteLine("- Se o tiro acertar em água será marcado com a letra 'X', se acertar num navio é marcado com a letra 'F'.");
    Console.WriteLine("- Cada jogador tem direito a marcar no tabuleiro de jogo os seguintes navios:");
    Console.WriteLine("  - 2 Porta-aviões com 4 posições");
    Console.WriteLine("  - 2 Fragatas com 3 posições");
    Console.WriteLine("  - 2 Corvetas com 2 posições");
    Console.WriteLine("  - 2 Submarinos com 1 posição");
    Console.WriteLine("- Vence quem afundar primeiro todos os navios do adversário.");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Pressione uma tecla para iniciar o JOGO.");
    Console.ResetColor();
}
while (Console.ReadKey() == null); // para manter as regras no ecrã enquanto o jogador não carregar numa tecla

Console.Clear();
Console.WriteLine();
Console.WriteLine("   Escolha uma opção de Jogo:");
Console.WriteLine();
Console.WriteLine("1 - Jogador 1 Vs Jogador 2.");
Console.WriteLine("2 - Jogador 1 Vs Computador");

carater_num = false;
valida_numero = null;

while (carater_num == false) // validação dos números introduzidos para evitar erros no programa
{
    Console.WriteLine();
    valida_numero = Console.ReadLine();

    if (valida_numero == "1" || valida_numero == "2")
    {
        carater_num = true;
        opcao_jogo = int.Parse(valida_numero);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine($"A opção escolhida não é válida, introduza novamente a opção pretendida.");
        Console.ResetColor();
    }
}

Console.Clear();
Console.WriteLine();
Console.WriteLine("Em que nível pretende jogar?");
Console.WriteLine();
Console.WriteLine("1 Nível - Campo de batalha 10x10");
Console.WriteLine("2 Nível - Campo de batalha 15x15");
Console.WriteLine("3 Nível - Campo de batalha 20x20");

carater_num = false;
valida_numero = null;

while (carater_num == false) // validação dos números introduzidos para evitar erros no programa
{
    Console.WriteLine();
    valida_numero = Console.ReadLine();

    if (valida_numero == "1" || valida_numero == "2" || valida_numero == "3")
    {
        carater_num = true;
        opcao_nivel_jogo = int.Parse(valida_numero);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine($"A opção escolhida não é válida, introduza novamente a opção pretendida.");
        Console.ResetColor();
    }
}


if (opcao_nivel_jogo == 1)
{
    nivel_jogo = 10;
}
if (opcao_nivel_jogo == 2)
{
    nivel_jogo = 15;
}
if (opcao_nivel_jogo == 3)
{
    nivel_jogo = 20;
}

string[,] tabuleiro_1_jogador_1 = new string[nivel_jogo, nivel_jogo];
string[,] tabuleiro_2_jogador_1 = new string[nivel_jogo, nivel_jogo];
string[,] tabuleiro_1_jogador_2 = new string[nivel_jogo, nivel_jogo];
string[,] tabuleiro_2_jogador_2 = new string[nivel_jogo, nivel_jogo];
int[,] matriz_memoria_jogador1 = new int[8, 8];
int[,] matriz_memoria_jogador2 = new int[8, 8];

carregar_matriz_inicio(tabuleiro_1_jogador_1, tabuleiro_2_jogador_1); // carregar tabuleiros jogador 1
carregar_matriz_inicio(tabuleiro_1_jogador_2, tabuleiro_2_jogador_2); // carregar tabuleiros jogador 2


Console.Clear();
Console.WriteLine("Jogador 1:");                                    //pedir nome jogador 1
Console.WriteLine();
Console.WriteLine("Escreva o seu nome.");
jogador1 = Console.ReadLine();
Console.Clear();

carregar_navios(jogador1, tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, matriz_memoria_jogador1, opcao_nivel_jogo, opcao_jogo); // carregar navios jogador 1

if (opcao_jogo == 1) // carrega matriz de jogo de acordo com a opcao de jogo, jogador1 VS jogador2 ou jogador1 VS Computador
{
    Console.Clear();
    Console.WriteLine("Jogador 2:");                                    //pedir nome jogador 2
    Console.WriteLine();
    Console.WriteLine("Escreva o seu nome.");
    jogador2 = Console.ReadLine();
    Console.Clear();

    carregar_navios(jogador2, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, matriz_memoria_jogador2, opcao_nivel_jogo, opcao_jogo); // carregar navios jogador 2
    Console.Clear();
}
if (opcao_jogo == 2)
{
    jogador2 = "Computador";

    carregar_navios(jogador2, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, matriz_memoria_jogador2, opcao_nivel_jogo, opcao_jogo); // carregar navios jogador 2
    Console.Clear();
}


terminar = false;                                                        // inicio de jogo
vez_jogador = jogador1;

while (terminar == false)
{
    Console.Clear();

    if (vez_jogador == jogador1)
    {
        mostrar_matriz(tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, vez_jogador, opcao_nivel_jogo);  // apresentação de matrizes de jogo jogador 1
        Console.WriteLine();
    }
    if (vez_jogador == jogador2 && vez_jogador != "Computador")
    {
        mostrar_matriz(tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, vez_jogador, opcao_nivel_jogo); // apresentação de matrizes de jogo jogador 2
        Console.WriteLine();
    }


    contador = 0;
    while (contador < 1)
    {

        int linha = 0;
        int coluna = 0;

        if (vez_jogador != "Computador")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{vez_jogador}");
            Console.ResetColor();
            Console.WriteLine($", marque a linha para o tiro");
            linha = validacao_numero_teclado(vez_jogador); // para validação dos números introduzidos no teclado e evitar que o programa bloqueie

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{vez_jogador}");
            Console.ResetColor();
            Console.WriteLine($", marque a coluna para o tiro");
            coluna = validacao_numero_teclado(vez_jogador); // para validação dos números introduzidos no teclado e evitar que o programa bloqueie
        }
        if (vez_jogador == "Computador")
        {
            linha = auto_gerar_linha_coluna(opcao_nivel_jogo);
            coluna = auto_gerar_linha_coluna(opcao_nivel_jogo);
        }

        if (vez_jogador == jogador1) // validação de tiros do jogador 1
        {
            if (linha < 0 || linha > tabuleiro_2_jogador_1.GetLength(0) || coluna < 0 || coluna > tabuleiro_1_jogador_2.GetLength(1) || (tabuleiro_1_jogador_2[linha, coluna] == "X ") || (tabuleiro_1_jogador_2[linha, coluna] == "F ")) // para validar se já tinha sido dado tiro naquela posição
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Já tem um tiro registado nessa posição, escolha novas coordenadas.");
                Console.ResetColor();
            }
            else
            {
                contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, tabuleiro_1_jogador_2, matriz_memoria_jogador2, 4, 0, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser um Porta-aviões

                if (contador < 1) // vários "if" para quando a condição deixar de ser == 1 já não fazer os restantes
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, tabuleiro_1_jogador_2, matriz_memoria_jogador2, 3, 2, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser uma Fregata
                }

                if (contador < 1)
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, tabuleiro_1_jogador_2, matriz_memoria_jogador2, 2, 4, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser uma Curveta                                                                               
                }

                if (contador < 1)
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_1, tabuleiro_2_jogador_1, tabuleiro_1_jogador_2, matriz_memoria_jogador2, 1, 6, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser um Submarino
                }

                if (contador == 1)  // para não contar jogada quando as coordenadas não são válidas
                {
                    contador_jogador1++;
                }
            }

        }

        if (vez_jogador == jogador2) // // validação de tiros do jogador 2
        {
            if (linha < 0 || linha > tabuleiro_1_jogador_1.GetLength(0) || coluna < 0 || coluna > tabuleiro_1_jogador_1.GetLength(1) || (tabuleiro_1_jogador_1[linha, coluna] == "X ") || (tabuleiro_1_jogador_1[linha, coluna] == "F ")) // para validar se já tinha sido dado tiro naquela posição
            {
                if (vez_jogador != "Computador")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine($"Já tem um tiro registado nessa posição, escolha novas coordenadas.");
                    Console.ResetColor();
                }
            }

            else
            {
                contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, tabuleiro_1_jogador_1, matriz_memoria_jogador1, 4, 0, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser um Porta-aviões

                if (contador < 1)
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, tabuleiro_1_jogador_1, matriz_memoria_jogador1, 3, 2, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser uma Fregata
                }

                if (contador < 1)
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, tabuleiro_1_jogador_1, matriz_memoria_jogador1, 2, 4, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser uma Curveta   
                }

                if (contador < 1)
                {
                    contador = validar_tiro(linha, coluna, tabuleiro_1_jogador_2, tabuleiro_2_jogador_2, tabuleiro_1_jogador_1, matriz_memoria_jogador1, 1, 6, 0, vez_jogador, opcao_nivel_jogo);   // validação de tiro de jogador 1 e registo nas matrizes para a hipótese de ser um Submarino
                }

                if (contador == 1)  // para não contar jogada quando as coordenadas não são válidas
                {
                    contador_jogador2++;
                }
            }

        }
    }
    if (vez_jogador == jogador1) // mudança de jogador
    {
        vez_jogador = jogador2;
    }
    else
    {
        vez_jogador = jogador1;
    }


    if (contagem_tiros_certeiros(tabuleiro_1_jogador_2) == 20) // contagem de tiros em navios dados pelo jogador1
    {
        Console.WriteLine();
        Console.WriteLine($"Parabéns {jogador1}, ganhou o JOGO, afundou os 8 navios do adversário em {contador_jogador1} jogadas.");
        terminar = true;
    }
    if (contagem_tiros_certeiros(tabuleiro_1_jogador_1) == 20) // contagem de tiros em navios dados pelo jogador2
    {
        if (vez_jogador != "Computador")
        {
            Console.WriteLine();
            Console.WriteLine($"Parabéns {jogador2}, ganhou o JOGO, afundou os 8 navios do adversário em {contador_jogador2} jogadas.");
            terminar = true;
        }
        if (vez_jogador == "Computador")
        {
            Console.WriteLine();
            Console.WriteLine($"O computador ganhou o Jogo.");
            terminar = true;
        }
    }

}

List<Highscores> lista = new List<Highscores>(); // criação de lista

if (vez_jogador == jogador2) // carrega lista de highscores caso vença o jogador 1
{
    Highscores registo = new Highscores();
    registo.adicionar_pontuacao(jogador1, contador_jogador1);
    lista.Add(registo);

}

if (vez_jogador == jogador1) // carrega lista de highscores caso vença o jogador 2
{
    Highscores registo = new Highscores();
    registo.adicionar_pontuacao(jogador2, contador_jogador2);
    lista.Add(registo);
}


StreamReader leitor = File.OpenText(@"c:\ficheiros\highscores_batalha_naval.txt"); // leitura de ficheiro para lista

string linhas = null;

while ((linhas = leitor.ReadLine()) != null)  // carregar lista com dados do ficheiro
{
    string nome = linhas.Substring(0, linhas.IndexOf('&'));
    int pontos = int.Parse(linhas.Substring(linhas.IndexOf('&') + 1));

    Highscores registo = new Highscores();
    registo.adicionar_pontuacao(nome, pontos);
    lista.Add(registo);
}
leitor.Close();


FileInfo ficheiro2 = new FileInfo(@"c:\ficheiros\highscores_batalha_naval.txt"); // grava highscore no fiheiro
StreamWriter escrita = ficheiro2.CreateText();


foreach (Highscores h in lista.OrderBy(x => x.Pontuacao))
{
    escrita.WriteLine($"{h.Nome}&{h.Pontuacao}");
}
escrita.Close();


Thread.Sleep(4000);
Console.Clear();

Console.WriteLine($"                 HIGHSCORES");   // mostrar highscores no ecrã
Console.WriteLine();
Console.WriteLine($"CLASSIFICAÇÂO       NOME           PONTUAÇÃO");
Console.WriteLine();

foreach (Highscores h in lista.OrderBy(x => x.Pontuacao))
{
    Console.WriteLine($"    {l.ToString().PadLeft(2)} -    {h.Nome.PadRight(15)}     -     {h.Pontuacao.ToString().PadLeft(2)}");
    l++;
}

// função para ajustar largura de colunas e linhas ao mostrar as matrizes de jogo

static void matriz_padleft(int num_padleft, int letra_padleft, string[,] matriz_jogo, string[,] matriz_tiros)
{
    for (int i = 0; i < matriz_jogo.GetLength(0); i++)
    {
        Console.WriteLine();
        Console.Write($"     {i.ToString().PadLeft(num_padleft)} ");
        for (int j = 0; j < matriz_jogo.GetLength(1); j++) //marcação matriz de jogo
        {
            if (matriz_jogo[i, j] == "A ")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{matriz_jogo[i, j].PadLeft(letra_padleft)}");
            }
            else if (matriz_jogo[i, j] == "X ")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{matriz_jogo[i, j].PadLeft(letra_padleft)}");
            }
            else if (matriz_jogo[i, j] == "F ")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{matriz_jogo[i, j].PadLeft(letra_padleft)}");
            }
            else if (matriz_jogo[i, j] == "N ")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{matriz_jogo[i, j].PadLeft(letra_padleft)}");
            }

        }
        Console.ResetColor();
        Console.Write($"       ");
        Console.Write($"{i.ToString().PadLeft(2)} ");
        for (int j = 0; j < matriz_tiros.GetLength(1); j++)  //marcação matriz de tiros
        {
            if (matriz_tiros[i, j] == "A ")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{matriz_tiros[i, j].PadLeft(letra_padleft)}");
            }
            else if (matriz_tiros[i, j] == "X ")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{matriz_tiros[i, j].PadLeft(letra_padleft)}");
            }
            else if (matriz_tiros[i, j] == "F ")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{matriz_tiros[i, j].PadLeft(letra_padleft)}");
            }
        }
        Console.ResetColor();
    }
}

// função para mostrar mensagens para introdução do número de linha para carregar navios de acordo com o tipo de navio

static int linha_carregar_navios(int linha, int coluna, string nome_jogador, int contador, string nome_navio)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"{nome_jogador}");
    Console.ResetColor();
    Console.WriteLine($", marque a linha para o navio {contador + 1} ({nome_navio}).");
    linha = validacao_numero_teclado(nome_jogador);

    return linha;
}


// função para mostrar mensagens para introdução do número de linha para carregar navios de acordo com o tipo de navio

static int coluna_carregar_navios(int linha, int coluna, string nome_jogador, int contador, string nome_navio)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"{nome_jogador}");
    Console.ResetColor();
    Console.WriteLine($", marque a coluna para o navio {contador + 1} ({nome_navio}).");
    coluna = validacao_numero_teclado(nome_jogador);
    return coluna;
}

// função para gerar numero de linha e coluna de forma automatica na versão de jogo jogador1 Vs Computador

static int auto_gerar_linha_coluna(int opcao_nivel_jogo)
{
    Random aleatorio = new Random();

    int num_gerado = 0;

    if (opcao_nivel_jogo == 1)
    {
        num_gerado = aleatorio.Next(0, 10);
    }
    if (opcao_nivel_jogo == 2)
    {
        num_gerado = aleatorio.Next(0, 15);
    }
    if (opcao_nivel_jogo == 3)
    {
        num_gerado = aleatorio.Next(0, 20);
    }

    return num_gerado;
}

// função para validação de números introduzidos no teclado pelo jogador

static int validacao_numero_teclado(string vez_jogador)
{
    bool carater_num = false;
    string valida_numero = null;

    while (carater_num == false) // validação dos números introduzidos para evitar que o programa bloqueie
    {
        Console.WriteLine();
        valida_numero = Console.ReadLine();

        if (valida_numero == "0" || valida_numero == "1" || valida_numero == "2" || valida_numero == "3" || valida_numero == "4" || valida_numero == "5" || valida_numero == "6" || valida_numero == "7" || valida_numero == "8" || valida_numero == "9" ||
        valida_numero == "10" || valida_numero == "11" || valida_numero == "12" || valida_numero == "13" || valida_numero == "14" || valida_numero == "15" || valida_numero == "16" || valida_numero == "17" || valida_numero == "18" || valida_numero == "19")
        {
            carater_num = true;
            int.Parse(valida_numero);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine($"{vez_jogador}, o número escolhido não é válido, introduza novamente o número pretendido.");
            Console.ResetColor();
        }
    }
    return int.Parse(valida_numero);
}



// função para registar navios nas matrizes

static void registo_navios(int linha, int coluna, int tamanho_navio, int linha_inicial_matriz_memoria, int coluna_inicial_matriz_memoria, string[,] matriz_jogo, int[,] matriz_memoria, string orientacao)
{
    if (orientacao == "H")
    {
        for (int i = 0; i < tamanho_navio; i++)
        {
            matriz_jogo[linha, coluna + i] = "N "; // marca posições do navio na matriz de jogo
            matriz_memoria[linha_inicial_matriz_memoria, coluna_inicial_matriz_memoria + i] = linha; // guarda coordenada da linha na matriz de memoria para saber onde o navio ficou posicionado
            matriz_memoria[linha_inicial_matriz_memoria + 1, coluna_inicial_matriz_memoria + i] = coluna + i; // // guarda coordenada da coluna na matriz de memoria para saber onde o navio ficou posicionado
        }
    }
    if (orientacao == "V")
    {
        for (int i = 0; i < tamanho_navio; i++)
        {
            matriz_jogo[linha + i, coluna] = "N "; // marca posições do navio na matriz de jogo
            matriz_memoria[linha_inicial_matriz_memoria, coluna_inicial_matriz_memoria + i] = linha + i; // guarda coordenada da linha na matriz de memoria para saber onde o navio ficou posicionado
            matriz_memoria[linha_inicial_matriz_memoria + 1, coluna_inicial_matriz_memoria + i] = coluna; // // guarda coordenada da coluna na matriz de memoria para saber onde o navio ficou posicionado
        }
    }
}

// função contagem de navios afundados

static int contagem_tiros_certeiros(string[,] matriz_jogo_adversario)
{
    int contador = 0;

    for (int i = 0; i < matriz_jogo_adversario.GetLength(0); i++)
    {
        for (int j = 0; j < matriz_jogo_adversario.GetLength(1); j++)
        {
            if (matriz_jogo_adversario[i, j] == "F ")
            {
                contador++;
            }
        }
    }
    return contador;
}

// função para validar posição dos navios quando são marcados no campo de batalha

static int validar_marcacao_navios(int linha, int coluna, int tamanho_navio, string[,] matriz_jogo, string orientacao)
{
    int contador1 = 0;


    if (orientacao == "H") // verifica se não há nenhum navio nas posições que se pretendem marcar, no caso de posição horizontal
    {
        if (linha < 0 || linha > matriz_jogo.GetLength(0) || coluna < 0 || coluna > (matriz_jogo.GetLength(1) - tamanho_navio)) // verifica se a posição está dentro dos valores aceitáveis de forma ao navio não sair fora da tabela
        {
            contador1++;
        }
        else
        {
            for (int i = 0; i < tamanho_navio; i++)
            {
                if ((matriz_jogo[linha, coluna + i] == "N "))
                {
                    contador1++;
                }
            }
        }
    }

    if (orientacao == "V")
    {
        if (linha < 0 || linha > (matriz_jogo.GetLength(0) - tamanho_navio) || coluna < 0 || coluna > matriz_jogo.GetLength(1))
        {
            contador1++;
        }
        else
        {
            for (int i = 0; i < tamanho_navio; i++)
            {
                if ((matriz_jogo[linha + i, coluna] == "N "))
                {
                    contador1++;
                }
            }
        }
    }
    return contador1;
}


// função que avalia se o tiro foi válido, se válido, faz o registo nas matrizes, avisa o jogador de foi fogo ou água e retorna um valor para informar que a jogada terminou
static int validar_tiro(int linha, int coluna, string[,] matriz_jogo, string[,] matriz_tiros, string[,] matriz_jogo_adversario, int[,] matriz_memoria, int tamanho_navio, int linha_inicial_matriz_memoria, int coluna_inicial_matriz_memoria, string nome_jogador, int opcao_nivel_jogo)
{
    int contador = 0, linha_memoria = 0, linha_memoria2 = 0, coluna_memoria = 0, contador2 = 0, contador3 = 0;


    if (matriz_jogo_adversario[linha, coluna] == "A ")
    {
        matriz_jogo_adversario[linha, coluna] = "X ";
        matriz_tiros[linha, coluna] = "X ";
        Console.Clear();
        if (nome_jogador != "Computador")
        {
            mostrar_matriz(matriz_jogo, matriz_tiros, nome_jogador, opcao_nivel_jogo);
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"O tiro foi na água!");
        Console.ResetColor();
        Thread.Sleep(3000);
        contador++; // para dar saída de função de tiro válido
    }

    if (matriz_jogo_adversario[linha, coluna] == "N ")
    {
        matriz_jogo_adversario[linha, coluna] = "F ";
        matriz_tiros[linha, coluna] = "F ";
        Console.Clear();
        if (nome_jogador != "Computador")
        {
            mostrar_matriz(matriz_jogo, matriz_tiros, nome_jogador, opcao_nivel_jogo);
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"O tiro foi fogo!");
        Console.ResetColor();
        Console.WriteLine();
        Thread.Sleep(2000);
    }
    int j = 0; // validar na matriz memória em qual dos navios acertou e se afundou algum. Esta variável é para acrescentar no segundo ciclo do primeiro "for" o tamanho do à posição da coluna por forma a fazer a verificação para os segundos navios de cada classe

    for (
        int h = 0; h < 2; h++)
    {
        for (int i = 0; i < tamanho_navio; i++)
        {
            if (matriz_memoria[linha_inicial_matriz_memoria, coluna_inicial_matriz_memoria + i + j] == linha && matriz_memoria[linha_inicial_matriz_memoria + 1, coluna_inicial_matriz_memoria + i + j] == coluna) // Valida se as coordenadas de tiro existem na matriz memória do jogo do adversário, basta colocar as coordenadas da primeira posição onde estão guardadas as coordedadas de cada tipo de navio
            {
                coluna_memoria = coluna_inicial_matriz_memoria + i + j;
                linha_memoria = linha_inicial_matriz_memoria;
                linha_memoria2 = linha_inicial_matriz_memoria + 1;
                contador++; //para dar saída de função de tiro válido

                if (h == 0) // para garantir que está nas primeiras posições dos navios
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    if (tamanho_navio == 4)
                    {
                        Console.WriteLine("Acertou no Porta-Aviões 1.");
                    }
                    if (tamanho_navio == 3)
                    {
                        Console.WriteLine("Acertou na Fragata 1.");
                    }
                    if (tamanho_navio == 2)
                    {
                        Console.WriteLine("Acertou na Corveta 1.");
                    }
                    if (tamanho_navio == 1)
                    {
                        Console.WriteLine("Acertou no Submarino 1.");
                    }
                    Console.ResetColor();
                }

                if (h == 1) //para garantir que está nas segundas posições dos navios
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    if (tamanho_navio == 4)
                    {
                        Console.WriteLine("Acertou no Porta-Aviões 2.");
                    }
                    if (tamanho_navio == 3)
                    {
                        Console.WriteLine("Acertou na Fragata 2.");
                    }
                    if (tamanho_navio == 2)
                    {
                        Console.WriteLine("Acertou na Corveta 2.");
                    }
                    if (tamanho_navio == 1)
                    {
                        Console.WriteLine("Acertou no Submarino 2.");
                    }
                    Console.ResetColor();
                }
            }

            if (matriz_jogo_adversario[(matriz_memoria[linha_inicial_matriz_memoria, coluna_inicial_matriz_memoria + i + j]), (matriz_memoria[linha_inicial_matriz_memoria + 1, coluna_inicial_matriz_memoria + i + j])] == "F ") // verifica as posições que estão guardadas na matriz de memória já foram atingidas, em caso afirmativo faz a contagem e determina se o navio foi afundado ou não
            {
                if (h == 0)
                {
                    contador2++; // para fazer a contagem das posição com "F"
                }
                if (h == 1)
                {
                    contador3++;
                }
            }
        }
        j = j + tamanho_navio; // para incrementar na posição das colunas e evitar estar a introduzir novamente a posição da coluna na matriz para verificar se houve tiro no segundo barco da mesma classe   
    }
    if (contador2 == tamanho_navio && coluna_memoria < tamanho_navio && matriz_memoria[linha_memoria, coluna_memoria] == linha && matriz_memoria[linha_memoria2, coluna_memoria] == coluna)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        if (linha_memoria == 0)
        {
            Console.WriteLine("Afundou o Porta-Aviões 1.");
        }
        if (linha_memoria == 2)
        {
            Console.WriteLine("Afundou a Fragata 1.");
        }
        if (linha_memoria == 4)
        {
            Console.WriteLine("Afundou a Corveta 1.");
        }
        if (linha_memoria == 6)
        {
            Console.WriteLine("Afundou o Submarino 1.");
        }
        Console.ResetColor();
    }
    if (contador3 == tamanho_navio && coluna_memoria >= tamanho_navio && coluna_memoria < tamanho_navio * 2)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        if (linha_memoria == 0)
        {
            Console.WriteLine("Afundou o Porta-Aviões 2.");
        }
        if (linha_memoria == 2)
        {
            Console.WriteLine("Afundou a Fragata 2.");
        }
        if (linha_memoria == 4)
        {
            Console.WriteLine("Afundou a Corveta 2.");
        }
        if (linha_memoria == 6)
        {
            Console.WriteLine("Afundou o Submarino 2.");
        }
        Console.ResetColor();
    }
    Thread.Sleep(3000);
    return contador;
}


// função para carregar navios
static void carregar_navios(String nome_jogador, string[,] matriz_jogo, string[,] matriz_tiros, int[,] matriz_memoria, int opcao_nivel_jogo, int opcao_jogo)
{
    int num_orientacao;

    Random aleatorio = new Random();

    if (nome_jogador != "Computador") // Para não mostar as matrizes do jogador2 quando este é o computador
    {
        mostrar_matriz(matriz_jogo, matriz_tiros, nome_jogador, opcao_nivel_jogo);
        Console.WriteLine();
    }


    int contador = 0, linha = 0, coluna = 0, tamanho_navio = 0;
    string orientacao = null;

    while (contador < 8) // o valor definido aqui determina o número total de navios a carregar
    {
        bool carater = false;

        while (carater == false)
        {
            if (nome_jogador != "Computador") // Quando a opcao de jogo é jogador1 VS jogador2)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{nome_jogador}");
                Console.ResetColor();
                Console.WriteLine($", que orientação pretende para o navio, 'V' ou 'H'?");
                orientacao = Console.ReadLine().ToUpper();
            }

            else // escolha da orientação quando a versão de jogo é jogador1 Vs Computador
            {
                if (aleatorio.Next(1, 3) == 1)
                {
                    orientacao = "V";
                }
                else
                {
                    orientacao = "H";
                }

            }

            if (orientacao == "V" || orientacao == "H")
            {
                carater = true;
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{nome_jogador}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($", o carater escolhido não é válido, introduza novamente a orientação pretendida.");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        if (contador < 2) // para marcação dos porta-aviões
        {
            if (nome_jogador != "Computador") //Para só executar quando o jogador não é computador
            {
                linha = linha_carregar_navios(linha, coluna, nome_jogador, contador, "Porta-aviões - 4 posições");
                coluna = coluna_carregar_navios(linha, coluna, nome_jogador, contador, "Porta-aviões - 4 posições");
            }
            if (nome_jogador == "Computador")
            {
                linha = auto_gerar_linha_coluna(opcao_nivel_jogo);
                coluna = auto_gerar_linha_coluna(opcao_nivel_jogo);
            }

        }

        if (contador >= 2 && contador < 4) // para marcação das fragatas
        {
            if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
            {
                linha = linha_carregar_navios(linha, coluna, nome_jogador, contador, "Fragata - 3 posições");
                coluna = coluna_carregar_navios(linha, coluna, nome_jogador, contador, "Fragata - 3 posições");
            }
            if (nome_jogador == "Computador")
            {
                linha = auto_gerar_linha_coluna(opcao_nivel_jogo);
                coluna = auto_gerar_linha_coluna(opcao_nivel_jogo);
            }
        }

        if (contador >= 4 && contador < 6) // para marcação das corvetas
        {
            if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
            {
                linha = linha_carregar_navios(linha, coluna, nome_jogador, contador, "Corveta - 2 posições");
                coluna = coluna_carregar_navios(linha, coluna, nome_jogador, contador, "Corveta - 2 posições");
            }
            if (nome_jogador == "Computador")
            {
                linha = auto_gerar_linha_coluna(opcao_nivel_jogo);
                coluna = auto_gerar_linha_coluna(opcao_nivel_jogo);
            }
        }

        if (contador >= 6 && contador < 8) // para marcação dos submarinos
        {
            if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
            {
                linha = linha_carregar_navios(linha, coluna, nome_jogador, contador, "Submarino - 1 posição");
                coluna = coluna_carregar_navios(linha, coluna, nome_jogador, contador, "Submarino - 1 posição");
            }
            if (nome_jogador == "Computador")
            {
                linha = auto_gerar_linha_coluna(opcao_nivel_jogo);
                coluna = auto_gerar_linha_coluna(opcao_nivel_jogo);
            }
        }

        if (contador < 2) // para determinar o tamanho do navio
        {
            tamanho_navio = 4;
        }

        if (contador >= 2 && contador < 4)
        {
            tamanho_navio = 3;
        }

        if (contador >= 4 && contador < 6)
        {
            tamanho_navio = 2;
        }

        if (contador >= 6 && contador < 8)
        {
            tamanho_navio = 1;
        }


        if (validar_marcacao_navios(linha, coluna, tamanho_navio, matriz_jogo, orientacao) > 0) // valida se já há algum navio nas posições que se pretendem marcar
        {
            if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Já existe um navio numa das posições que pretende marcar, ou o navio irá sair fora do campo de batalha, escolha outra posição.");
                Console.ResetColor();
            }
        }

        else
        {
            switch (contador + 1)
            {
                case 1:
                    registo_navios(linha, coluna, 4, 0, 0, matriz_jogo, matriz_memoria, orientacao); // regista as posições dos porta-aviões na tabela de jogo e guarda as posições em memória
                    break;

                case 2:
                    registo_navios(linha, coluna, 4, 0, 4, matriz_jogo, matriz_memoria, orientacao); // regista as posições dos porta-aviões na tabela de jogo e guarda as posições em memória
                    break;

                case 3:
                    registo_navios(linha, coluna, 3, 2, 0, matriz_jogo, matriz_memoria, orientacao); // regista as posições das fragatas na tabela de jogo e guarda as posições em memória
                    break;

                case 4:
                    registo_navios(linha, coluna, 3, 2, 3, matriz_jogo, matriz_memoria, orientacao); // regista as posições das fragatas na tabela de jogo e guarda as posições em memória
                    break;

                case 5:
                    registo_navios(linha, coluna, 2, 4, 0, matriz_jogo, matriz_memoria, orientacao); // regista as posições das corvetas na tabela de jogo e guarda as posições em memória
                    break;

                case 6:
                    registo_navios(linha, coluna, 2, 4, 2, matriz_jogo, matriz_memoria, orientacao); // regista as posições das corvetas na tabela de jogo e guarda as posições em memória
                    break;

                case 7:
                    registo_navios(linha, coluna, 1, 6, 0, matriz_jogo, matriz_memoria, orientacao); // regista as posições dos submarinos na tabela de jogo e guarda as posições em memória
                    break;

                case 8:
                    registo_navios(linha, coluna, 1, 6, 1, matriz_jogo, matriz_memoria, orientacao); // regista as posições dos submarinos na tabela de jogo e guarda as posições em memória
                    break;
            }
            Console.Clear();

            if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
            {
                mostrar_matriz(matriz_jogo, matriz_tiros, nome_jogador, opcao_nivel_jogo); // mostra matrizes de jogo
                Console.WriteLine();
            }
            contador++;
        }
    }
    if (nome_jogador != "Computador") //Para só mostrar quando o jogador não é  computador
    {
        Thread.Sleep(4000);
    }
}


// função para carregamento inicial da matriz
static void carregar_matriz_inicio(string[,] matriz_jogo, string[,] matriz_tiros)
{
    for (int i = 0; i < matriz_jogo.GetLength(0); i++)
    {
        for (int j = 0; j < matriz_jogo.GetLength(1); j++)
        {
            matriz_jogo[i, j] = "A ";
        }
        for (int j = 0; j < matriz_tiros.GetLength(1); j++)
        {
            matriz_tiros[i, j] = "A ";
        }
    }
}

// função para mostrar matrizes de jogo 
static void mostrar_matriz(string[,] matriz_jogo, string[,] matriz_tiros, string nome_jogador, int opcao_nivel_jogo)
{


    if (opcao_nivel_jogo == 1)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"                        JOGO JOGADOR:");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($" {nome_jogador}");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"            O meu jogo                   Os meus tiros");
        Console.WriteLine();
        Console.Write($"        0 1 2 3 4 5 6 7 8 9           0 1 2 3 4 5 6 7 8 9");
    }
    if (opcao_nivel_jogo == 2)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"                                                JOGO JOGADOR:");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($" {nome_jogador}");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"                        O meu jogo                                           Os meus tiros");
        Console.WriteLine();
        Console.Write($"         0  1  2  3  4  5  6  7  8  9 10 11 12 13 14            0  1  2  3  4  5  6  7  8  9 10 11 12 13 14");
    }
    if (opcao_nivel_jogo == 3)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"                                                              JOGO JOGADOR:");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($" {nome_jogador}");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"                                O meu jogo                                                          Os meus tiros");
        Console.WriteLine();
        Console.Write($"         0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19            0  1  2  3  4  5  6  7  8  9 10 11 12 13 14 15 16 17 18 19");
    }
    if (opcao_nivel_jogo == 1)
    {
        matriz_padleft(2, 0, matriz_jogo, matriz_tiros);
    }
    else
    {
        matriz_padleft(2, 3, matriz_jogo, matriz_tiros);
    }
}

// Criação de classe para lista de highscores

public class Highscores
{
    public void adicionar_pontuacao(string _nome_jogador, int _pontuacao)
    {
        Nome = _nome_jogador;
        Pontuacao = _pontuacao;
    }
    public string Nome { get; set; }
    public int Pontuacao { get; set; }
}
