using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAI : EnemyAI
{
    public NavMeshAgent enemy;
    public Transform player;
    public GameObject enemyBullet;
    public Transform spawnPoint;
    Animator anim;
    public Animator bowAnim;

    //Temporary variables, change the stats of enemy
    public int health;
    public float damage;
    public float speed;
    
    //Attacking Variables
    public float fireDelaySeconds;
    public bool canFire = true;
    public float bulletSpeed;

    //State Varables
    public float sightRange, attackRange;
    public bool playerInSightRange, playerinAttackRange;
    public override string EnemyType => "Range";

    //Private Varables
    private enum State
    {
        Chase,
        Idle,
        Attack
    }
    private State state;
    private void Awake()
    {
        bloodManager = FindObjectOfType<BloodManager>();
        Stats = new EnemyStats(health, damage, speed);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.Idle;
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        playerInSightRange = Vector3.Distance(player.position, transform.position) <= sightRange;
        playerinAttackRange = Vector3.Distance(player.position, transform.position) < attackRange;

        if(playerInSightRange && !playerinAttackRange) ChasePlayer();
        if(playerInSightRange && playerinAttackRange) AttackPlayer();
        if(!playerInSightRange && !playerinAttackRange) StopEnemy();
       
    }

    private void ChasePlayer()
    {
        enemy.isStopped = false;
        bowAnim.SetBool("walking",true);
        anim.SetBool("Walking",true);
        anim.SetBool("Attacking",false);
        bowAnim.SetBool("attacking",false);
        state = State.Chase;
        enemy.SetDestination(player.position);
    }

    private void StopEnemy()
    {
        enemy.isStopped = true;
        state = State.Idle;
        anim.SetBool("Walking", false);
        bowAnim.SetBool("walking",false);
    }

    private void AttackPlayer()
    {
        state = State.Attack;
        anim.SetBool("Attacking",true);
        bowAnim.SetBool("attacking",true);

        //Enemy doesn't move when in attack range
        transform.LookAt(player);
        enemy.SetDestination(transform.position);

        if(canFire)
        {
            //Attack
            GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, player.rotation) as GameObject;
            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bulletObj, 4f);

            canFire = false;
            Invoke(nameof(ResetAttack), fireDelaySeconds);
        }
    }

    private void ResetAttack()
    {
        canFire = true;
    }

     protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
