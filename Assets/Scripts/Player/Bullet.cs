using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BloodSplashFX = null;

    private void Start()
    {
        StartCoroutine(DestroyProjectile(5f));
    }


    private void OnCollisionEnter(Collision other)
    {
        // Bullet
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
            StartCoroutine(DestroyProjectile(2f));
            StartCoroutine(BloodDriping());   
        }

        // Arrow
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= 5f;
        }
    }

    private IEnumerator BloodDriping() 
    {
        m_BloodSplashFX.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_BloodSplashFX.SetActive(false);
    }

    private IEnumerator DestroyProjectile(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
