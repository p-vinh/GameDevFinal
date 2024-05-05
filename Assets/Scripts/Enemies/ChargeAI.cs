using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;
using UnityEngine.AI;

public class ChargeAI : EnemyAI
{
    public override Constants.EnemyType Type => Constants.EnemyType.Charger;

    // Charge variables

    private float chargeSpeed = 4f;
    private float chargeDuration = 3f;
    private float chargeTimer = 3f;
    private float chargeRange = 10f; 


    private ChargeAIState currentState = ChargeAIState.Roaming;
    private Vector3 targetPosition;
    private Vector3 spottedPlayerPosition;
    private GameObject player;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private float stateTimer;
    private Vector3 roamDirection;

    // Enumeration for enemy state
    private enum ChargeAIState
    {
        Roaming,
        Charging,
        Idle
    }

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 4;
        stateTimer = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
        // Update cooldown timer
        stateTimer += Time.deltaTime;

        // Handle state transitions
        switch (currentState)
        {
            case ChargeAIState.Idle:
                navMeshAgent.isStopped = true;
                anim.SetBool("Idle",true);
                break;
            case ChargeAIState.Roaming:
                print("Roaming");

                if (stateTimer >= 3f)
                {
                    stateTimer = 0f;
                }
                else
                {
                    RoamAround();
                }
                CheckForPlayerInRange();
                break;
            case ChargeAIState.Charging:
                print("Charging");
                ChargeAtPlayer();
                break;
        }
    }

    private void RoamAround()
    {
        // Set "Walk" animation trigger
        anim.SetBool("Walk",true);

        Vector3  roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        navMeshAgent.SetDestination(transform.position + roamDirection * 5f);
    }


    // Coroutine to wait for one second before choosing a new roaming target
    private IEnumerator WaitForNewRoamingTarget()
    {
        anim.SetBool("Idle",true);
        // Wait for one second
        yield return new WaitForSeconds(5f);
    }

    private void CheckForPlayerInRange()
    {
        if (player != null)
        {
            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distanceToPlayer <= chargeRange)
            {
                // Store player's position and switch to Charging state
                spottedPlayerPosition = player.transform.position;
                currentState = ChargeAIState.Charging;
                chargeTimer = chargeDuration;
            }
        }
    }

    private void ChargeAtPlayer()
    {
        // Move towards player
        anim.SetBool("Walk",false);
        anim.SetBool("Run",true);
        navMeshAgent.speed = chargeSpeed;
        
        navMeshAgent.SetDestination(spottedPlayerPosition);

        // Update charge timer
        chargeTimer -= Time.deltaTime;

        // If the charge duration is over, switch back to roaming and reset cooldown timer
        if (chargeTimer <= 0f)
        {
            print("CHANGING");
            anim.SetBool("Run",false);
            anim.SetBool("Idle",true);
            currentState = ChargeAIState.Idle;
            chargeTimer = chargeDuration; //reset charagetimer
            navMeshAgent.speed = 3f;
            Invoke("roamingAgain",3f);
        }
    }

    private void roamingAgain()
    {
        navMeshAgent.isStopped = false;
        currentState = ChargeAIState.Roaming;
    }
    protected override void Attack()
    {
        //Debug.Log($"{Stats.EnemyType} attacks the player with damage: {Stats.Damage}");
        PlayerStats.Instance.Health -= Stats.Damage;
    }

    public override void TakeDamage(float damage)
    {
        anim.SetBool("GetHit",true);
        Stats.Health -= damage;

        // Log damage and check if enemy's health is zero or below
        //Debug.Log($"{Stats.EnemyType} takes damage. Current health: {Stats.Health}");

        // If health is zero or below, trigger the death sequence
        if (Stats.Health <= 0)
        {
            Die();
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Attack();
            PlayerStats.Instance.Health -= Stats.Damage;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            //ChooseNewRoamingTarget();
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }

    public override void Die()
    {
        anim.SetBool("Die",true);
        Debug.Log("Charge dies");
        base.Die();
        Destroy(gameObject);
    }
}