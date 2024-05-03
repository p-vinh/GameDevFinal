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
    public AudioSource attackSound;

    // State Variables
    private enum State {
        Idle,
        Chase,
        Attack
    }//end enum-State
    private State state;

    // Public Variables
    public float health = 100.0f;
    public float swordDamage = 5.0f; // general soldier attack
    public float speed = 2.0f;
    public float attackSpeed = 5.0f;
    public float sightRange = 5.0f; 
    public float attackRange = 2.0f;
    public override string EnemyType => "General Soldier";
    private float distanceToPlayer;
    private bool canMove = true;


    // Setup Variables
    protected override void Start() {
        // Set up the enemy stats
        bloodManager = FindObjectOfType<BloodManager>();
        Stats = new EnemyStats(health, swordDamage, speed);
        // Set up the enemy AI
        mesh = GetComponent<NavMeshAgent>();
        mesh.speed = speed;
        enemy = GetComponent<Transform>();
        // Get other game objects
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        // Get the enemies sword collider
        sword = GetComponentInChildren<BoxCollider>();
        state = State.Idle;
    }//end Start()

    // Update is called once per frame
    protected override void Update() {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Move to Player if in sight range
        if (distanceToPlayer <= sightRange && distanceToPlayer > attackRange && canMove) {
            animator.SetBool("Attack", false);
            MoveToPlayer();
        } else if (distanceToPlayer <= attackRange) {
            mesh.SetDestination(enemy.position); // Stop moving
            Attack();
            canMove = true;
        } else if (distanceToPlayer > sightRange && distanceToPlayer > attackRange) {
            animator.SetBool("Attack", false);
            StopMoving();
        }//end else-if
    }//end Update()

    // Universal Soldier AI Methods //
    // Move to Player AI
    void MoveToPlayer() {
        // TODO: Walking Animation not setup yet
        //animator.SetBool("Walking", true);
        mesh.SetDestination(player.position);
        state = State.Chase;
    }//end MoveToPlayer()

    // Stop Moving AI
    void StopMoving() {
        // TODO: Return to Idle Animation not setup yet
        //animator.SetBool("Idle", true);
        mesh.SetDestination(enemy.position);
        state = State.Idle;
    }//end StopMoving()

    // Attack Player if in range
    public override void Attack() {
        // TODO: Set up the attack animation with the sword and if the sword object hits the player then and only then the player takes damage
        canMove = false;

        state = State.Attack;
        animator.SetBool("Attack", true);

        Debug.Log("General Soldier Attack Player");
        if (sword.bounds.Intersects(player.GetComponent<Collider>().bounds)) {
            PlayerStats.Instance.Health -= swordDamage;
        }//end if
    }//end AttackPlayer()

    void changeCanMove() {
        canMove = true;
        Debug.Log("Can Move: " + canMove);
    }//end changeCanMove()

    // Take Damage if sheild is down
    public override void TakeDamage(float damage) {
        Stats.Health -= damage;
        Debug.Log("General Soldier Health: " + Stats.Health);

        if (Stats.Health <= 0) {
            Die();
        }//end if
    }//end EnemyTakeDamage()

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }//end OnDrawGizmosSelected()
}//end GeneralSoldierAI
