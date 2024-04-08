using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAI : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public GameObject enemyBullet;
    public int enemyHealth;
    public Transform spawnPoint;

    //Attacking Variables
    public float fireDelaySeconds;
    public bool canFire = true;
    public float bulletSpeed;

    //State Varables
    public float sightRange, attackRange;
    public bool playerInSightRange, playerinAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        playerInSightRange = Vector3.Distance(player.position, transform.position) <= sightRange;
        playerinAttackRange = Vector3.Distance(player.position, transform.position) <= attackRange;

        if(playerInSightRange && !playerinAttackRange) ChasePlayer();
        if(playerInSightRange && playerinAttackRange) AttackPlayer();
       
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        print("Attack!");

        //Enemy doesn't move when in attack range
        enemy.SetDestination(transform.position);
        transform.LookAt(player);

        if(canFire)
        {

            //Attack
            GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, Quaternion.identity) as GameObject;
            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bulletObj, 4f);

            canFire = false;
            Invoke(nameof(ResetAttack), fireDelaySeconds);
        }
    }

    private void ResetAttack()
    {
        canFire = true;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
