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

    public bool playerInSightRange, playerInAttackRange;
    public SphereCollider sightRangeCollider;
    public SphereCollider attackRangeCollider;
    public LayerMask playerLayer;
    public override Constants.EnemyType Type => Constants.EnemyType.Mimic;
    private Animator animator;
    private State state;
    private GameObject playerGameObject; 

    public enum State
    {
        IdleResting, 
        IdleHostile, 
        Attack 
    }

    protected override void Start()
    {
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
        playerLayer = LayerMask.GetMask("Player");
        animator = GetComponent<Animator>();
    }

    protected override void Update() 
    {
        playerInSightRange =  IsPlayerInSightRange();
        playerInAttackRange = IsPlayerInAttackRange();

       switch(state){
        case State.IdleResting:
            if(playerInSightRange){
                state = State.IdleHostile;
                SetAnimationState("IdleResting");
                transform.LookAt(playerTransform.position);
            }
            break;
        case State.IdleHostile:
            if (!playerInSightRange)
            {
                SetAnimationState("IdleHostile");
                state = State.IdleResting;
            }
            else if (playerInAttackRange)
            {
                SetAnimationState("Attacking");
                state = State.Attack;
            }
            break;
        case State.Attack:
            if (!playerInSightRange)
            {
                SetAnimationState("IdleResting");
                state = State.IdleResting;
            }
            else if (!playerInAttackRange)
            {
                SetAnimationState("IdleHostile");
                state = State.IdleHostile;
            }
            else
            {
                SetAnimationState("Attacking");
                Attack();
            }
            break;
       }
    }

    bool IsPlayerInSightRange()
    {
        if (playerTransform != null)
        {
            return sightRangeCollider.bounds.Contains(playerTransform.position);
        }
        return false;
    }

    bool IsPlayerInAttackRange()
    {
        if (playerTransform != null)
        {
            return attackRangeCollider.bounds.Contains(playerTransform.position);
        }
        return false;
    }
    
    protected override void Attack()
    {
        if (playerTransform != null)
        {
            Debug.Log("Player is too close to mimic! It attacks with damage: " + Stats.Damage);
            SetAnimationState("Attacking");
            PlayerStats.Instance.Health -= Stats.Damage;
        }
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
        SetAnimationState("Dead");
        Debug.Log("Mimic dies");
        base.Die();
        Destroy(gameObject);
    }

    protected override void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //
            PlayerStats.Instance.Health -= Stats.Damage;
        }
        if (other.gameObject.CompareTag("Projectile"))
        {
            //
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }    

    private void SetAnimationState(string state){
        //to ensure that no lerftover animations are happening
        animator.ResetTrigger("IdleHostile");
        animator.ResetTrigger("IdleResting");
        animator.ResetTrigger("Attacking");
        animator.ResetTrigger("Dead");

        animator.SetTrigger(state);
    }
}
