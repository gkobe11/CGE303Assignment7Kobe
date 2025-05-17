using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    private DisplayBar healthBar;
    public int damage = 10; // Damage dealt to player on contact

    private void Start()
    {
        healthBar = GetComponentInChildren<DisplayBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar (DisplayBar script) not found.");
            return;
        }

        healthBar.SetMaxValue(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.setValue(health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); // Get PlayerHealth script from player object
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth script not found on Player object.");
                return;
            }

            playerHealth.TakeDamage(damage); // Deal damage to player
            playerHealth.Knockback(transform.position); // Apply knockback effect
        }
    }
}
