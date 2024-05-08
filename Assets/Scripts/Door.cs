using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float Timestamp { get; private set; }
    public AudioSource audioSource; // Code added by Abby (Sound Engineer)
    [SerializeField]
    private Animator m_Animator = null;

    [SerializeField]
    private LayerMask m_PlayerLayerMask = default;

    [SerializeField]
    public float m_MaxDistance = 10f;

    [SerializeField]
    private bool m_PlayerDetected = false;

    [SerializeField]
    private BoxCollider doNotEnterCollider;
    private float StartAngle = 0f;
    private float EndAngle = 360f;
    private int rayCount = 20;

    private void OnEnable()
    {
        EnemySpawner.lockAllDoors += LockUnlockDoors;
    }

    private void OnDisable()
    {
        EnemySpawner.lockAllDoors -= LockUnlockDoors;
    }

    private void Awake()
    {
        Timestamp = Time.time;
    }
    private void Start()
    {
        doNotEnterCollider.enabled = false;
    }

    private void Update()
    {
        if (m_PlayerDetected)
        {
            return;
        }
        RaycastIn180DegreeRange();
    }

    private void LockUnlockDoors(bool lockDoors)
    {
        doNotEnterCollider.enabled = lockDoors;
    }

    public void RaycastIn180DegreeRange()
    {

        float angleStep = Mathf.Abs(StartAngle - EndAngle) / rayCount;
        float angle = StartAngle;
        for (int i = 0; i < rayCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            Vector3 direction = rotation * transform.forward * -1;

            RaycastHit hit;
            Debug.DrawRay(transform.position, direction, Color.blue, m_MaxDistance);
            // Debug.Log(Physics.Raycast(transform.position, direction, out hit, m_MaxDistance, m_PlayerLayerMask));

            if (Physics.Raycast(transform.position, direction, out hit, m_MaxDistance, m_PlayerLayerMask))
            {
                m_PlayerDetected = true;
                PlayAnimation();
            }
            angle += angleStep;

        }
    }

    private void PlayAnimation()
    {
        m_Animator.SetTrigger("DoorOpen");
    }
}
