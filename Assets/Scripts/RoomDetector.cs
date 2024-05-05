using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlankStudio.Constants.Constants;

public class RoomDetector : MonoBehaviour
{
    [SerializeField]
    private RoomType m_RoomType = RoomType.EntryPoint;

    public static Action<RoomType, Bounds, Transform> PlayerEntered;

    [SerializeField]
    private BoxCollider m_BoxCollider = null;

    [SerializeField]
    private VisitStatus m_VisitStatus = VisitStatus.NotVisited;

    private void Start()
    {
        m_VisitStatus= VisitStatus.NotVisited;
        m_BoxCollider= GetComponent<BoxCollider>();
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
            PlayerEntered?.Invoke(m_RoomType, m_BoxCollider.bounds, collision.gameObject.transform);
        }
    }

  /*  private VisitStatus GetRoomVisitStatus(RoomType roomType)
    {
        string key = roomType.ToString() + "_VisitStatus";
        if (PlayerPrefs.HasKey(key))
        {
            return (VisitStatus)PlayerPrefs.GetInt(key);
        }
        return VisitStatus.NotVisited; 
    }

    private void SetRoomVisitStatus(RoomType roomType, VisitStatus status)
    {
        string key = roomType.ToString() + "_VisitStatus";
        PlayerPrefs.SetInt(key, (int)status);
    }*/
}
