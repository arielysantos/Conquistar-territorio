using UnityEngine;
using static UnityEditor.Progress;

public class Bloco : MonoBehaviour
{
    private bool conquistado = false; // O bloco foi conquistado?
    private SpriteRenderer spriteRenderer;
    private int jogadorDono;
    // Delegate para notificar quando um bloco � conquistado
    public delegate void BlocoConquistadoHandler(Bloco bloco, int jogadorDono);
    public static event BlocoConquistadoHandler OnBlocoConquistado;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarCor(Color.white);
    }
    // M�todo para alterar o estado de conquista do bloco com a cor do jogador
    public void AlterarConquista(bool jogador1, Color corDoJogador)
    {
        if (conquistado)
        {
            Debug.LogWarning("Bloco j� foi conquistado.");
            return;
        }
        conquistado = true;
        AtualizarCor(corDoJogador);
        jogadorDono = jogador1 ? 1 : 2;
        GameManager.instance.ConquistarTerritorio();
        // Dispara o evento de conquista
        OnBlocoConquistado?.Invoke(this, jogadorDono);
    }
    // M�todo para verificar se o bloco foi conquistado
    public bool PegarConquistado()
    {
        return conquistado;
    }
    public int PegarJogadorDono()
    {
        return jogadorDono;
    }
    // M�todo que muda a cor do bloco dependendo se ele foi conquistado ou n�o
    private void AtualizarCor(Color novaCor)
    {
        spriteRenderer.color = novaCor;
    }
    // Sobrescrevendo OnMouseDown para detectar clique no bloco
    private void OnMouseDown()
    {
        if (!conquistado)
        {
            Debug.Log("Bloco clicado! Pronto para ser conquistado.");
            // Exemplo: AlterarConquista pode ser chamado por outro script para tratar l�gica do jogo.
        }
        else
        {
            Debug.Log("Este bloco j� foi conquistado.");
        }
    }
}
tem menu de contexto