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

    // Variables
    public float speed = 5.0f; //can be changed for each type of soldier (running speed)
    public float sightRange = 8.0f;
    public float attackRange = 1.0f;
    public float health = 100f; //can be changed for each type of soldier
    public float punchDamage = 5f;

    // Setup Variables
    void Start() {
        mesh = GetComponent<NavMeshAgent>(); // Get the Navigator
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform; // Find the player object
        // Create a fist object attached to the enemy's hand position
        fist = Instantiate(fist, enemy.position, enemy.rotation);
        animator = GetComponent<Animator>(); // Get the animator component
        mesh.speed = speed; // Set running speed
    }//end Start()

    // Update is called once per frame
    void Update() {
        // Check Ranges
        checkSightRange();
        checkAttackRange();

        // Always look at player
        //mesh.LookAt(player);

        // Move to Player if in sight range
        if (checkSightRange() && !checkAttackRange()) {
            MoveToPlayer();
        } else if (checkSightRange() && checkAttackRange()) {
            AttackPlayer();
        }//end if
    }//end Update()

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

    // Universal Soldier AI Methods //
    // Move to Player AI
    void MoveToPlayer() {
        if (Vector3.Distance(transform.position, player.position) <= sightRange) {
            mesh.SetDestination(player.position);
            //mesh.LookAt(player);
        }//end if
    }//end moveToPlayer()

    void AttackPlayer() {
        // Check if the player is close to the enemy
        if (checkAttackRange()) {
            mesh.SetDestination(enemy.position); // Stop moving
            mesh.isStopped = true; // Stop the enemy
            animator.SetTrigger("PunchAttack"); // Punch the player
            // If animation is done, call the player's TakeDamage() method
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PunchAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                if (fist.GetComponent<Collider>().bounds.Intersects(player.GetComponent<Collider>().bounds)) {
                    Debug.Log("Attack Player");
                    // playerStats.TakeDamage(punchDamage);
                }//end if
            }//end if
        }//end if
    }//end AttackPlayer()

    // Take Damage if sheild is down
    public void EnemyTakeDamage(int damage) {
        //health -= damage; // Player attacks the enemy

        // Check if the enemy is dead
        if (health <= 0) {
            Destroy(gameObject); // Kill the enemy
        }//end if
    }//end EnemyTakeDamage()
}//end BasicSoldierAI
