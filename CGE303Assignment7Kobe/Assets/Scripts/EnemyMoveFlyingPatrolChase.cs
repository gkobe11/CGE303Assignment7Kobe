using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveFlyingPatrolChase : MonoBehaviour
{
    public GameObject[] patrolPoints;

    public float speed = 2f;
    public float chaseRange = 3f;

    public enum EnemyState
    {
        Patrol,
        Chase
    }

    public EnemyState currentState = EnemyState.Patrol;

    public GameObject target;

    private GameObject player;

    private Rigidbody2D rb;

    private SpriteRenderer sr;

    private int currentPatrolIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (patrolPoints.Length == null || patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned to the enemy.");
        }

        target = patrolPoints[currentPatrolIndex];
    }

    void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        }

        Debug.DrawLine(transform.position, target.transform.position, Color.red);
    }

    void UpdateState()
    {
        if (isPlayerInChaseRange() && currentState == EnemyState.Patrol)
        {
            currentState = EnemyState.Chase;
        }
        else if (!isPlayerInChaseRange() && currentState == EnemyState.Chase)
        {
            currentState = EnemyState.Patrol;
        }
    }

    bool isPlayerInChaseRange()
    {
        if (player == null)
        {
            Debug.LogError("Player not found.");
            return false;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= chaseRange;
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        target = patrolPoints[currentPatrolIndex];

        MoveTowardsTarget();
    }

    void ChasePlayer()
    {
        target = player;
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
        FaceForward(direction);
    }

    void FaceForward(Vector2 direction)
    {
        if (direction.x < 0)
        {
            sr.flipX = false;
        }
        else if (direction.x > 0)
        {
            sr.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (GameObject point in patrolPoints)
            {
                Gizmos.DrawSphere(point.transform.position, 0.5f);
            }
        }
    }
}
