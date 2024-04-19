using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicSoldierAI : MonoBehaviour {
    // This will have basic Human AI
    // Unity Editor Variables
    private NavMeshAgent mesh;
    private Transform enemy;
    private Transform player;
    private Animator animator;
    private Animator playerAnimator;

    // Variables
    public float speed = 5.0f; //can be changed for each type of soldier
    public float increasedSpeed = 10.0f; //can be changed for each type of soldier
    public float sightRange = 10.0f; //can be changed for Juggernaut
    public float attackRange = 2.0f; //can be changed for each type of soldier (or attacks if we want)
    public int punchDamage = 5; // basic soldier attack
    public int health = 100; //can be changed for each type of soldier
    private  int maxHealth = health;
    private int halfHealth = maxHealth / 2;
    private bool canTakeDamage = true;

    // Setup Variables
    void Start() {
        soldierType = GameObject.Tag;
        mesh = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        mesh.speed = speed;
        damage = punchDamage;
    }//end Start()

    // Update is called once per frame
    void Update() {
        // Always look at player
        enemy.LookAt(player.position);

        // Move to Player if in sight range
        if (checkSightRange()) {
            MoveToPlayer();
        }//end if

        // Attack Player if in attack range
        if (checkAttackRange()) {
            AttackPlayer(animator, damage);
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
    void AttackPlayer(Animator animator, int damage) {
        //attack player
        mesh.SetDestination(transform.position);
        enemy.LookAt(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackRange) {
            animator.SetTrigger(GetComponent<Animation>());

            player.GetComponent<Player>().TakeDamage(damage);
        }//end if
        Debug.Log("Human Enemy Attacking Player");
    }//end attackPlayer()

    // Take Damage if sheild is down
    void EnemyTakeDamage() {
        if(canTakeDamage) {
            health -= punchDamage;
            if (health <= 0) {
                Destroy(gameObject);
            }//end if
        }//end if
    }//end EnemyTakeDamage()
}//end BasicSoldierAI
