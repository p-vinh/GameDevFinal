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


    private ChargeAIState currentState = ChargeAIState.Idle;
    private Vector3 targetPosition;
    private Vector3 spottedPlayerPosition;
    private GameObject player;
    private Animator anim;
    public AudioSource attackSound;
    private NavMeshAgent navMeshAgent;
    private Vector3 roamDirection;
    private bool currentlyCharging;

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
    }

    protected override void Update()
    {
        base.Update();

        // Handle state transitions
        switch (currentState)
        {
            case ChargeAIState.Idle:
                CheckForPlayerInRange();
                break;
            case ChargeAIState.Charging:
                ChargeAtPlayer();
                break;
        }
    }
    private void CheckForPlayerInRange()
    {
        if (player != null)
        {
            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= chargeRange)
            {
                // Store player's current position and switch to Charging state
                spottedPlayerPosition = player.transform.position;
                currentState = ChargeAIState.Charging;
                chargeTimer = chargeDuration;
            }
        }
    }

    private void ChargeAtPlayer()
    {


        // Update charge timer
        chargeTimer -= Time.deltaTime;

        if (!currentlyCharging)
        {
            navMeshAgent.SetDestination(spottedPlayerPosition);
            anim.SetBool("Run", true);
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = chargeSpeed;
            navMeshAgent.acceleration = 100;

            if (Vector3.Distance(transform.position, spottedPlayerPosition) < 0.4f || chargeTimer <= 0f)
            {
                currentlyCharging = true;
                anim.SetBool("Run", false);
                navMeshAgent.isStopped = true;
                chargeTimer = chargeDuration; //reset charagetimer
                navMeshAgent.speed = 3f;
                Invoke("backToIdle", 2f);
            }
        }


    }
    private void backToIdle() //gives time before looking for player again
    {
        currentlyCharging = false;
        currentState = ChargeAIState.Idle;
    }

    public override void TakeDamage(float damage)
    {
        navMeshAgent.isStopped = true;
        anim.SetBool("Run", false);
        Stats.Health -= damage;
        Debug.Log("Charger Enemy Health:" + Stats.Health);

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
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= Stats.Damage;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            anim.SetBool("Idle", true);
            navMeshAgent.isStopped = false;
            currentState = ChargeAIState.Idle;
        }
    }

}