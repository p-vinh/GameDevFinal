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


    public BloodType bloodType { get; private set; }
    public static PlayerStats Instance { get; private set; }
    public float Health { get; set; }
    public float MaxHealth { get; private set; }
    public float MovementSpeed { get; private set; }
    public Weapon CurrentWeapon { get; private set; }
    public Constants.WeaponType CurrentWeaponType { get; private set; }
    public Vector3 LastDoorEntered { get; set; }

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
        MovementSpeed = 10;
        bloodType = BloodType.A; // Difficulty setting
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
        CurrentWeapon = new Weapon("Sword", 10f, 2f, 1f);
        CurrentWeaponType = Constants.WeaponType.Sword;
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
