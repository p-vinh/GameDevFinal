using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            
            Rigidbody hit = other.gameObject.GetComponent<Rigidbody>();
            if(hit != null) //if hit has a rigidbody
            {
                Vector3 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode.Impulse);
                    //normalized = vector can only have 1
            }

            print("Added knockback!");
        }
    }
}