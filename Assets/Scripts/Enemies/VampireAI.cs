using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;

public class VampireAI : GeneralSoldierAI
{
    public float healAmount = 10;
    public override Constants.EnemyType Type => Constants.EnemyType.Vampire;
    public bool hit;

    protected override void Start()
    {
        base.Start();
        attackRange = 2f;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        base.Attack();
        Heal();
    }

    private void Heal()
    {
        Stats.Health += healAmount;
    }

    public override void TakeDamage(float damage) 
    {
        Stats.Health -= damage;
        Debug.Log("Vampire Health: " + Stats.Health);

        if (Stats.Health <= 0) {
            base.Die();
            Destroy(gameObject);
        }
    }
}

