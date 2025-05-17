using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public DisplayBar healthBar; // Reference to the health bar UI
    private Rigidbody2D rb;
    public float knockbackForce = 10f; // Force applied when contact with enemy
    public GameObject playerDeathEffect; // Prefab that respawns player after death
    public static bool hitRecently = false; // Flag to prevent multiple hits in a short time
    public float hitRecoveryTime = 0.2f; // Time in seconds to recover from hit

    //sound effects
    private AudioSource playerAudio; // Referen e to the player's audio source
    public AudioClip playerHitSound; // Sound played when player is hit

    //Animation
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // set rigid body reference

        playerAudio = GetComponent<AudioSource>(); // set audio source reference

        animator = GetComponent<Animator>(); // set animator reference

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on PlayerHealth object.");
        }

        healthBar.SetMaxValue(health); // Initialize health bar with max health
        hitRecently = false; // Initialize hitRecently to false
    }

    // Knocks the player back when hit by an enemy
    public void Knockback(Vector3 enemyPosition)
    {
        if (hitRecently)
        {
            return;
        }

        hitRecently = true; // Set the flag to prevent further hits
        if (gameObject.activeSelf)
        {
            StartCoroutine(RecoverFromHit()); // Start the recovery coroutine
        }
        Vector2 direction = transform.position - enemyPosition; // Calculate direction away from enemy
        direction.Normalize(); // Normalize the direction vector
        direction.y = direction.y * 0.5f + 0.5f; // Add a slight upward force
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse); // Apply knockback force
    }

    IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(hitRecoveryTime); // Wait for recovery time
        hitRecently = false; // Reset the hitRecently flag
        animator.SetBool("hit", false); // Reset hit animation
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.setValue(health); // Update health bar UI

        // TODO: add animation

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // play hit sound
            playerAudio.PlayOneShot(playerHitSound);

            // Trigger hit animation
            animator.SetBool("hit", true);
        }
    }

    public void Die()
    {
        ScoreManager.gameOver = true; // Set game over state

        // Death sound played in PlaySound script

        Instantiate(playerDeathEffect, transform.position, Quaternion.identity); // Spawn death effect
        Destroy(playerDeathEffect, 2); //destroy death effect after 2 seconds

        gameObject.SetActive(false); // Deactivate player object
    }
}
