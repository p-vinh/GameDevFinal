using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_Follow = null;

    [SerializeField]
    private float m_CameraSpeed = 1f;

    //[SerializeField]
    //private float m_PivotAngle = 0f;

    /* [SerializeField]
     private float m_CameraSensitivity = 0f;*/

    // Idle Offset  -0.35, 0, 4.25
    [SerializeField]
    private Vector3 m_CameraOffset = Vector3.zero;

    private Vector3 m_CameraVelicity = Vector3.zero;
    void Start()
    {
        m_Follow = FindObjectOfType<PlayerManager>().transform;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 newPosition = new Vector3(m_Follow.position.x + m_CameraOffset.x, m_Follow.position.y + m_CameraOffset.y, m_Follow.position.z + m_CameraOffset.z);
        float distance = Vector3.Distance(transform.position, newPosition);

        if (distance > 5f) // Will snap to the player if the distance is greater than 5
        {
            transform.position = newPosition;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref m_CameraVelicity, m_CameraSpeed);

        }
    }

}
