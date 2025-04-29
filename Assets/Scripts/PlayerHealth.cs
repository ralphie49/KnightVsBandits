using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Slider pHealth;
    public Animator animator;
    public bool isDead = false;

    public AudioSource audioSource;           // Assign in Inspector
    public AudioClip hurtClip;
    public AudioClip dieClip;
    public ParticleSystem bloodParticle;
    public float bloodPosY;

    void Start()
    {
        currentHealth = maxHealth;
        pHealth.maxValue = maxHealth;
        pHealth.value = currentHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        pHealth.value = currentHealth;


        bloodParticle.transform.position = transform.position + new Vector3(0, bloodPosY, 0);
        bloodParticle.Play();

        if (currentHealth <= 0)
        {
            StartCoroutine(StopBloodEffectAfterTime(1f));  // Stop after 2 seconds
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
    public void AddHealth(int addHeal)
    {
        currentHealth += addHeal;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        pHealth.value = currentHealth;
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        audioSource.PlayOneShot(dieClip, 2.0f);
        Debug.Log("Game Over");

        StartCoroutine(DelayGameOver(1f)); // 2f is the delay in seconds before pausing the game (you can adjust it)
        

    }

    // Coroutine to delay the game over and freeze the game
    IEnumerator DelayGameOver(float delayTime)
    {
        // Wait for the animation duration (or any other time you want to wait)
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("GameOver");
        // Optionally destroy the player after some time, if needed



    }
}