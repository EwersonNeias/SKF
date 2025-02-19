using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private ParticleSystem particulas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);

        // Verifica se o objeto que entrou na zona de destruição tem a tag "Player" ou "Enemy"
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            string tag = collision.gameObject.tag;
            Debug.Log($"{tag} entered the kill zone.");

            // Destroi o objeto que tem a tag "Player" ou "Enemy"
            var entity = collision.gameObject;
            if (entity != null)
            {
                // Obtém a posição atual do jogador ou inimigo
                Vector3 entityPosition = entity.transform.position;

                // Instancia as partículas na posição do jogador ou inimigo
                Instantiate(particulas, entityPosition, Quaternion.identity);

                // Destroi o jogador ou inimigo
                Destroy(entity);
                Debug.Log($"{tag} destroyed.");
            }
        }
    }
}
