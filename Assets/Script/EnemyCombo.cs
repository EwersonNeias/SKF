using UnityEngine;
using System.Collections;

public class EnemyCombo : MonoBehaviour
{
    public Combo[] combos;
    public Attack attack;
    public float attackRange = 1.5f;
    public float attackInterval = 5f;
    public float timeBetweenAttacks = 0.5f; // Tempo entre cada animação de ataque

    private Animator anim;
    private Transform player;
    private bool isAttacking = false;
    private int currentComboIndex = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    private void Update()
    {
        // Apenas executa o combo se o player existir e estiver no alcance
        if (!isAttacking && player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            ExecuteCombo();
        }
    }

    public void ExecuteCombo()
    {
        if (combos.Length > 0)
        {
            // Seleciona um combo aleatório
            Combo combo = combos[Random.Range(0, combos.Length)];
            StartCoroutine(PerformCombo(combo));
        }
    }

    private IEnumerator PerformCombo(Combo combo)
    {
        isAttacking = true;
        foreach (Hit hit in combo.hits)
        {
            PlayHit(hit);
            yield return new WaitForSeconds(timeBetweenAttacks); // Delay entre cada animação de ataque
        }
        yield return new WaitForSeconds(attackInterval); // Delay entre combos
        currentComboIndex = 0; // Resetar o índice do combo atual
        isAttacking = false;
    }

    private void PlayHit(Hit hit)
    {
        // Certifica que o objeto 'attack' está atribuído antes de usá-lo
        if (attack != null)
        {
            attack.SetAttack(hit);
        }
        else
        {
            Debug.LogWarning("Attack reference is missing!");
        }

        // Toca a animação do hit
        if (anim != null)
        {
            anim.Play(hit.animation);
        }
        else
        {
            Debug.LogWarning("Animator is missing!");
        }

        // Realiza o ataque
        PerformAttack(hit);
    }

    private void PerformAttack(Hit hit)
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing.");
            return;
        }

        LifeSystem lifeSystem = player.GetComponent<LifeSystem>();
        if (lifeSystem == null)
        {
            Debug.LogWarning("LifeSystem component not found on the Player.");
            return;
        }

        if (lifeSystem.currentHealth > 0)
        {
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                lifeSystem.TakeDamage(hit.damage);
                if (hit.hitSound != null)
                {
                    if (attack != null && attack.audioPlayer != null)
                    {
                        attack.audioPlayer.PlaySound(hit.hitSound);
                    }
                    else
                    {
                        Debug.LogWarning("Attack or its audioPlayer reference is missing!");
                    }
                }
            }
        }
    }
}
