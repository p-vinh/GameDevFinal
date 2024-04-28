using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralSoldierAI : MonoBehaviour {
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    private Transform player;
    private PlayerController playerController;
    //private Animator animator;
    //private Animator playerAnimator;
    //private GameObject shield;
    private Collider sword;

    // Variables
    public float speed = 3.5f;
    public float attackSpeed = 10.0f;
    public float protectDuration = 10.0f; 
    public float downtime = 5.0f; // Should be smaller
    public float sightRange = 8.0f; 
    public float attackRange = 3.0f; // Check this based on how close the sword needs to be to hit the player
    public int health = 100;
    public static int maxHealth = 100;

    // Damage Variables
    public int swordDamage = 5; // general soldier attack

    // Boolean Variables
    private bool canTakeDamage = true;
    //private bool isAttaching = false;
    private bool canMove = true;


    // Setup Variables
    void Start() {
        mesh = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        //animator = GetComponent<Animator>();
        //playerAnimator = player.GetComponent<Animator>();
        mesh.speed = speed;
        //shield = GetComponentInChildren("Shield");
        //sword = GetComponentInChildren("Sword").GetComponent<Collider>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        checkAttackRange();

        if (checkSightRange() && !canTakeDamage) {
            canTakeDamage = true; // Drop Shield
        }//end if

        // Move to Player if in sight range
        if (checkSightRange() && !checkAttackRange() && !isAttacking && !canTakeDamage) {
            if (canMove) {
                MoveToPlayer();
            }//end if
        }//end if

        // Protect with Shield
        if (checkSightRange() && canTakeDamage) { // && Player Position Requirement
            ProtectWithShield(); // Start the ShieldRoutine coroutine
        }//end if

        // Attack Player if in attack range
        if (checkSightRange() && checkAttackRange() && !isAttacking && !canTakeDamage) {
            mesh.SetDestination(enemy.position); // Stop moving
            StartCoroutine(AttackPlayer(swordDamage));
            //isAttacking = true;
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
        if (checkSightRange() && !checkAttackRange()) {
            mesh.SetDestination(player.position);
            //enemy.LookAt(player.position);
        }//end if
    }//end moveToPlayer()

    // Attack Player if in range
    IEnumerator AttackPlayer(int damage) {
        //canMove = false;
        canTakeDamage = true; // Drop Sheild
        mesh.SetDestination(enemy.position); // Stop moving
        //enemy.LookAt(player.position);

        // TODO: Set up the attack animation with the sword and if the sword object hits the player then and only then the player takes damage
        if (checkAttackRange()) {
            //animator.SetTrigger("SwordAttack");
            
            yield return new WaitForSeconds(attackSpeed);
            // if sword hits player then player takes damage
            // if (sword.bounds.Intersects(player.GetComponent<Collider>().bounds)) {
            //     playerController.TakeDamage(damage);
            // }//end if

            Debug.Log("General Soldier Attack Player");
        }//end if
    }//end attackPlayer()

    // Protect with Shield Coroutine (handels time and downtime)
    IEnumerator ShieldRoutine(float protectDuration, float downtime) {
        while (true) {
            // Protect with Shield
            canTakeDamage = false;
            Debug.Log("Protect With Shield");

            // Shield Animation
            //animator.SetTrigger("ProtectWithShield");

            // Wait for protectDuration seconds
            yield return new WaitForSeconds(protectDuration);

            // Stop protecting with Shield
            canTakeDamage = true;
            Debug.Log("Stop Protecting With Shield");

            // Wait for downtime seconds
            yield return new WaitForSeconds(downtime);
        }//end while
    }//end ShieldRoutine()

    // Protect with Shield
    void ProtectWithShield() {
        // Start the ShieldRoutine coroutine with 5 seconds of protection and 2 seconds of downtime
        StartCoroutine(ShieldRoutine(5f, 2f));
    }//end ProtectWithShield()

    // Take Damage if sheild is down
    public void EnemyTakeDamage(int damage) {
        if (canTakeDamage) {
            if (health >= damage) {
                health -= damage;
            } else {
                health = 0;
            }//end if
            Debug.Log("General Soldier Health: " + health);

            if (health <= 0) {
                Destroy(gameObject);
            }//end if
        }//end if
    }//end EnemyTakeDamage()

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}//end GeneralSoldierAI
