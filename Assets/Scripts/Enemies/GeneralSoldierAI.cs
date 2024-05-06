using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlankStudio.Constants;

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
    public float sightRange = 8.0f; 
    public float attackRange = 3.0f;
    public override Constants.EnemyType Type => Constants.EnemyType.GeneralSoldier;
    private float distanceToPlayer;
    private bool canMove = true;


    // Setup Variables
    protected override void Start() {
        // Set up the enemy stats
        base.Start();
        // Set up the enemy AI
        mesh = GetComponent<NavMeshAgent>();
        mesh.speed = Stats.Speed;
        enemy = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        sword = GetComponentInChildren<BoxCollider>();
        state = State.Idle;
    }//end Start()

    // Update is called once per frame
    protected override void Update() 
    {
        

        bool playerInSightRange = Vector3.Distance(player.position, transform.position) <= sightRange;
        bool playerinAttackRange = Vector3.Distance(player.position, transform.position) < attackRange;

        // Move to Player if in sight range
        if (playerInSightRange && !playerinAttackRange && canMove) //chase player
        {
            MoveToPlayer();
        } 
        else if (playerInSightRange && playerinAttackRange) 
        {
            mesh.SetDestination(enemy.position);
            Attack();
        } 
        else if(!playerInSightRange && !playerinAttackRange)
        {
            StopMoving();
        }
    }

    // Universal Soldier AI Methods //
    // Move to Player AI
    void MoveToPlayer() 
    {
        mesh.isStopped = false;
        animator.SetBool("Walking", true);
        mesh.SetDestination(player.position);
        state = State.Chase;
    }

    void StopMoving() 
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Walking", false);
        animator.SetBool("Idle", true);
        mesh.SetDestination(enemy.position);
        mesh.isStopped = true;
        state = State.Idle;
    }

    protected override void Attack() {
        // TODO: Set up the attack animation with the sword and if the sword object hits the player then and only then the player takes damage
        canMove = false;
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);

        state = State.Attack;
        animator.SetBool("Attack", true);

        Debug.Log("General Soldier Attack Player");
        if (sword.bounds.Intersects(player.GetComponent<Collider>().bounds)) 
        {
            PlayerStats.Instance.Health -= Stats.Damage;
        }
    }

    void changeCanMove() 
    {
        canMove = true;
        StopMoving();
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }


    public override void TakeDamage(float damage) 
    {
        Stats.Health -= damage;
        Debug.Log("General Soldier Health: " + Stats.Health);

        if (Stats.Health <= 0) {
            base.Die();
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }//end OnDrawGizmosSelected()
}//end GeneralSoldierAI
