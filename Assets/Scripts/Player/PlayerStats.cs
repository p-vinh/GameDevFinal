using System.Collections;
using System.Collections.Generic;
using BlankStudio.Constants;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


// We can seperate the weapoon class so we can have a heirarchy of classes for different types of weapons
public class Weapon
{
    public string Name { get; set; }
    public float Damage { get; set; }
    public float Range { get; set; }
    public float Speed { get; set; }

    public Weapon(string name, float damage, float range, float speed)
    {
        Name = name;
        Damage = damage;
        Range = range;
        Speed = speed;
    }
}



public class PlayerStats : MonoBehaviour
{
    // Access the stats by using PlayerStats.Instance.{StatName}
    public enum BloodType
    {
        A,
        B,
        AB,
        O
    }


    public BloodType bloodType { get; set; }
    public static PlayerStats Instance { get; private set; }
    public float Health { get; set; }
    public float MaxHealth { get; private set; }
    public float MovementSpeed { get; set; }
    public Weapon CurrentWeapon { get; set; }
    public Constants.WeaponType CurrentWeaponType { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Health = 100;
        MaxHealth = 100;
        MovementSpeed = 4;
        CurrentWeaponType = Constants.WeaponType.Sword;
        CurrentWeapon = new Weapon("Sword", 10f, 2f, 0.2f);
    }

    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    public void ResetStats()
    {
        Health = MaxHealth;
        MovementSpeed = 4;
        CurrentWeapon = new Weapon("Sword", 10f, 2f, 1f);
        CurrentWeaponType = Constants.WeaponType.Sword;
    }

    public int increaseRandomStat()
    {
        int randomStat = Random.Range(0, 3);
        switch (randomStat)
        {
            case 0:
                MaxHealth += 10;
                break;
            case 1:
                MovementSpeed += 1;
                break;
            case 2:
                CurrentWeapon.Damage += 5;
                break;
        }

        return randomStat;
    }

    public void Die()
    {
        Debug.Log("Player dies");
        // Play death animation
        // Show game over screen
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);
    }
}
