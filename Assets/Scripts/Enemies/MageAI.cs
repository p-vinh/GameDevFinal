using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAI : EnemyAI
{
    public float drainAmount = 1.0f;
    public float approachSpeed = 2.0f;
    public float backAwaySpeed = 3.0f;
    public SphereCollider drainRangeCollider;
    public SphereCollider tooCloseRangeCollider;
    public SphereCollider approachRangeCollider;
    private Rigidbody rb;
    public LayerMask playerLayer;
    public Animator m_Animator;
    private LineRenderer lineRenderer;

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
    public Vector3 roamDirection;
    public Vector3 lastKnownPlayerPosition;

    int Horizontal = 0;
    int Vertical = 0;

    public override string EnemyType => "Mage";

    protected override void Start()
    {
        bloodManager = FindObjectOfType<BloodManager>();
        m_Animator = GetComponent<Animator>();
        Stats = new EnemyStats(10, drainAmount, 5);
        player = GameObject.FindGameObjectWithTag("Player");
        state = State.Idle;
        playerLayer = LayerMask.GetMask("Player");
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        stateTimer = 0.0f;

        Horizontal = Animator.StringToHash("Horizontal");
        Vertical = Animator.StringToHash("Vertical");

    }

    protected override void Update()
    {
        stateTimer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (stateTimer >= 3f)
                {
                    state = State.Roaming;
                    stateTimer = 0f;
                    roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                }
                else if (Physics.CheckSphere(transform.position, approachRangeCollider.radius, playerLayer))
                {
                    state = State.Approach;
                    lastKnownPlayerPosition = player.transform.position;
                }

                UpdateAnimationValue(0, 0);
                break;
            case State.Roaming:
                if (stateTimer >= 3f)
                {
                    state = State.Idle;
                    stateTimer = 0f;
                }
                else if (Physics.CheckSphere(transform.position, approachRangeCollider.radius, playerLayer))
                {
                    state = State.Approach;
                    lastKnownPlayerPosition = player.transform.position;
                }
                else
                {
                    if (lastKnownPlayerPosition != Vector3.zero)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, lastKnownPlayerPosition, approachSpeed * Time.deltaTime);

                        if (transform.position == lastKnownPlayerPosition)
                            lastKnownPlayerPosition = Vector3.zero;
                    }
                    else
                        rb.velocity = roamDirection * approachSpeed;
                }

                if (roamDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(roamDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, approachSpeed * Time.deltaTime);
                }

                UpdateAnimationValue(roamDirection.x, roamDirection.z);
                break;
            case State.Approach:
                if (Physics.CheckSphere(transform.position, drainRangeCollider.radius, playerLayer))
                {
                    state = State.Attack;
                    lastKnownPlayerPosition = player.transform.position;
                }
                else if (!Physics.CheckSphere(transform.position, approachRangeCollider.radius, playerLayer))
                    state = State.Idle;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, approachSpeed * Time.deltaTime);
                    lastKnownPlayerPosition = player.transform.position;
                }

                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                if (directionToPlayer != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, approachSpeed * Time.deltaTime);
                }
                UpdateAnimationValue(directionToPlayer.x, directionToPlayer.z);
                break;
            case State.Attack:
                Vector3 attackDirectionToPlayer = (player.transform.position - transform.position).normalized;
                if (attackDirectionToPlayer != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(attackDirectionToPlayer, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                }
                rb.velocity = Vector3.zero;
                Attack();
                UpdateAnimationValue(0, 0);
                m_Animator.SetBool("Attack", true);
                break;
            case State.BackingAway:
                Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

                if (!Physics.CheckSphere(transform.position, drainRangeCollider.radius, playerLayer))
                {
                    state = State.Attack;
                }
                else
                    rb.velocity = directionAwayFromPlayer * -backAwaySpeed;

                if (directionAwayFromPlayer != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(directionAwayFromPlayer, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, backAwaySpeed * Time.deltaTime);
                }

                UpdateAnimationValue(directionAwayFromPlayer.x, directionAwayFromPlayer.z);
                break;
        }
    }

    public override void Attack()
    {
        Debug.Log("Mage attacks with damage: " + Stats.Damage);
        if (tooCloseRangeCollider.bounds.Contains(player.transform.position))
        {
            state = State.BackingAway;
            lineRenderer.enabled = false;
        }
        else if (!Physics.CheckSphere(transform.position, drainRangeCollider.radius, playerLayer))
        {
            state = State.Idle;
            lineRenderer.enabled = false;
        }
        else
        {
            PlayerStats.Instance.Health -= drainAmount;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }
    }

    public override void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        Debug.Log("Mage takes damage. Current health: " + Stats.Health);
        if (Stats.Health <= 0)
        {
            m_Animator.SetTrigger("Death");
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.Health -= Stats.Damage;
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);

        }

        if (other.gameObject.CompareTag("Wall"))
        {
            roamDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(PlayerStats.Instance.CurrentWeapon.Damage);
        }
    }

    public override void Die()
    {
        Debug.Log("Mage dies");
        base.Die();
        Destroy(gameObject);
    }

    public void UpdateAnimationValue(float horizontalValue, float verticalValue)
    {
        m_Animator.SetBool("Attack", false);
        float time = 0.1f;
        float clampedHorizontal = Mathf.Clamp(horizontalValue, -1f, 1f);
        float clampedVertical = Mathf.Clamp(verticalValue, -1f, 1f);
        m_Animator.SetFloat(Horizontal, clampedHorizontal, time, Time.deltaTime);
        m_Animator.SetFloat(Vertical, clampedVertical, time, Time.deltaTime);
    }
}
