using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMoveWalkingChase : MonoBehaviour
{
    public float chaseRange = 4f;
    public float enemyMovementSpeed = 1.5f;
    private Transform playerTransform;
    private Rigidbody2D rb; //rb of enemy
    private Animator animator; //animator of enemy
    private SpriteRenderer sr;  //sprite renderer of enemy

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //find player
        sr = GetComponent<SpriteRenderer>(); //get sprite renderer
    }

    void Update()
    {
        Vector2 playerDirection = playerTransform.position - transform.position; //get direction to player
        float distanceToPlayer = playerDirection.magnitude; //get distance to player
        
        if (distanceToPlayer <= chaseRange)
        {
            playerDirection.Normalize(); //normalize direction
            playerDirection.y = 0f; //ignore y direction
            FacePlayer(playerDirection); //face player
            if (IsGroundAhead())
            {
                MoveTowardsPlayer(playerDirection); //move towards player
            }
            else
            {
                StopMoving(); //stop moving if no ground ahead
            }
        }
        else
        {
            StopMoving();
        }
    }

    bool IsGroundAhead()
    {
        float groundCheckDistance = 2.0f; //distance to check for ground
        LayerMask groundLayer = LayerMask.GetMask("Ground"); //layer mask for ground

        Vector2 enemyFacingDirection = playerTransform.rotation.y == 0 ? Vector2.left : Vector2.right; //get enemy facing direction

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down + enemyFacingDirection, groundCheckDistance, groundLayer); //raycast to check for ground

        return hit.collider != null; //return true if ground is detected
    }

    private void FacePlayer(Vector2 playerDirection)
    {
        if(playerDirection.x < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true; 
        }
    }

    private void MoveTowardsPlayer(Vector2 playerDirection)
    {
        rb.velocity = new Vector2(playerDirection.x * enemyMovementSpeed, rb.velocity.y); //move towards player
        animator.SetBool("isMoving", true); //set walking animation
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y); //stop moving
        animator.SetBool("isMoving", false); //set idle animation
    }
}
