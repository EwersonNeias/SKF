using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject bloodEffect;
    public GameObject deathParticleEffect;

    public Color damageColor = Color.red;
    public float damageFlashDuration = 0.1f;
    public float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    public LifeBar lifeBar;
    public List<AudioClip> damageSounds;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private AudioSource audioSource;
    private Animator animator;
    private bool isGrounded = true;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer is missing in the prefab children.");
        }

        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the player or its children.");
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player or its children.");
        }

        if (lifeBar != null)
        {
            lifeBar.SetMaxHealth(maxHealth);
            lifeBar.SetHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("LifeBar reference is missing in LifeSystem script");
        }
    }

    void Update()
    {
        // Atualize o estado isGrounded conforme necessário
        // Isso pode depender do seu próprio sistema de verificação se o player está no chão
        // isGrounded = seuMetodoDeVerificacaoSeEstaNoChao();
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            StartCoroutine(ApplyDamage(damage));
        }
    }

    private IEnumerator ApplyDamage(int damage)
    {
        canTakeDamage = false;

        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(gameObject.name + " took damage. Current Health: " + currentHealth);

        if (lifeBar != null)
        {
            lifeBar.SetHealth(currentHealth, maxHealth);
        }

        if (audioSource != null && damageSounds.Count > 0)
        {
            int randomIndex = Random.Range(0, damageSounds.Count);
            audioSource.PlayOneShot(damageSounds[randomIndex]);
        }

        if (animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetTrigger("isTakingDamage");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Espera o fim da animação de dano
            animator.ResetTrigger("isTakingDamage"); // Resetar o trigger para evitar loop
            animator.SetTrigger("Idle"); // Define o gatilho para retornar à animação Idle
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }

        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (lifeBar != null)
        {
            lifeBar.SetHealth(currentHealth, maxHealth);
        }
    }

    public void Die()
    {
        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(damageFlashDuration);
            spriteRenderer.color = originalColor;
        }
    }
}
