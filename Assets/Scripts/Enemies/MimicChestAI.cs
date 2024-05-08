using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlankStudio.Constants;

public class MimicChestAI : EnemyAI
{
    public NavMeshAgent enemy;
    public Transform playerTransform; 

    public AudioSource attackSound; // Code added by Abby (Sound Engineer)

    private bool playerInAttackRange;
    private bool playerInChaseRange;
    public float attackRange;
    public float chaseRange = 20f;

    public override Constants.EnemyType Type => Constants.EnemyType.Mimic;
    private Animator animator;
    private State state;
    private GameObject playerGameObject; 
    private bool finishedAttackAnim = true;

    public enum State
    {
        IdleResting, 
        Attack, 
        Chasing 
    }

    protected override void Start()
    {
        //Initialize stats
        base.Start();

        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
        {
            playerTransform = playerGameObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene!");
        }

        enemy = GetComponent<NavMeshAgent>();
        //state = State.IdleResting;
        animator = GetComponent<Animator>();
        ChasePlayer();
    }

    protected override void Update() 
    {
        if (playerGameObject != null)
        {
               //to check if the player is within chasing range
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            playerInChaseRange = distanceToPlayer <= chaseRange; 
            playerInAttackRange = Vector3.Distance(playerTransform.position, transform.position) < attackRange;


            // if (distanceToPlayer <= chaseRange)
            // {
            // enemy.SetDestination(playerTransform.position);
            
            //     if(distanceToPlayer <= attackRange)
            //     {
            //         state = State.Attack;
            //         if(finishedAttackAnim)
            //         {
            //             finishedAttackAnim = false;
            //             Attack();
            //         }
            //     } else 
            //     {
            //         state = State.IdleResting;
            //     }
            // } else 
            //     {
            //         state = State.IdleResting;
            //     }
        

            switch(state)
            {
                case State.IdleResting:
                    if(playerInAttackRange)
                    {
                        state = State.Attack;
                    }
                    if(playerInChaseRange)
                    {
                        state = State.Chasing;
                    }
                    break;
                case State.Attack:
                    if(finishedAttackAnim)
                    {
                        finishedAttackAnim = false;
                        Attack();
                    }
                    break;
                case State.Chasing:
                    if(playerInChaseRange)
                    {
                        ChasePlayer();
                    }
                    break;
            }
        }
        else
        {
            print("Error");
        }
    }

    protected override void Attack()
    {
        playerTransform = playerGameObject.transform; //Get player info
        transform.LookAt(playerTransform.position); //Look at player

        if (playerTransform != null)
        {
            animator.SetBool("Chasing", false);
            animator.SetBool("Attacking",true);
            PlayerStats.Instance.Health -= Stats.Damage;
            print("Player took damage from chest:" + PlayerStats.Instance.Health);
        }
    }

    private void ChasePlayer()
    {
        if(playerTransform != null)
        {
            //animator.
            enemy.SetDestination(playerTransform.position);
            animator.SetBool("Attacking",false);
            animator.SetBool("Chasing", true);
        }
    }
    public void canAttackAgain()
    {
        finishedAttackAnim = true;
        animator.SetBool("Attacking",false);
        animator.SetBool("Chasing", false);
        state = State.IdleResting;
    }
    
    public override void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        animator.SetTrigger("Hurt");
        Debug.Log("Mimic is hurting. Current health: " + Stats.Health);
        if (Stats.Health <= 0)
        {
            Die();
        }
    }
    
    public override void Die()
    {
        animator.SetTrigger("Dead");
        base.Die();
    }

    protected override void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            //
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    } 

    // private void SetAnimationState(string state){
    //     //to ensure that no lerftover animations are happening
    //     //animator.ResetTrigger("IdleHostile");
    //     animator.ResetTrigger("IdleResting");
    //     animator.ResetTrigger("Attacking");
    //     animator.ResetTrigger("Chasing");
    //     //animator.ResetTrigger("Dead");

    // }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }   
}
