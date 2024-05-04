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
    private float chargeCooldownTime = 8f;
    private float chargeCooldownTimer = 0f;
    private float chargeTimer = 0f;
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
        Charging
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

    // private void ChooseNewRoamingTarget()
    // {
    //     // Define roaming area
    //     Vector3 minRoamPoint = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z - 10f);
    //     Vector3 maxRoamPoint = new Vector3(transform.position.x + 10f, transform.position.y, transform.position.z + 10f);
        
    //     // Choose random target position
    //     targetPosition = new Vector3(
    //         Random.Range(minRoamPoint.x, maxRoamPoint.x),
    //         transform.position.y,
    //         Random.Range(minRoamPoint.z, maxRoamPoint.z)
    //     );
    // }

    private void RoamAround()
    {
        // Set "Walk" animation trigger
        anim.SetBool("Walk",true);

        Vector3  roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        navMeshAgent.SetDestination(transform.position + roamDirection * 5f);

        // // Check if the enemy has reached the target position
        // if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        // {
        //     // Start the coroutine to wait for one second before choosing a new roaming target
        //     anim.SetBool("Idle",true);
        //     anim.SetBool("Walk",false);
        //     StartCoroutine(WaitForNewRoamingTarget());
        // }
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

            // Check if the player is within charge range and cooldown timer is up
            if (distanceToPlayer <= chargeRange && chargeCooldownTimer <= 0f)
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
        Vector3 direction = (spottedPlayerPosition - transform.position).normalized;
        anim.SetBool("Run",true);
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction * chargeSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, spottedPlayerPosition) < 0.1f)
        {
            // Set the "Idle" animation trigger to indicate the enemy is idling
            anim.SetBool("Idle",true);
            anim.SetBool("Run",false);
        
            // Transition back to the Roaming state and start the cooldown timer
            currentState = ChargeAIState.Roaming;
            chargeCooldownTimer = chargeCooldownTime;
            //ChooseNewRoamingTarget();
        }

        // Update charge timer
        chargeTimer -= Time.deltaTime;

        // If the charge duration is over, switch back to roaming and reset cooldown timer
        if (chargeTimer <= 0f)
        {
            currentState = ChargeAIState.Roaming;
            chargeCooldownTimer = chargeCooldownTime;
            //ChooseNewRoamingTarget();
        }
    }

    protected override void Attack()
    {
        Debug.Log($"{Stats.EnemyType} attacks the player with damage: {Stats.Damage}");
        PlayerStats.Instance.Health -= Stats.Damage;
    }

    public override void TakeDamage(float damage)
    {
        anim.SetBool("GetHit",true);
        Stats.Health -= damage;

        // Log damage and check if enemy's health is zero or below
        Debug.Log($"{Stats.EnemyType} takes damage. Current health: {Stats.Health}");

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