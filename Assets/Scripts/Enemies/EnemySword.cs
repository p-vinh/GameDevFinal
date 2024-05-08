using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
    public BoxCollider swordCollider;
    public GeneralSoldierAI generalSoldierAI;

    // Start is called before the first frame update
    void Start() {
        swordCollider = GetComponent<BoxCollider>();
        // Get the parent object of the sword
        generalSoldierAI = GetComponentInParent<GeneralSoldierAI>();
    }//end Start()

    // Update is called once per frame
    void Update() {
        
    }//end Update()

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerStats.Instance.Health -= generalSoldierAI.Stats.Damage;
            Debug.Log("Player Health: " + PlayerStats.Instance.Health);
        }//end if
    }//end OnTriggerEnter()
}//end EnemySword
