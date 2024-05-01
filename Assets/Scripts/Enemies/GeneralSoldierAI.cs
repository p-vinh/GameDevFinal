using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralSoldierAI : EnemyAI {
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    private Transform player;
    private Animator animator;
    private BoxCollider sword;

    // State Variables
    private enum State {
        Idle,
        Approach,
        Attack,
        Cooldown
    }//end enum-State
    private State state;

    // Public Variables
    public float health = 100.0f;
    public float swordDamage = 5.0f; // general soldier attack
    public float speed = 2.0f;
    public float attackSpeed = 5.0f;
    public float sightRange = 8.0f; 
    public float attackRange = 3.0f; // Check this based on how close the sword needs to be to hit the player
    public override string EnemyType => "General Soldier";
    private float distanceToPlayer;
    private bool canMove = true;


    // Setup Variables
    void Start() {
        // Set up the enemy stats
        bloodManager = FindObjectOfType<BloodManager>();
        Stats = new EnemyStats(health, swordDamage, speed);
        // Set up the enemy AI
        mesh = GetComponent<NavMeshAgent>();
        mesh.speed = speed;
        enemy = GetComponent<Transform>();
        // Get other game objects
        player = FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        sword = GetComponentInChildren("Sword").GetComponent<Collider>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Move to Player if in sight range
       //if (canMove) {
            if (distanceToPlayer <= sightRange && distanceToPlayer > attackRange) {
                MoveToPlayer();
            } else if (distanceToPlayer <= attackRange) {
                mesh.SetDestination(enemy.position); // Stop moving
                AttackPlayer(swordDamage);
            }//end else-if
        //} else if (distanceToPlayer > attackRange) {
          //  canMove = true;
        //}//end else-if
    }//end Update()

    // Universal Soldier AI Methods //
    // Move to Player AI
    void MoveToPlayer() {
        mesh.SetDestination(player.position);
    }//end MoveToPlayer()

    // Attack Player if in range
    void AttackPlayer(int damage) {
        // TODO: Set up the attack animation with the sword and if the sword object hits the player then and only then the player takes damage
        canMove = false;

        if (distanceToPlayer <= attackRange) {
            animator.SetBool("Attack", true);

            Debug.Log("General Soldier Attack Player");
            if (sword.bounds.Intersects(player.GetComponent<Collider>().bounds)) {
                PlayerStats.Instance.TakeDamage(damage);
            }//end if
        }//end if
    }//end AttackPlayer()

    void changeCanMove() {
        canMove = true;
        Debug.Log("Can Move: " + canMove);
    }//end changeCanMove()

    // This is a fake method so I cancolapse commented code while working on the other methods
    void shield() {
        // Boolean Helper Methods //
        // bool checkSightRange() {
        //     if (Vector3.Distance(transform.position, player.position) <= sightRange) {
        //         return true;
        //     }//end if
        //     return false;
        // }//end checkSightRange()

        // bool checkAttackRange() {
        //     if (Vector3.Distance(transform.position, player.position) <= attackRange) {
        //         return true;
        //     }//end if
        //     return false;
        // }//end checkAttackRange()

        // Protect with Shield Coroutine (handels time and downtime)
        // IEnumerator ShieldRoutine(float protectDuration, float downtime) {
        //     while (true) {
        //         // Protect with Shield
        //         canTakeDamage = false;
        //         Debug.Log("Protect With Shield");

        //         // Shield Animation
        //         //animator.SetTrigger("ProtectWithShield");

        //         // Wait for protectDuration seconds
        //         yield return new WaitForSeconds(protectDuration);

        //         // Stop protecting with Shield
        //         canTakeDamage = true;
        //         Debug.Log("Stop Protecting With Shield");

        //         // Wait for downtime seconds
        //         yield return new WaitForSeconds(downtime);
        //     }//end while
        // }//end ShieldRoutine()

        // Protect with Shield
        // void ProtectWithShield() {
        //     // Start the ShieldRoutine coroutine with 5 seconds of protection and 2 seconds of downtime
        //     StartCoroutine(ShieldRoutine(5f, 2f));
        // }//end ProtectWithShield()
    }
    
    // Take Damage if sheild is down
    public void EnemyTakeDamage(int damage) {
        if (health >= damage) {
            health -= damage;
        } else {
            health = 0;
        }//end if-else
        Debug.Log("General Soldier Health: " + health);

        if (health <= 0) {
            Destroy(gameObject);
        }//end if
    }//end EnemyTakeDamage()

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }//end OnDrawGizmosSelected()
}//end GeneralSoldierAI
