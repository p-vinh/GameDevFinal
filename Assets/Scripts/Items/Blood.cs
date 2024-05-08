using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] private float healthAmount = 10.0f;
    public AudioSource audioSource; // Code added by Abby (Sound Engineer)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            if (audioSource.isPlaying == false) {
                audioSource.Play(); // Code added by Abby (Sound Engineer)
            }//end if
            
            if (PlayerStats.Instance.Health + healthAmount > PlayerStats.Instance.MaxHealth)
            {
                PlayerStats.Instance.Health = PlayerStats.Instance.MaxHealth;
                Destroy(gameObject);
                return;
            }

            PlayerStats.Instance.Health += healthAmount;
            Destroy(gameObject);
        }
    }
}
