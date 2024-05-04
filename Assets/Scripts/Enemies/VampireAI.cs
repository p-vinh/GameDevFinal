using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;

public class VampireAI : GeneralSoldierAI
{
    public float healAmount = 10;
    public override Constants.EnemyType Type => Constants.EnemyType.Vampire;

    protected override void Start()
    {
        base.Start();
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
}

