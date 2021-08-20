
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi2 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform targetpos;
    //public GameObject gun;
    public Transform enemyPos;

    //Stats
    public float health;
    private bool hit;
    public static float damageToPlayer = 20f;

    //Check for Ground/Obstacles
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attack Player
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public bool isDead;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Special
    public Material blue, orange, yellow;
    public GameObject projectile;
    public Transform shootPoint;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Bullet")
        {
            Destroy(other.gameObject);
            hit = true;
        }
    }



    private void Update()
    {


        if (hit)
        {
            health -= GameManager.Instance.bullet1Damage;
            hit = false;

            if (health <= 0)
                Destroy(this.gameObject);
        }

      



        if (!isDead)
        {
            //Check if Player in sightrange
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

            //Check if Player in attackrange
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }


    private void Patroling()
    {
        if (isDead) return;

        if (!walkPointSet) SearchWalkPoint();

        //Calculate direction and walk to Point
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            //Vector3 direction = walkPoint - transform.position;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }

        //Calculates DistanceToWalkPoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 3f)
            walkPointSet = false;

        GetComponent<MeshRenderer>().material = blue;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        if (isDead) return;

        agent.SetDestination(player.position);

        GetComponent<MeshRenderer>().material = yellow;
    }


    private void AttackPlayer()
    {
        if (isDead) return;

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);


        //transform.LookAt(player);
        Vector3 lookAtPos = targetpos.position - enemyPos.transform.position;
        // lookAtPos.y = player.transform.position.y; // do not rotate the player around x
        Quaternion newRotation = Quaternion.LookRotation(lookAtPos, enemyPos.transform.up);
        enemyPos.transform.rotation = Quaternion.Slerp(enemyPos.transform.rotation, newRotation, Time.deltaTime * 8);



        if (!alreadyAttacked)
        {

            //Attack
            //            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();

            Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            //  Vector3 dir = Quaternion.AngleAxis(shootAngle, Vector3.forward) * Vector3.right;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(-transform.up * 11f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }

        GetComponent<MeshRenderer>().material = orange;
    }
    private void ResetAttack()
    {
        if (isDead) return;

        alreadyAttacked = false;
    }
    


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
