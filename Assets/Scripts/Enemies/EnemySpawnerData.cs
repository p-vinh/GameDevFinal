using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BlankStudio.Constants.Constants;

[System.Serializable]
public class EnemySpawnerData
{
    public List<EnemyTypeData> _EnemyTypeData
    {
        get => m_EnemyTypeData;
        private set => m_EnemyTypeData = value; 
    }

    [SerializeField]
    private List<EnemyTypeData> m_EnemyTypeData = new List<EnemyTypeData>();

    public EnemyTypeData GetEnemyProperties(RoomType roomType)
    {
        for (int i = 0; i < m_EnemyTypeData.Count; i++)
        {
            if (m_EnemyTypeData[i]._RoomType == roomType)  
            {
                return m_EnemyTypeData[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class EnemyTypeData
{
    public List<GameObject> _EnemyPrefeb
    {
        get => m_EnemyPrefeb; 
        private set => m_EnemyPrefeb = value;
    }

    [SerializeField]
    private List<GameObject> m_EnemyPrefeb = null;

    public int _EnemyCount
    {
        get => m_EnemyCount;
        private set => m_EnemyCount = value;
    }

    [SerializeField]
    private int m_EnemyCount = 10;

    public RoomType _RoomType
    {
        get => m_RoomType;
        private set => m_RoomType = value;
    }

    [SerializeField]
    private RoomType m_RoomType = RoomType.Room_1;
}