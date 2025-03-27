using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    public Transform patrolPointA;
    public Transform patrolPointB;
    private Transform currentTarget;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private bool allowedToMove = true;
    private bool allowedToAttack = true;

    public void SetEnemyState(bool isActive)
    {
        allowedToMove = isActive;
        allowedToAttack = isActive;

        if (!isActive)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentTarget = patrolPointA;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        if (!allowedToMove) return;

        // Detect player within range
        bool playerInRawSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        bool playerInRawAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Line of sight check
        if (playerInRawSight)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, whatIsGround))
            {
                playerInSightRange = true;
            }
            else
            {
                playerInSightRange = false; // There's an obstacle (e.g., a roof)
            }
        }
        else
        {
            playerInSightRange = false;
        }

        // Same check for attack range
        if (playerInRawAttack)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, whatIsGround))
            {
                playerInAttackRange = true;
            }
            else
            {
                playerInAttackRange = false;
            }
        }
        else
        {
            playerInAttackRange = false;
        }

        // Enemy Behavior
        if (!playerInSightRange && !playerInAttackRange)
            PatrolBetweenPoints();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInAttackRange && allowedToAttack)
            AttackPlayer();
    }


    private void PatrolBetweenPoints()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Transform nextTarget = (currentTarget == patrolPointA) ? patrolPointB : patrolPointA;

            if (NavMesh.SamplePosition(nextTarget.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                currentTarget = nextTarget;
                agent.SetDestination(hit.position);
            }
            else
            {
                Debug.LogWarning($"Enemy {gameObject.name} cannot reach {nextTarget.name}. Check your NavMesh!");
            }
        }
    }


    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 lookAtTarget = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
        transform.LookAt(lookAtTarget);

        if (!alreadyAttacked)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
            Rigidbody rb = Instantiate(projectile, spawnPosition, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 direction = (player.position - spawnPosition).normalized;
            rb.AddForce(direction * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
