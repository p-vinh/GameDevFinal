using System.Collections;
using System.Collections.Generic;
using BlankStudio.Constants;
using UnityEngine;
using UnityEngine.AI;

public class MageAI : EnemyAI
{
    #region Variables

    [Header("Ranges")]
    public float attackRange = 6.0f;
    public float backAwayRange = 1f;

    private NavMeshAgent navMeshAgent;
    public LayerMask playerLayer;
    private Animator m_Animator;
    private LineRenderer lineRenderer;
    private bool isDead = false;
    private bool attackAnimationsDone = true;

    private enum State
    {
        Approach,
        Attack,
        BackingAway
    }

    private State state;
    private GameObject player;

    int Horizontal = 0;
    int Vertical = 0;

    public AudioSource attackSound;
    public override Constants.EnemyType Type => Constants.EnemyType.Mage;

    #endregion

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerLayer = LayerMask.GetMask("Player");
        lineRenderer = GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackSound = GetComponent<AudioSource>();

        state = State.Approach;

        Horizontal = Animator.StringToHash("Horizontal");
        Vertical = Animator.StringToHash("Vertical");

    }

    protected override void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        switch (state)
        {
            case State.Approach:
                lineRenderer.enabled = false;
                navMeshAgent.isStopped = false;

                if (distanceToPlayer <= attackRange)
                {
                    state = State.Attack;
                    navMeshAgent.isStopped = true;
                    navMeshAgent.SetDestination(transform.position);
                    UpdateAnimationValue(0, 0);
                }
                else
                {
                    navMeshAgent.speed = Stats.Speed;
                    navMeshAgent.SetDestination(player.transform.position);
                }
                UpdateAnimationValue(directionToPlayer.x, directionToPlayer.z);
                break;
            case State.Attack:
                if(attackAnimationsDone)
                {
                    attackSound.Play();
                    attackAnimationsDone = false;
                    Attack();
                }
                break;
            case State.BackingAway:
                Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
                navMeshAgent.isStopped = false;

                if (distanceToPlayer > backAwayRange)
                {
                    state = State.Attack;
                    attackAnimationsDone = true;
                }
                else
                {
                    navMeshAgent.speed = Stats.Speed;
                    navMeshAgent.SetDestination(transform.position - directionToPlayer);
                }

                UpdateAnimationValue(directionAwayFromPlayer.x, directionAwayFromPlayer.z);
                break;
        }
    }

    protected override void Attack()
    {
        transform.LookAt(player.transform);
        m_Animator.SetBool("Attack", true);
        Debug.Log("Mage attacks with damage: " + Stats.Damage);
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < backAwayRange)
        {
            state = State.BackingAway;
            lineRenderer.enabled = false;
        }
        else
        {
            PlayerStats.Instance.Health -= Stats.Damage;
            lineRenderer.enabled = true;
            Debug.Log(lineRenderer.positionCount+" Position");
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);   
        }
    }

    public void animationsDoneChange()
    {
        attackAnimationsDone = true;
        state = State.Approach;
    }

    public override void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        Debug.Log("Mage takes damage. Current health: " + Stats.Health);
        if (Stats.Health <= 0 && !isDead)
        {
            isDead = true;
            m_Animator.SetTrigger("Death");
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }

    public override void Die()
    {
        base.Die();
    }

    private void UpdateAnimationValue(float horizontalValue, float verticalValue)
    {
        m_Animator.SetBool("Attack", false);
        float time = 0.1f;
        float clampedHorizontal = Mathf.Clamp(horizontalValue, -1f, 1f);
        float clampedVertical = Mathf.Clamp(verticalValue, -1f, 1f);
        m_Animator.SetFloat(Horizontal, clampedHorizontal, time, Time.deltaTime);
        m_Animator.SetFloat(Vertical, clampedVertical, time, Time.deltaTime);
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, backAwayRange);
    }

}
