using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 7.71f;
    public float jumpForce = 14.69f;

    [Header("Referências")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator; // Para animações

    private bool isFacingRight = true;

    void Update()
    {
        // Captura o input horizontal
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Aplica o movimento horizontal
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Atualiza a animação de movimentação
        animator.SetBool("Run", horizontal != 0);

        // Verifica se o jogador está no chão para permitir o pulo
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // Inverte a direção do jogador conforme o movimento
        if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            Flip();
        }
    }

    // Realiza o pulo aplicando uma força vertical
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Verifica se o jogador está tocando o chão
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Inverte a escala do personagem para mudar a direção do olhar
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
