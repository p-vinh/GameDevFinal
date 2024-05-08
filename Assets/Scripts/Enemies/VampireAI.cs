using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlankStudio.Constants;

public class VampireAI : GeneralSoldierAI
{
    public float healAmount = 10;
    private float MaxHealth;
    public override Constants.EnemyType Type => Constants.EnemyType.Vampire;
    public bool hit;

    protected override void Start()
    {
        base.CallEnemyAIStart();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        mesh = GetComponent<NavMeshAgent>();
        mesh.speed = Stats.Speed;

        attackSound = GetComponent<AudioSource>();
        sword = GetComponentInChildren<BoxCollider>();
        MaxHealth = Stats.Health;
    }

    protected override void Update()
    {
        if (player == null) return;
        base.Update();
    }

    protected override void Attack()
    {
        base.Attack();
        Heal();
    }

    private void Heal()
    {
        if (Stats.Health + healAmount > MaxHealth)
        {
            Stats.Health = MaxHealth;
            return;
        }

        Stats.Health += healAmount;
    }

    public override void TakeDamage(float damage) 
    {
        Stats.Health -= damage;
        Debug.Log("Vampire Health: " + Stats.Health);

        if (Stats.Health <= 0) {
            base.Die();
        }
    }
}

