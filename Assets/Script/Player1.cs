using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 7.71f;
    private float jumpingPower = 14.69f;
    private bool isFacingRight = true;
    private bool canMove = true;

    [SerializeField] private Animator animator;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    private bool doubleJump;
    private bool canDoubleJump;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem particulas;

    private int playerLayer;
    private int platformLayer;

    private bool isConfused = false;
    private float confusionDuration = 3f;
    private float originalSpeed;
    private Coroutine speedBoostCoroutine;

    private float fastFallSpeed = 59.6f;

    private bool hasCollectedItem = false;
    public GameObject itemToEquip;

    public CooldownBar cooldownBar;
    public float trapCooldownDuration = 5f;
    public float speedCooldownDuration = 5f;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer = LayerMask.NameToLayer("Platform");
        originalSpeed = speed;
        animator.SetBool("isDashing", false);

        gameObject.layer = LayerMask.NameToLayer("IgnorePlayerCollision");
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        animator.SetBool("isJumping", true);
    }

    public void ApplyItemEffect(Sprite newSprite)
    {
        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null && newSprite != null)
        {
            playerSpriteRenderer.sprite = newSprite;
        }
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Eixo horizontal do joystick
        bool isMovingHorizontally = Mathf.Abs(horizontal) > 0;

        animator.SetBool("isGrounded", isGrounded());

        if (isGrounded())
        {
            canDoubleJump = true;
            doubleJump = false;
            animator.SetBool("isDoubleJumping", false);
        }

        if (Input.GetButtonDown("Jump")) // Botão de pular no joystick
        {
            if (isGrounded())
                Jump();
            else if (isWallSliding)
                WallJump();
            else if (canDoubleJump)
                DoubleJump();
        }

        WallSlide();

        if (!isWallJumping)
            Flip();

        animator.SetBool("Run", isMovingHorizontally);
        animator.SetBool("isWallSliding", isWallSliding);

        if (isConfused)
        {
            // Implement confusion control logic if necessary
        }

        if (isMovingHorizontally)
            particulas.Play();
        else
            particulas.Stop();

        if (hasCollectedItem && Input.GetKeyDown(KeyCode.JoystickButton0)) // Botão A no joystick
        {
            EquipItem(itemToEquip);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && !isGrounded() && rb.velocity.y < 0)
        {
            rb.velocity += Vector2.down * fastFallSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (!isGrounded() && rb.velocity.y < 0 && !isWallSliding)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (wallSlidingSpeed - 1) * Time.fixedDeltaTime;
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (isWalled() && !isGrounded() && rb.velocity.y < 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = true;
            wallJumpingDirection = isFacingRight ? 1 : -1;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        doubleJump = true;
        canDoubleJump = false;
        animator.SetBool("isJumping", false);
        animator.SetBool("isDoubleJumping", true);
    }

    private void Flip()
    {
        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y <= 0)
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            if (cooldownBar != null && cooldownBar.IsCooldownComplete())
            {
                cooldownBar.StartCooldown(trapCooldownDuration);
            }
        }
        else if (collision.CompareTag("Speed"))
        {
            if (cooldownBar != null && cooldownBar.IsCooldownComplete())
            {
                cooldownBar.StartCooldown(speedCooldownDuration);
            }
        }
        else if (collision.CompareTag("Item"))
        {
            CollectItem(collision.gameObject);
        }
    }

    private void CollectItem(GameObject item)
    {
        hasCollectedItem = true;
        itemToEquip = item;
        item.SetActive(false);
    }

    private void EquipItem(GameObject item)
    {
        item.SetActive(true);
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        Destroy(item.GetComponent<Collider2D>());
        hasCollectedItem = false;
    }

    public void ApplySpeedBoost(float increase, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoost(increase, duration));
    }

    private IEnumerator SpeedBoost(float increase, float duration)
    {
        speed += increase;
        yield return new WaitForSeconds(duration);
        speed -= increase;
        speedBoostCoroutine = null;
    }

    public void ConfuseControls()
    {
        isConfused = true;
        speed *= -1;
        StartCoroutine(ResetConfusion());
    }

    private IEnumerator ResetConfusion()
    {
        yield return new WaitForSeconds(confusionDuration);
        speed = Mathf.Abs(speed);
        isConfused = false;
    }

    public void ResetControls()
    {
        // Implement logic to reset player controls if necessary
    }
}