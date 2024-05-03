using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public float Health { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }

    public EnemyStats(float health, float damage, float speed)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
    }
}

public abstract class EnemyAI : MonoBehaviour
{
    public EnemyData EnemyData;
    public EnemyStats Stats { get; set; }
    public abstract string EnemyType { get; }
    public BloodManager bloodManager;

    protected virtual void Start()
    {
        bloodManager = FindObjectOfType<BloodManager>();
        Stats = new EnemyStats(100f, 10f, 5f); // Default stats for all enemies
    }

    protected virtual void Update()
    {
        // Update enemy AI here
    }

    // Define other common methods for all enemies here

    public virtual void Attack()
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

        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }

    public virtual void Die()
    {
        Debug.Log("Enemy dies");
        bloodManager.OnEnemyDeath();
    }
}