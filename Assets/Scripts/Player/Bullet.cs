using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3.0f;

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);
    }


    private void OnCollisionEnter(Collision other)
    {
        // Bullet
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
            DestroyProjectile();
        }

        // Arrow
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= 2f;
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
