using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using PlayerController;

public class GeneralSoldierAI : MonoBehaviour {
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    private Transform player;
    //private Animator animator;
    //private Animator playerAnimator;
    //private GameObject shield;
    //private GameObject sword;

    // Variables
    public float speed = 3.5f; //can be changed for each type of soldier
    public float attackSpeed = 10.0f; //can be changed for each type of soldier
    public float increasedSpeed = 10.0f; //can be changed for each type of soldier
    public float sightRange = 8.0f; //can be changed for Juggernaut
    public float attackRange = 1.0f; //can be changed for each type of soldier (or attacks if we want)
    public int health = 100; //can be changed for each type of soldier
    public static int maxHealth = 100;
    private int halfHealth = maxHealth / 2;

    // Damage Variables
    public int swordDamage = 5; // general soldier attack
    public int specialDamage = 15; // general soldier special attack
    public int swordPhaseTwoSwordDamage = 10; // general soldier phase 2 attack

    // Boolean Variables
    private bool canProtectWithShield = true;
    private bool canTakeDamage = true;
    //private bool isAttaching = false;
    private bool canMove = true;


    // Setup Variables
    void Start() {
        mesh = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        //animator = GetComponent<Animator>();
        //playerAnimator = player.GetComponent<Animator>();
        mesh.speed = speed;
        //shield = GetComponentInChildren("Shield");
        //sword = GetComponentInChildren("Sword");
        //canProtectWithShield = true;
    }//end Start()

    // Update is called once per frame
    void Update() {
        // TODO: Always look at player ?
        //enemy.LookAt(player.position);
        checkSightRange();
        checkAttackRange();

        if (checkSightRange()) {
            canTakeDamage = true; // Drop Sheild
        }//end if

        // Phase 2 AI
        if (health == halfHealth) {
            PhaseTwo();
        }//end if

        // Move to Player if in sight range
        if (checkSightRange() && !checkAttackRange()) {
            // TODO: Test for when you want the enemy to move to the player (At start or Set player out of sight range)
            if (canMove)
                MoveToPlayer();
        }//end if

        // Attack Player if in attack range
        if (checkSightRange() && checkAttackRange()) {
            mesh.SetDestination(enemy.position); // Stop moving
            if (health <= halfHealth) {
                AttackPlayer(swordPhaseTwoSwordDamage);
                canMove = true;
            } else {
                AttackPlayer(swordDamage);
                canMove = true;
            }//end if-else
        }//end if

        // Protect with Shield
        if (canProtectWithShield && checkSightRange()) { // && Player Position Requirement
            ProtectWithShield();
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
    void AttackPlayer(int damage) {
        canMove = false;
        canTakeDamage = true; // Drop Sheild
        mesh.SetDestination(enemy.position); // Stop moving
        //enemy.LookAt(player.position);

        if (checkAttackRange()) {
            //animator.SetTrigger("SwordAttack");

            //player.TakeDamage(damage);
            Debug.Log("General Soldier Attack Player");
        }//end if
    }//end attackPlayer()

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

    // General Soldier AI and Juggernaut AI //
    // Protect with Shield
    void ProtectWithShield() {
        // Protect with Shield
        canTakeDamage = false;

        // Shield Animation
        //animator.SetTrigger("ProtectWithShield");
        Debug.Log("Protect With Shield");
    }//end ProtectWithShield()
    
    // Phase 2 AI for General Soldier //
    // Remove General Soldier Shield
    void RemoveShield() {
        if (halfHealth == health) {
            //animator.SetTrigger("RemoveShield");

            //Destroy(shield);
            Debug.Log("Shield Removed");
            canProtectWithShield = false;
            canTakeDamage = true;
        }//end if
    }//end RemoveShield()

    // Controls Phase 2
    void PhaseTwo() {
        // Break Shield
        RemoveShield();

        // Increase Speed
        mesh.speed = increasedSpeed;
        
        // Move to Player
        while (checkAttackRange() == false || enemy.position != player.position) {
            mesh.SetDestination(player.position);
            //enemy.LookAt(player.position);
        }//end while

        // Special Attack
        AttackPlayer(specialDamage);
        Debug.Log("Special Attack");
    }//end PhaseTwo()

    // For testing purposes
    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }//end OnDrawGizmosSelected()
}//end GeneralSoldierAI
