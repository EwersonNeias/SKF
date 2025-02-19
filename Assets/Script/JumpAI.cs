using UnityEngine;

public class JumpAI : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float jumpPower = 14.69f;
    public float minDistanceToPlayer = 2.0f;
    public float jumpInterval = 1.0f; // Intervalo de tempo entre os pulos
    public LayerMask groundLayer;

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool canJump = true;
    private float jumpTimer = 0.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verifica se o jogador est� dentro da dist�ncia m�nima
        if (Vector2.Distance(transform.position, player.position) <= minDistanceToPlayer)
        {
            MoveTowardsPlayer();

            // Se o inimigo puder pular e estiver no ch�o, ou se o tempo de espera entre os pulos acabou, ele pula
            if ((isJumping && canJump) || (isJumping && Time.time >= jumpTimer))
            {
                Jump();
                jumpTimer = Time.time + jumpInterval;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcula a dire��o para o jogador
        Vector2 direction = (player.position - transform.position).normalized;

        // Move o inimigo na dire��o do jogador
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Flip o inimigo para encarar o jogador
        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    private void Jump()
    {
        // Aplica a for�a de pulo
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        canJump = false; // Impede que o inimigo pule novamente at� tocar no ch�o
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reseta o estado de pulo quando toca no ch�o
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            canJump = true;
        }
    }

    private void Flip()
    {
        // Inverte a dire��o do inimigo
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
