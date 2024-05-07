using System.Collections;
using System.Collections.Generic;
using BlankStudio.Constants;
using UnityEngine;
using UnityEngine.AI;

public class RangeAI : EnemyAI
{
    public NavMeshAgent enemy;
    public Transform player;
    public GameObject enemyBullet;
    public Transform spawnPoint;
    public AudioSource attackSound1; // Code added by Abby (Sound Engineer)
    public AudioSource attackSound2; // Code added by Abby (Sound Engineer)
    public AudioSource attackSound3; // Code added by Abby (Sound Engineer)
    Animator anim;

    //Temporary variables, change the stats of enemy
    // TODO Change to scriptable object
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
    public override Constants.EnemyType Type => Constants.EnemyType.Ranger;

    //Private Varables
    private enum State
    {
        Chase,
        Idle,
        Attack
    }
    private State state;
    private bool canWalk = true;
    private Vector3 playerCurrentPosition;

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.Idle;
    }

    private void FixedUpdate()
    {

        if(player != null)
        {
            playerinAttackRange = Vector3.Distance(player.position, transform.position) < attackRange;

            if(canWalk)
            {
                playerCurrentPosition = player.position;
                ChasePlayer();
            }
            if (playerinAttackRange) AttackPlayer();
        }

    }

    private void ChasePlayer()
    {
        enemy.isStopped = false;
        anim.SetBool("Walking", true);
        anim.SetBool("Attacking", false);
        state = State.Chase;
        enemy.SetDestination(playerCurrentPosition);
    }

    private void AttackPlayer()
    {
        canWalk = false;
        state = State.Attack;
        anim.SetBool("Attacking", true);

        //Enemy doesn't move when in attack range
        transform.LookAt(player);
        enemy.SetDestination(transform.position);

        if (canFire)
        {
            Vector3 direction = (player.position - spawnPoint.transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            //Attack
            GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, rotation) as GameObject;
            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bulletObj, 4f);

            canFire = false;
            Invoke(nameof(ResetAttack), fireDelaySeconds);
            attackSound2.Play(); // Code added by Abby (Sound Engineer)
        }
    }

    public void changeCanWalk()
    {
        canWalk = true;
    }

    private void ResetAttack()
    {
        canFire = true;
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
        Debug.Log("Range Enemy Health:"  + Stats.Health);

        if (Stats.Health <= 0) 
        {
            base.Die();
            Destroy(this.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
