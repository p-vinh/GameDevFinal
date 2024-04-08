using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAI : MonoBehaviour
{
    public float drainAmount = 1.0f;
    public float approachSpeed = 2.0f;
    public float backAwaySpeed = 3.0f;
    public SphereCollider drainRangeCollider;
    public SphereCollider tooCloseRangeCollider;
    public SphereCollider approachRangeCollider;
    public LayerMask playerLayer;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = State.Idle;
        playerLayer = LayerMask.GetMask("Player");
        stateTimer = 0.0f;
    }

    void Update()
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
                if (tooCloseRangeCollider.bounds.Contains(player.transform.position))
                {
                    state = State.BackingAway;
                }
                else if (!Physics.CheckSphere(transform.position, drainRangeCollider.radius, playerLayer))
                {
                    state = State.Idle;
                }
                else
                {
                    // PlayerManager.Instance.playerHealth -= drainAmount;
                }
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
}
