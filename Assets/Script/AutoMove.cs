using System.Collections;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public float speed = 5f;  // Velocidade de movimento do objeto
    public float lifetime = 10f;  // Tempo de vida do objeto em segundos
    public float pushForce = 10f;  // Força do empurrão aplicado ao jogador

    private void Start()
    {
        // Inicia a coroutine que destruirá o objeto após o tempo de vida
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private void Update()
    {
        // Move o objeto na direção horizontal (esquerda)
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        // Aguarda pelo tempo especificado
        yield return new WaitForSeconds(time);

        // Destroi o objeto
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
