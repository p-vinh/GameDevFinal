using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;
using UnityEngine.AI;

public class ChargeAI : EnemyAI
{
    public override Constants.EnemyType Type => Constants.EnemyType.Charger;

    // Charge variables

    public float chargeSpeed = 5f; //charge speed
    private float chargeDuration = 5f; //max chargetimer
    private float chargeTimer = 5f; //current chargetimer
    private float chargeRange = 10f;  //range to where enemy will charge 


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
        base.Start();
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
                ChargeAtPlayer();
                break;
        }
    }

    private void RoamAround()
    {
        // Set "Walk" animation trigger
        anim.SetBool("Walk",true);
        navMeshAgent.acceleration = 8f;

        Vector3  roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        navMeshAgent.SetDestination(transform.position + roamDirection * 5f);
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
        navMeshAgent.isStopped = false;
        // Move towards player
        anim.SetBool("Walk",false);
        anim.SetBool("Run",true);
        navMeshAgent.speed = chargeSpeed;
        navMeshAgent.acceleration = 100;
        
        navMeshAgent.SetDestination(spottedPlayerPosition);

        // Update charge timer
        chargeTimer -= Time.deltaTime;

        //Continuosly goes forward,even if they meet player position
        if (Vector3.Distance(transform.position, spottedPlayerPosition) < 0.1f && chargeTimer > 0f)
        {
            anim.SetBool("Run",false);
            anim.SetBool("Idle",true);
            currentState = ChargeAIState.Idle;
            chargeTimer = chargeDuration; //reset charagetimer
            navMeshAgent.speed = 3f;
            Invoke("roamingAgain",3f);
        }

        if (chargeTimer <= 0f)
        {
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

    public override void TakeDamage(float damage) 
    {
        anim.SetBool("Idle",true);
        navMeshAgent.isStopped = true;
        Stats.Health -= damage;
        Debug.Log("Charger Enemy Health:"  + Stats.Health);

        if (Stats.Health <= 0) 
        {
            base.Die();
            Destroy(this.gameObject);
        }
    }
    

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }
    // protected override void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         PlayerStats.Instance.Health -= Stats.Damage;
    //     }

    //     if (other.gameObject.CompareTag("Wall"))
    //     {
    //         anim.SetBool("Run",false);
    //         anim.SetBool("Idle",true);
    //         anim.SetBool("Walk",false);
    //         navMeshAgent.isStopped = false;
    //         currentState = ChargeAIState.Roaming;

    //     }

    //     if (other.gameObject.CompareTag("Projectile")) //Go back to this
    //     {
    //         TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
    //     }
    // }

    public override void Die()
    {
        anim.SetBool("Die",true);
        Debug.Log("Charge dies");
        base.Die();
        Destroy(gameObject);
    }
}