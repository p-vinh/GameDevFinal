using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlankStudio.Constants;

public class MimicChestAI : EnemyAI
{
    public NavMeshAgent enemy;
    public Transform playerTransform; 
    public float attackRange;
    public bool playerInAttackRange;
    public override Constants.EnemyType Type => Constants.EnemyType.Mimic;
    private Animator animator;
    private State state;
    private GameObject playerGameObject; 
    private bool finishedAttackAnim = true;

    public enum State
    {
        IdleResting, 
        Attack 
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
        state = State.IdleResting;
        animator = GetComponent<Animator>();
    }

    protected override void Update() 
    {

        if (playerGameObject != null)
        {
            playerInAttackRange = Vector3.Distance(playerTransform.position, transform.position) < attackRange;

            switch(state)
            {
                case State.IdleResting:
                    if(playerInAttackRange)
                    {
                        state = State.Attack;
                    }
                    break;
                case State.Attack:
                    if(finishedAttackAnim)
                    {
                        finishedAttackAnim = false;
                        Attack();
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
            animator.SetBool("Attacking",true);
            PlayerStats.Instance.Health -= Stats.Damage;
            print("Player took damage from chest:" + PlayerStats.Instance.Health);
        }
    }

    public void canAttackAgain()
    {
        finishedAttackAnim = true;
        animator.SetBool("Attacking",false);
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
        animator.SetTrigger("Died");
        Debug.Log("Mimic dies");
        base.Die();
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            //
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    } 

    protected override void OnCollisionEnter(Collision other)
    {

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }   
}
