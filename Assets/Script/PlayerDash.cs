using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 24f;       // Velocidade do dash
    public float dashTime = 0.2f;         // Duração do dash (ajuste conforme necessário)
    public float dashCooldown = 1f;       // Tempo de recarga entre dashes
    private bool canDash = true;
    private float originalGravity;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D playerCollider;

    void Start()
    {
        originalGravity = rb.gravityScale; // Guarda o valor original da gravidade
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3") && canDash)
            StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        canDash = false;
        animator.SetBool("isDashing", true); 
        playerCollider.enabled = false;     
        rb.gravityScale = 0;               

       
        float dashDirection = transform.localScale.x;
        if (dashDirection == 0) dashDirection = 1;

      
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashTime);

       
        animator.SetBool("isDashing", false);
        playerCollider.enabled = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
