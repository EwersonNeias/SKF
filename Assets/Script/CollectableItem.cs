using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Sprite newSprite; // Nova sprite que ser� aplicada ao jogador

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider)
        {
            // Se o jogador colidir com o item, coleta o item e aplica o efeito
            CollectItem(collision.gameObject);
        }
    }

    private void CollectItem(GameObject player)
    {
        // Aplica a nova apar�ncia ao jogador
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null && newSprite != null)
        {
            playerSpriteRenderer.sprite = newSprite;
        }

        // Destroi o objeto do colet�vel ap�s a coleta
        Destroy(gameObject);
    }
}
