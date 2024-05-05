using System.Collections;
using System.Collections.Generic;
using BlankStudio.Constants;
using UnityEngine;
using UnityEngine.AI;

public class MageAI : EnemyAI
{
    #region Variables

    [Header("Ranges")]
    public float attackRange = 7.0f;
    public float backAwayRange = 3f;
    public float approachRange = 20f;

    private NavMeshAgent navMeshAgent;
    public LayerMask playerLayer;
    private Animator m_Animator;
    private LineRenderer lineRenderer;
    private bool isDead = false;

    private enum State
    {
        Idle,
        Roaming,
        Approach,
        Attack,
        BackingAway
    }

    private State state;

    private GameObject player;
    private float stateTimer;
    private Vector3 roamDirection;
    private Vector3 lastKnownPlayerPosition;

    int Horizontal = 0;
    int Vertical = 0;

    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();

        state = State.Idle;
        stateTimer = 0.0f;

        Horizontal = Animator.StringToHash("Horizontal");
        Vertical = Animator.StringToHash("Vertical");

    }

    protected override void Update()
    {
        if (isDead) return;

        stateTimer += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        switch (state)
        {
            case State.Idle:
                navMeshAgent.isStopped = false;

                if (stateTimer >= 3f)
                {
                    state = State.Roaming;
                    stateTimer = 0f;
                    roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                }
                else if (distanceToPlayer <= approachRange)
                {
                    state = State.Approach;
                    lastKnownPlayerPosition = player.transform.position;
                }

                UpdateAnimationValue(0, 0);
                break;
            case State.Roaming:
                navMeshAgent.isStopped = false;

                if (stateTimer >= 3f)
                {
                    state = State.Idle;
                    stateTimer = 0f;
                }
                else if (distanceToPlayer <= approachRange)
                {
                    state = State.Approach;
                    lastKnownPlayerPosition = player.transform.position;
                }
                else
                {
                    if (lastKnownPlayerPosition != Vector3.zero)
                    {
                        navMeshAgent.speed = Stats.Speed;
                        navMeshAgent.SetDestination(lastKnownPlayerPosition);
                        if (Vector3.Distance(navMeshAgent.destination, transform.position) <= navMeshAgent.stoppingDistance)
                            lastKnownPlayerPosition = Vector3.zero;
                    }
                    else
                        navMeshAgent.SetDestination(transform.position + roamDirection * 5f);
                }

                SetRotation(roamDirection);
                UpdateAnimationValue(roamDirection.x, roamDirection.z);
                break;
            case State.Approach:
                navMeshAgent.isStopped = false;

                if (distanceToPlayer <= attackRange)
                {
                    state = State.Attack;
                    lastKnownPlayerPosition = player.transform.position;
                }
                else if (distanceToPlayer > approachRange)
                    state = State.Idle;
                else
                {
                    navMeshAgent.speed = Stats.Speed;
                    navMeshAgent.SetDestination(player.transform.position);
                    lastKnownPlayerPosition = player.transform.position;
                }


                SetRotation(directionToPlayer);
                UpdateAnimationValue(directionToPlayer.x, directionToPlayer.z);
                break;
            case State.Attack:
                navMeshAgent.speed = 0;
                navMeshAgent.isStopped = true;
                SetRotation(directionToPlayer);
                Attack();
                m_Animator.SetBool("Attack", true);
                break;
            case State.BackingAway:
                navMeshAgent.isStopped = false;
                Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

                if (distanceToPlayer > backAwayRange)
                {
                    state = State.Attack;
                }
                else
                {
                    navMeshAgent.speed = Stats.Speed;
                    navMeshAgent.SetDestination(transform.position - directionToPlayer);
                }

                SetRotation(directionAwayFromPlayer);
                UpdateAnimationValue(directionAwayFromPlayer.x, directionAwayFromPlayer.z);
                break;
        }
    }

    protected override void Attack()
    {
        Debug.Log("Mage attacks with damage: " + Stats.Damage);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= backAwayRange)
        {
            state = State.BackingAway;
            lineRenderer.enabled = false;
        }
        else if (distanceToPlayer > attackRange)
        {
            state = State.Idle;
            lineRenderer.enabled = false;
        }
        else
        {
            audioSource.Play();
            PlayerStats.Instance.Health -= Stats.Damage;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }
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

        if (other.gameObject.CompareTag("Wall"))
        {
            roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }

    }

    public override void Die()
    {
        Debug.Log("Mage dies");
        base.Die();
        Destroy(gameObject);
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

    private void SetRotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Stats.Speed * Time.deltaTime);
        }
    }
}
