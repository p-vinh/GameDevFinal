using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlankStudio.Constants;

public class GeneralSoldierAI : EnemyAI {
    // Unity Editor Variables
    protected NavMeshAgent mesh;
    protected Transform enemy;
    protected Transform player;
    protected Animator animator;
    protected BoxCollider sword;
    public AudioSource attackSound;
    public EnemyStats enemyStats;

    // State Variables
    private enum State {
        Idle,
        Chase,
        Attack
    }//end enum-State
    private State state;

    // Public Variables
    public float attackRange = 6.0f;
    public override Constants.EnemyType Type => Constants.EnemyType.GeneralSoldier;
    private float distanceToPlayer;
    private Vector3 playerCurrentPosition;
    public bool canAttackAgain = true;


    // Setup Variables
    protected override void Start() {
        // Set up the enemy stats
        base.Start();
        enemyStats = base.Stats;
        // Set up the enemy AI
        player = GameObject.FindGameObjectWithTag("Player").transform;

        print(Stats.Health);
        if (player != null) {
            mesh = GetComponent<NavMeshAgent>();
            mesh.speed = Stats.Speed;
            enemy = GetComponent<Transform>();
            animator = GetComponent<Animator>();
            sword = GetComponentInChildren<BoxCollider>();
            state = State.Idle;
        }//end if
    }//end Start()

    // Update is called once per frame
    protected override void Update() {
        if (player != null) {
            bool playerinAttackRange = Vector3.Distance(player.position, transform.position) < attackRange;
            if (canAttackAgain) {
                playerCurrentPosition = player.position;
                MoveToPlayer();
            }//end if

            if (playerinAttackRange && canAttackAgain) {
                mesh.SetDestination(enemy.position);
                Attack();
            }//end if
        }//end if
    }//end Update()

    // Move to Player AI
    void MoveToPlayer() {
        mesh.isStopped = false;
        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);
        enemy.LookAt(playerCurrentPosition);
        mesh.SetDestination(playerCurrentPosition);
        state = State.Chase;
    }//end MoveToPlayer()

    void StopMoving() {
        animator.SetBool("Attack", false);
        animator.SetBool("Walk", false);
        mesh.SetDestination(enemy.position);
        mesh.isStopped = true;
        state = State.Idle;
    }//end StopMoving()

    protected override void Attack() {
        enemy.LookAt(playerCurrentPosition);
        canAttackAgain = false;
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
        
        state = State.Attack;

        if (sword.bounds.Intersects(player.GetComponent<Collider>().bounds)) {
            PlayerStats.Instance.Health -= Stats.Damage;
        }//end if
    }//end Attack()

    void changeCanMove() {
        canAttackAgain = true;
    }//end changeCanMove()

    protected override void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon")) {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }//end if
    }//end OnTriggerEnter()

    public override void TakeDamage(float damage) {
        animator.SetBool("Impact", true);
        Stats.Health -= damage;
        animator.SetBool("Impact", false);

        Debug.Log("General Soldier Health: " + Stats.Health);

        if (Stats.Health <= 0) {
            animator.SetBool("Die", true);
            base.Die();
            Destroy(gameObject);
        }//end if
    }//end TakeDamage()

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }//end OnDrawGizmosSelected()

    protected void CallEnemyAIStart() {
        base.Start();
    }//end CallEnemyAIStart()
}//end GeneralSoldierAI
