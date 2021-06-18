
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public Transform shootPoint;

    //patrolling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    //attack
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    public float health;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); // range for player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //range for attack
        
        //States
        if (!playerInSightRange && !playerInAttackRange) patrolling(); // if not in sight or attack range patroll randomly
        if (playerInSightRange && !playerInAttackRange) chasePlayer(); // if player in sight but not attack range, chase player
        if (playerInAttackRange && playerInSightRange) attackPlayer();   //if in range and sight range attack player (stops and attacks)


    }


    private void patrolling() {
        //if walkpoint is not set create it
        if (!walkPointSet) searchWalkPoint();

        //if walkpoint created set agent destination to walkpoint
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        //check distance from agent to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint Reached so we set walkpoint set to false to generate a new coordinate.
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void searchWalkPoint() {
        // calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //new walking point
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        //can delete this line if we don't have a ground layer.
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }


    private void chasePlayer() {
        //goes towards player.
        agent.SetDestination(player.position);

    }

    private void attackPlayer() {
        agent.SetDestination(transform.position); // set destination to player
        transform.LookAt(player); //look at player

        //attack based on projectile (will be replaced by raycast)
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttacks);
        }
    }

    private void resetAttack() {
        alreadyAttacked = false;
    }
    /* WIP
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject,.5f);
    }
    */


    // for range visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
