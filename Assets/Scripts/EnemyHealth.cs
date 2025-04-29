using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Animator animator;
    public bool isDead = false;
    public int addHeal = 20;
    public GameObject player;
    public AudioSource audioSource;           // Assign in Inspector
    public AudioClip hurtClip;
    public ParticleSystem bloodParticle;
    public float bloodPosY;
    public Score score;
    public int points = 10;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        score = FindObjectOfType<Score>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        bloodParticle.transform.position = transform.position + new Vector3(0, bloodPosY, 0);
        bloodParticle.Play();

        if (currentHealth <= 0)
        {
            StartCoroutine(StopBloodEffectAfterTime(1f));
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(StopBloodEffectAfterTime(0.5f));  // Stop after 2 seconds

            audioSource.PlayOneShot(hurtClip, 2.0f);
        }
    }
    private IEnumerator StopBloodEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        bloodParticle.Stop();  // Stop the blood effect after 'duration' time
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.AddHealth(addHeal);
        }
        else
        {
            Debug.Log("PlayerHealth script not found on player or its children!");
        }
        Destroy(gameObject);
        score.AddScore(points);
        
    }
}
