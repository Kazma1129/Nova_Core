
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{

    [Header("Transform etc")]
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public Transform shootPoint;
    public NavMeshAgent agent;
    private bool hit;

    //patrolling
    [Header("Patrolling Settings")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public float distanceToReachedMagnitude;

    //attack
    [Header("Attack Settings")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public float health;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public float timer;
    public float damageTP;
    public static float damageToPlayer;
    private Transform cameraTransform; //to get main camera.
    
    /* for missing the target, maybe I'll find a better use for this.
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    */

    private void Awake()
    {
        damageToPlayer = damageTP;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = Camera.main.transform;

    }

    private void Update()
    {
        
        timer -= Time.deltaTime;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); // range for player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //range for attack
       
        //States
        if (!playerInSightRange && !playerInAttackRange) patrolling(); // if not in sight or attack range patroll randomly
        if (playerInSightRange && !playerInAttackRange) chasePlayer(); // if player in sight but not attack range, chase player
        if (playerInAttackRange && playerInSightRange) attackPlayer();   //if in range and sight range attack player (stops and attacks)

        if (hit)
        {
            health -= GameManager.Instance.bullet1Damage;
            hit = false;
        }

        if (health <= 0)
            Destroy(this.gameObject);

    }


    private void patrolling() {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 3f)
            walkPointSet = false;
    }

    private void searchWalkPoint() {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //walpoint position hey
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //once it reaches the walkpoint it resets
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
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position), 10 * Time.deltaTime);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer //oddly enough it shoots at feet.
     
            if (timer <=0 &&Physics.Raycast(shootPoint.position, shootPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, whatIsPlayer))
            {

                Debug.DrawRay(shootPoint.position, shootPoint.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                timer = timeBetweenAttacks;
            }
        
        
        //attack based on projectile (will be replaced by raycast)
        /*
        if(timer <= 0) { 
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        timer = timeBetweenAttacks;
        }
        */
        


        /*
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, Mathf.Infinity))
            Debug.DrawRay(shootPoint.position, shootPoint.forward, Color.green);//print("There is something in front of the object!");





        ////Raycast Test
        /*
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(projectile, shootPoint.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {

            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }

        */

        //////////////////////////////////////////////////////////////////////////////
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            resetAttack();
        }
    }

    private void resetAttack() {
        alreadyAttacked = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player Bullet")
        {
            hit = true;
        }
    }

    

    private void DestroyEnemy()
    {
        Destroy(gameObject,.5f);
    }
    


    // for range visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
