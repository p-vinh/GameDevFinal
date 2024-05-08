using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
        if (other.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= transform.parent.GetComponent<GeneralSoldierAI>().Stats.Damage;
        }
    }
}
