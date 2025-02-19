using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool isPlayerInside = false;

    // Tempo de duração dos controles bagunçados
    public float confusedDuration = 3f;

    // Referência ao componente de controle do jogador
    private PlayerMovement playerController;

    void Start()
    {
        // Destruir o objeto após 10 segundos
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerController = collision.gameObject.GetComponent<PlayerMovement>();

            // Bagunça os controles do jogador
            if (playerController != null)
            {
                playerController.ConfuseControls();
                Invoke("ResetControls", confusedDuration);
            }

            // Destroi a armadilha
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void ResetControls()
    {
        // Reseta os controles do jogador após o tempo de confusão
        if (playerController != null && isPlayerInside)
        {
            playerController.ResetControls();
        }
    }
}
