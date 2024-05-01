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
    public LayerMask playerLayer;
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

    public override string EnemyType => "Mage";

    protected override void Start()
    {
        bloodManager = FindObjectOfType<BloodManager>();
        Stats = new EnemyStats(10, drainAmount, 5);
        player = GameObject.FindGameObjectWithTag("Player");
        state = State.Idle;
        playerLayer = LayerMask.GetMask("Player");
        lineRenderer = GetComponent<LineRenderer>();
        stateTimer = 0.0f;
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
                        transform.position += roamDirection * approachSpeed * Time.deltaTime;
                }
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
                break;
            case State.Attack:
                Attack();
                break;
            case State.BackingAway:
                if (!Physics.CheckSphere(transform.position, drainRangeCollider.radius, playerLayer))
                {
                    state = State.Attack;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -backAwaySpeed * Time.deltaTime);
                }
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
            Die();
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
        base.Die(); // Call the base class method for blood drop logic
        Destroy(gameObject);
    }

}
