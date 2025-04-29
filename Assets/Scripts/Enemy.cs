using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D enemyRb;
    public float stopDistance = 1.5f;
    public float chaseDistance = 10f;
    public float speed = 2f;
    public Animator animator;
    public Vector3 originalScale;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    public float attackRange = 1.0f;
    public Vector2 attackBoxSize = new Vector2(2f, 2f); // Tweak as needed
    public LayerMask playerLayer;
    public int damage=20;
    public AudioSource enemyAudio;           // Assign in Inspector
    public AudioClip swingClip;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        animator = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        enemyAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Flip enemy to face player
        if (direction.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        if (distance < chaseDistance && distance > stopDistance)
        {
            Vector2 newPosition = Vector2.MoveTowards(enemyRb.position, player.transform.position, speed * Time.deltaTime);
            enemyRb.MovePosition(newPosition); 
            animator.SetInteger("AnimState", 2); // Walking
        }
        else if (distance <= stopDistance)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
                TryDamagePlayer();
                enemyAudio.PlayOneShot(swingClip, 2.0f);
                Debug.Log("Distance:" + distance);
            }
            else
            {
                animator.SetInteger("AnimState", 1); // Idle
            }
        }
    }
   
    void TryDamagePlayer()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1; // Facing check
        float offsetX = direction * 1.1f; // In front of the enemy
        float offsetY = 2f; // Slightly above the feet

        Vector2 attackCenter = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0, playerLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player")) // Ensure only player is hit
            {
                PlayerHealth pHealth = hit.GetComponent<PlayerHealth>();
                if (pHealth != null)
                {
                    pHealth.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1;
        float offsetX = direction * 1.1f;
        float offsetY = 2f;

        Vector2 attackCenter = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCenter, attackBoxSize);
    }

}

