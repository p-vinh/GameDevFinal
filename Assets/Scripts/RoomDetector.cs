using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlankStudio.Constants.Constants;

public class RoomDetector : MonoBehaviour
{
    [SerializeField]
    private RoomType m_RoomType = RoomType.EntryPoint;

    public static Action<RoomType, Transform,  Bounds, Transform> PlayerEntered;

    [SerializeField]
    private BoxCollider m_BoxCollider = null;

    [SerializeField]
    private VisitStatus m_VisitStatus = VisitStatus.NotVisited;

    private float m_Width, m_Breadth;

    private void Start()
    {
        m_VisitStatus= VisitStatus.NotVisited;
        m_BoxCollider= GetComponent<BoxCollider>();
        Renderer renderer = GetComponent<Renderer>();
        m_Width = renderer.bounds.size.x;
        m_Breadth = renderer.bounds.size.z;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (m_VisitStatus == VisitStatus.Visited)
            {
                return;
            }
            m_VisitStatus = VisitStatus.Visited;
            PlayerEntered.Invoke(m_RoomType, transform, m_BoxCollider.bounds, collision.gameObject.transform);
        }
    }
}
