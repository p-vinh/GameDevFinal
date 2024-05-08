using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] private float healthAmount = 10.0f;
    public AudioSource audioSource;
    void Start()
    {
        Destroy(gameObject, 10.0f);
    }
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player healed for " + healthAmount + " health.");
            
            if (PlayerStats.Instance.Health + healthAmount > PlayerStats.Instance.MaxHealth)
            {
                PlayerStats.Instance.Health = PlayerStats.Instance.MaxHealth;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                Destroy(gameObject);
                return;
            }

            PlayerStats.Instance.Health += healthAmount;
            Destroy(gameObject);
        }
    }
}
