using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 1.5f; // Alcance do ataque
    public float attackInterval = 5f; // Intervalo de 5 segundos entre ataques

    private Transform playerTransform;
    private LifeSystem playerLifeSystem;
    private bool isAttacking = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerLifeSystem = player.GetComponent<LifeSystem>();
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }

        InvokeRepeating(nameof(PerformAttack), 0f, attackInterval);
    }

    private void PerformAttack()
    {
        if (!isAttacking && playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            isAttacking = true;

            // Realiza a verificação de colisão e aplica dano apenas uma vez para cada ataque
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    LifeSystem playerLifeSystem = collider.GetComponent<LifeSystem>();
                    if (playerLifeSystem != null)
                    {
                        Debug.Log("Enemy attacking player: " + collider.gameObject.name + " with damage: " + damage);
                        playerLifeSystem.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.LogWarning("LifeSystem component not found on player: " + collider.gameObject.name);
                    }
                }
            }

            isAttacking = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
