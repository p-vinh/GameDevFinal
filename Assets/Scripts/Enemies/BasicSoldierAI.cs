using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicSoldierAI : MonoBehaviour {
    // This will have basic Human AI
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    public GameObject fist;
    private Transform player;
    private Animator animator;
    private PlayerStats playerStats;

    // Variables
    public float speed = 5.0f; //can be changed for each type of soldier (running speed)
    public float shieldHealth = 50.0f;
    public float sightRange = 8.0f;
    public float attackRange = 1.0f;
    public float health = 100.0f; //can be changed for each type of soldier
    public float punchDamage = 5.0f;

    public bool canProtectWithShield = true;

    // The Basic Soldier Enemy protects with a shield once the player is in sight range.
    // The shield will take damage for the enemy until it is destroyed. Once the shield is destroyed,
    // the enemy will attack with punch. The enemy will attack the player if the player is in attack range.
    // The enemy will move to the player if the player is in sight range.
    // The enemy will stop moving if the player is in attack range. The enemy will stop moving if the player is dead.

    // Setup Variables
    void Start() {
        mesh = GetComponent<NavMeshAgent>(); // Get the Navigator
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform; // Find the player object
        // Create a fist object attached to the enemy's hand position
        // TODO: figure out how to work with an enemy fist (COuld just be animation and hand colision)
        fist = Instantiate(fist, enemy.position, enemy.rotation);
        animator = GetComponent<Animator>(); // Get the animator component
        //playerStats = player.GetComponent<PlayerStats>(); // Get the player stats
        mesh.speed = speed; // Set running speed
    }//end Start()

    // Boolean Helper Methods //
    bool checkSightRange() {
        if (Vector3.Distance(transform.position, player.position) <= sightRange) {
            return true;
        }//end if
        return false;
    }//end checkSightRange()

    bool checkAttackRange() {
        if (Vector3.Distance(transform.position, player.position) <= attackRange) {
            return true;
        }//end if
        return false;
    }//end checkAttackRange()

    // Update is called once per frame
    void Update() {
        // Check Ranges
        checkSightRange();
        checkAttackRange();

        // TODO: Always look at player

        // Move to Player if in sight range
        if (checkSightRange() && !checkAttackRange()) {
            if (canProtectWithShield) {
                ProtectWithShield();
            }//end if
            MoveToPlayer();
        } else if (checkSightRange() && checkAttackRange()) {
            AttackPlayer();
            if (shieldHealth <= 0) {
                canProtectWithShield = false;
                AttackPlayer();
            }//end if
    }//end Update()

    // Universal Soldier AI Methods //
    // Move to Player AI
    void MoveToPlayer() {
        if (Vector3.Distance(transform.position, player.position) <= sightRange) {
            mesh.SetDestination(player.position);
            // TODO: Always look at player
        }//end if
    }//end moveToPlayer()

    void ProtectWithShield() {
        if (shieldHealth > 0) {
            // Shield takes damage
            shieldHealth -= playerStats.Get(Damage); // TODO: Find out how to connect to player
        } else if (shieldHealth <= 0) {
            // Shield is destroyed
            canProtectWithShield = false;
            Destroy(shield);
        }//end if
    }//end ProtectWithShield()

    void AttackPlayer() {
        // Check if the player is close to the enemy
        if (checkAttackRange()) {
            // Stop moving
            mesh.SetDestination(enemy.position); // Stop moving
            mesh.isStopped = true; // Stop the enemy

            animator.SetTrigger("PunchAttack"); // Punch the player
            // Player take Damage if the fist collides with the player
            if (fist.GetComponent<Collider>().bounds.Intersects(player.GetComponent<Collider>().bounds)) {
                playerStats.TakeDamage(punchDamage);
                Debug.Log("Attack Player");
            }//end if
        }//end if
    }//end AttackPlayer()

    // Take Damage if sheild is down
    void TakeDamage() {
        if (shieldHealth <= 0) {
            health -= playerDamage; // Player attacks the enemy
            canProtectWithShield = false; // Destroy the shield
            Destroy(shield); // Destroy the shield
        }//end if

        // Check if the enemy is dead
        if (health <= 0) {
            Destroy(gameObject); // Kill the enemy
        }//end if
    }//end EnemyTakeDamage()
}//end BasicSoldierAI
