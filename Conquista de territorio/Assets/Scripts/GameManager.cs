using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject blocoPrefab;
    [SerializeField] GameObject jogador1;
    [SerializeField] GameObject jogador2;
    [SerializeField] int linhas = 5;
    [SerializeField] int colunas = 5;
    [SerializeField] float espacamento = 1.1f;
    private Bloco[,] grade;
    private int territoriosConquistados;
    public static GameManager instance;
    void Awake()
    {
        instance = this;
        grade = new Bloco[linhas, colunas];
        // Inscrição no evento do bloco
        Bloco.OnBlocoConquistado += AtualizarTerritorios;
        CriarGrade();
    }
    void CriarGrade()
    {
        for (int linha = 0; linha < linhas; linha++)
        {
            for (int coluna = 0; coluna < colunas; coluna++)
            {
                Vector2 posicao = new Vector2(coluna * espacamento, linha * espacamento);
                GameObject novoBloco = Instantiate(blocoPrefab, posicao, Quaternion.identity);
                grade[linha, coluna] = novoBloco.GetComponent<Bloco>();
            }
        }
        Vector2 posicaoInicialJogador1 = new Vector2((colunas - 1) * espacamento / 2f - espacamento, (linhas - 1) * espacamento / 2f);
        Vector2 posicaoInicialJogador2 = new Vector2((colunas - 1) * espacamento / 2f + espacamento, (linhas - 1) * espacamento / 2f);
        Camera.main.transform.position = new Vector3((colunas - 1) * espacamento / 2f, (linhas - 1) * espacamento / 2f, -10);
        Camera.main.orthographicSize = linhas / 2f * espacamento;
        Instantiate(jogador1, posicaoInicialJogador1, Quaternion.identity);
        Instantiate(jogador2, posicaoInicialJogador2, Quaternion.identity);
    }
    // Método chamado quando um bloco é conquistado
    void AtualizarTerritorios(Bloco bloco, int jogadorDono)
    {
        territoriosConquistados++;
        // Verifica se todos os blocos foram conquistados
        if (territoriosConquistados == grade.Length)
        {
            int jogador1Territorios = 0;
            int jogador2Territorios = 0;
            foreach (Bloco b in grade)
            {
                if (b.PegarJogadorDono() == 1)
                    jogador1Territorios++;
                else if (b.PegarJogadorDono() == 2)
                    jogador2Territorios++;
            }
            FimDeJogo(jogador1Territorios, jogador2Territorios);
        }
    }
    void FimDeJogo(int territoriosJogador1, int territoriosJogador2)
    {
        string vencedor;
        if (territoriosJogador1 > territoriosJogador2)
        {
            vencedor = "Jogador 1 venceu!";
        }
        else if (territoriosJogador2 > territoriosJogador1)
        {
            vencedor = "Jogador 2 venceu!";
        }
        else
        {
            vencedor = "Empate!";
        }
        Debug.Log("Fim do jogo! " + vencedor);
    }
    void OnDestroy()
    {
        // Remove a inscrição do evento ao destruir o GameManager
        Bloco.OnBlocoConquistado -= AtualizarTerritorios;
    }
}