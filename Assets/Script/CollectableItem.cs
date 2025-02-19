using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Sprite newSprite; // Nova sprite que será aplicada ao jogador

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
        // Aplica a nova aparência ao jogador
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null && newSprite != null)
        {
            playerSpriteRenderer.sprite = newSprite;
        }

        // Destroi o objeto do coletável após a coleta
        Destroy(gameObject);
    }
}
