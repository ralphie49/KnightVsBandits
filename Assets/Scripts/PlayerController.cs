using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 10f;
    public Animator animator;
    private Vector3 originalScale;
    public Rigidbody2D playerRb;
    public Transform healthBar;
    private float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;
    public float attackRange = 1.0f;
    public Vector2 attackBoxSize = new Vector2(2f, 2f); // Tweak as needed
    public LayerMask enemyLayer;
    public bool isGrounded = true;
    public float gravityModifier;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    public AudioSource playerAudio;           // Assign in Inspector
    public AudioClip swingClip;               // Drag your swing sound
    public AudioClip jumpClip;
    public AudioClip blockClip;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

    }

    void FixedUpdate()
    {
        PlayerHealth pHealth = GetComponent<PlayerHealth>();
        if (!pHealth.isDead)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
            int isWalking = (Mathf.Abs(horizontalInput) > 0.01f ? 1 : 0);
            animator.SetInteger("AnimState", isWalking);
            animator.SetBool("Grounded", true);

            if (horizontalInput > 0)
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            else if (horizontalInput < 0)
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

            if (healthBar != null)
            {
                healthBar.localScale = new Vector3(Mathf.Abs(healthBar.localScale.x), healthBar.localScale.y, healthBar.localScale.z);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                animator.SetTrigger("Block");
                playerAudio.PlayOneShot(blockClip, 2.0f);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Roll");
                playerAudio.PlayOneShot(jumpClip, 2.0f);
            }
            isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (Input.GetKeyDown(KeyCode.J) && isTouchingGround)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                animator.SetTrigger("Jump");
                playerAudio.PlayOneShot(jumpClip, 2.0f);
                isGrounded = false;
                Debug.Log("Jumping with velocity.y = " + playerRb.linearVelocity.y);
                animator.SetFloat("AirSpeedY", playerRb.linearVelocity.y);
                animator.SetBool("Grounded", true);
            }

            isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            isGrounded = isTouchingGround;

            // Update Animator every frame
            animator.SetBool("Grounded", isGrounded);
            animator.SetFloat("AirSpeedY", playerRb.velocity.y);


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("Attack1");
                    lastAttackTime = Time.time;
                    DamageEnemy(20);
                    playerAudio.PlayOneShot(swingClip, 2.0f);
                }
                else animator.SetInteger("AnimState", 10); // Idle
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("Attack2");
                    lastAttackTime = Time.time;
                    DamageEnemy(30);
                    playerAudio.PlayOneShot(swingClip, 2.0f);
                }
                else animator.SetInteger("AnimState", 10); // Idle
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("Attack3");
                    lastAttackTime = Time.time;
                    DamageEnemy(40);
                    playerAudio.PlayOneShot(swingClip, 2.0f);
                }
                else animator.SetInteger("AnimState", 10); // Idle
            }
        }
    }
    void DamageEnemy(int damage)
    {
        float direction = transform.localScale.x > 0 ? 1 : -1; // Facing check
        float offsetX = direction * 1.1f; // In front of the enemy
        float offsetY = 0.3f; // Slightly above the feet

        Vector2 attackCenter = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth eHealth = hit.GetComponent<EnemyHealth>();
            if (eHealth != null)
            {
                eHealth.TakeDamage(damage);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnDrawGizmosSelected()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        float offsetX = direction * 1.1f;
        float offsetY = 0.3f;

        Vector2 attackCenter = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);
    }

}


