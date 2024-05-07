using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;

public class EnemyStats
{
    public float Health { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
    public Constants.EnemyType EnemyType { get; set; }
    public GameObject EnemyPrefab { get; set; }

    public EnemyStats(float health, float damage, float speed, Constants.EnemyType enemyType, GameObject enemyPrefab)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
        EnemyType = enemyType;
        EnemyPrefab = enemyPrefab;
    }
}

public abstract class EnemyAI : MonoBehaviour
{
    public EnemyData Data;
    public EnemyStats Stats { get; private set; }

    public abstract Constants.EnemyType Type { get; }
    public EnemyData.EnemyProperties Properties;


    public BloodManager bloodManager;

    protected virtual void Start()
    {
        bloodManager = FindObjectOfType<BloodManager>();
        Properties = Data.EnemiesData.EnemyPropertiesList.Find(x => x.EnemyType == Type);
        Debug.Log(Properties.Health + " " + Properties.Damage + " " + Properties.Speed + " " + Properties.EnemyType + " " + Properties.EnemyPrefab);

        Stats = new EnemyStats
        (
            Properties.Health,
            Properties.Damage,
            Properties.Speed,
            Properties.EnemyType,
            Properties.EnemyPrefab
        );
    }

    protected virtual void Update()
    {
        // Update enemy AI here
    }

    // Define other common methods for all enemies here

    protected virtual void Attack()
    {
        Debug.Log("Enemy attacks with damage: " + Stats.Damage);
    }

    public virtual void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        Debug.Log("Enemy takes damage. Current health: " + Stats.Health);
        if (Stats.Health <= 0)
        {
            Die();
        }
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= Stats.Damage;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            // Do something when enemy collides with wall
        }
    }  

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }

    public virtual void Die()
    {
        bloodManager.OnEnemyDeath(gameObject.transform);
        Destroy(gameObject);
    }
}