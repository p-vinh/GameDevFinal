using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralSoldierAI : MonoBehaviour {
    // This will have basic Human AI
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    private Transform player;
    private Animator animator;
    private Animator playerAnimator;
    private GameObject shield;
    private GameObject sword;
    public GameObject soldierType; //set based on Tag

    // Variables
    public float speed = 5.0f; //can be changed for each type of soldier
    public float increasedSpeed = 10.0f; //can be changed for each type of soldier
    public float sightRange = 10.0f; //can be changed for Juggernaut
    public float attackRange = 2.0f; //can be changed for each type of soldier (or attacks if we want)
    public int health = 100; //can be changed for each type of soldier
    private  int maxHealth = health;
    private int halfHealth = maxHealth / 2;

    // Damage Variables
    public int swordDamage = 10; // general soldier attack
    public int specialDamage = 30; // general soldier special attack
    public int swordPhaseTwoSwordDamage = 20; // general soldier phase 2 attack

    // Boolean Variables
    private bool canProtectWithShield = false;
    private bool canTakeDamage = true;


    // Setup Variables
    void Start() {
        mesh = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        mesh.speed = speed;
        shield = GetComponentInChildren("Shield");
        sword = GetComponentInChildren("Sword");
        canProtectWithShield = true;
    }//end Start()

    // Update is called once per frame
    void Update() {
        // TODO: Always look at player ?
        enemy.LookAt(player.position);

        // Phase 2 AI
        if (health == halfHealth) {
            PhaseTwo(specialDamage);
        }//end if

        // Move to Player if in sight range
        if (checkSightRange()) {
            MoveToPlayer();
        }//end if

        // Protect with Shield
        if (canProtectWithShield) { // && Player Possition Requirement
            ProtectWithShield();
        }//end if

        // Attack Player if in attack range
        if (checkAttackRange()) {
            if (health <= halfHealth) {
                AttackPlayer(swordPhaseTwoSwordDamage);
            } else {
                AttackPlayer(swordDamage);
            }//end if
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
            enemy.LookAt(player.position);
        }//end if
    }//end moveToPlayer()

    // Attack Player if in range
    void AttackPlayer(ing damage) {
        //attack player
        mesh.SetDestination(transform.position);
        enemy.LookAt(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackRange) {
            animator.SetTrigger("SwordAttack");
            canTakeDamage = true;

            player.GetComponent<Player>().TakeDamage(damage);
        }//end if
        Debug.Log("Human Enemy Attacking Player");
    }//end attackPlayer()

    // Take Damage if sheild is down
    void EnemyTakeDamage(int damage) {
        if (canTakeDamage) {
            health -= damage;
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
        animator.SetTrigger("ProtectWithShield");

        Debug.Log("Protect With Shield");
    }//end ProtectWithShield()
    
    // Phase 2 AI for General Soldier //
    // Remove General Soldier Shield
    void RemoveShield() {
        if (soldierType == "GeneralSoldierEnemy") {
            if (halfHealth == health) {
                animator.SetTrigger("RemoveShield");

                Destroy(shield);
                canProtectWithShield = false;
                canTakeDamage = true;
            }//end if
        }//end if
    }//end RemoveShield()

    // Phase 2 AI for General Soldier and Juggernaut //

    // Controls Phase 2 Start for General Soldier and Juggernaut
    void PhaseTwo() {
        // If General Soldier, Remove Shield
        RemoveShield();

        // Increase Speed
        mesh.speed = increasedSpeed;
        
        // Move to Player
        while (checkAttackRange() == false && checkSightRange() == true) {
            mesh.SetDestination(player.position);
            enemy.LookAt(player.position);
        }//end while

        // Special Attack
        if (checkAttackRange()) {
            AttackPlayer(specialDamage);
            Debug.Log("Special Attack");
        }//end if
    }//end PhaseTwo()
}//end GeneralSoldierAI
