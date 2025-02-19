using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float jumpPower = 14.69f;
    public float attackRange = 1.5f;
    public float edgeDetectionDistance = 1.0f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public float platformCheckDistance = 2.0f; // Dist�ncia para detectar plataformas � frente
    public EnemyCombo enemyCombo;

    private Transform player;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isJumping = false; // Adiciona um estado para controlar o pulo
    private bool canDoubleJump = true; // Controle para permitir double jump
    private bool isAttacking = false; // Controle para verificar se o inimigo est� atacando

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private Transform wallSlideCheck;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        bool isTouchingWall = Physics2D.Raycast(wallSlideCheck.position, isFacingRight ? Vector2.right : Vector2.left, groundCheckDistance, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verificar se o jogador est� dentro do alcance de ataque
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking) // Se n�o estiver atacando, iniciar ataque
            {
                AttackPlayer();
            }
            enemyCombo.ExecuteCombo();
        }
        else
        {
            MoveTowardsPlayer();
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isJumping", isJumping); // Alterado para refletir o estado real de pulo

        // Atualizar o estado de WallSliding
        isWallSliding = isTouchingWall && !isGrounded;
        animator.SetBool("isWallSliding", isWallSliding);

        // Checar bordas e pular se necess�rio
        if (!IsEdgeAhead() && !isGrounded)
        {
            Jump();
        }
    }

    void MoveTowardsPlayer()
    {
        if (!isJumping && isGrounded) // Evitar mover enquanto est� pulando
        {
            Vector2 targetPosition = new Vector2(player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            animator.SetBool("Run", true);

            // Flip the enemy to face the player
            if ((targetPosition.x > rb.position.x && !isFacingRight) || (targetPosition.x < rb.position.x && isFacingRight))
            {
                Flip();
            }
        }
    }

    void AttackPlayer()
    {
        isAttacking = true; // Indicar que o inimigo est� atacando
        // Parar de se mover
        rb.velocity = Vector2.zero;
        animator.SetBool("Run", false);

        // Iniciar anima��o de ataque
        animator.SetTrigger("Attack");

        // Adicionar l�gica de dano ao jogador
        Debug.Log("Enemy attacking player!");
        // player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    void OnAttackEnd() // Este m�todo ser� chamado no final da anima��o de ataque do inimigo
    {
        isAttacking = false; // Indicar que o inimigo terminou de atacar
    }

    void Jump()
    {
        if (!isJumping && !isGrounded && canDoubleJump) // Double jump apenas se n�o estiver pulando, nem no ch�o, e puder dar double jump
        {
            Debug.Log("Jumping!");

            // Calcular dire��o para frente (dire��o do jogador)
            Vector2 jumpDirection = (player.position - transform.position).normalized;

            // Calcular for�a de salto com base na dire��o calculada e na pot�ncia de salto
            Vector2 jumpForce = jumpDirection * jumpPower;

            // Aplicar a for�a de salto ao Rigidbody2D
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            isJumping = true;
            canDoubleJump = false; // Desabilitar double jump ap�s o primeiro pulo
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y <= 0)
        {
            animator.SetBool("isJumping", false);
            canDoubleJump = true; // Habilitar double jump ao tocar no ch�o
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    bool IsEdgeAhead()
    {
        return Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeDetectionDistance, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        // Desenhar o alcance de detec��o no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Desenhar o alcance de detec��o de borda no editor
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeDetectionDistance);

        // Desenhar o alcance de detec��o de plataformas � frente
        Gizmos.color = Color.green;
        Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * platformCheckDistance);

        // Desenhar o alcance de detec��o de paredes para slide no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallSlideCheck.position, wallSlideCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * groundCheckDistance);
    }
}
