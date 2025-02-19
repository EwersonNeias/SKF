using UnityEngine;

public class Attack : MonoBehaviour
{
    public AudioPlayer audioPlayer;
    public PlayerMovement playerMovement;

    private int damage;
    private bool slowDown;
    private AudioClip hitSound;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on GameObject: " + gameObject.name);
        }
    }

    public void PerformAttack()
    {
        // Lógica para executar um ataque
        Debug.Log("Attack: Executando ataque");
        // Adicione aqui a lógica de ataque
    }

    public void SetAttack(Hit hit)
    {
        damage = hit.damage;
        slowDown = hit.slowDown;
        hitSound = hit.hitSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LifeSystem enemyLifeSystem = collision.gameObject.GetComponent<LifeSystem>();
            if (enemyLifeSystem != null)
            {
                Debug.Log("Attacking enemy: " + collision.gameObject.name + " with damage: " + damage); // Log para depuração
                enemyLifeSystem.TakeDamage(damage);
                if (audioPlayer != null)
                {
                    audioPlayer.PlaySound(hitSound);
                }
                if (slowDown)
                {
                    SlowDown.instance.SetSlowDown();
                }
                ComboManager.instance.SetCombo();
                if (playerMovement != null)
                {
                    playerMovement.SetCanMove(false);
                }
            }
            else
            {
                Debug.LogWarning("LifeSystem component not found on enemy: " + collision.gameObject.name);
            }
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    // Método para o ataque quando a seta para cima é pressionada
    public void AttackUp()
    {
        animator.SetTrigger("AttackUp");
    }

    // Método para o ataque quando a seta para baixo é pressionada
    public void AttackDown()
    {
        animator.SetTrigger("AttackDown");
    }

    // Método para o ataque básico
    public void BasicAttack()
    {
        animator.SetTrigger("AttackBasic");
    }

    // Método para o ataque quando o jogador pula
    public void JumpAttack()
    {
        animator.SetTrigger("JumpAttack");
    }

    // Método para o ataque quando o jogador está caindo
    public void FallingAttack()
    {
        animator.SetTrigger("FallingAttack");
    }
}
