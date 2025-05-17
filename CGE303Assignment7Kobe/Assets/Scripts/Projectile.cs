using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public int damage = 20;
    public GameObject impactEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (hitInfo.gameObject.tag != "Player")
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
