using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.InputsManager;

public class Crosshair : MonoBehaviour
{

    Vector3 pos;
    public float speed = 1f;
    public InputsManager m_InputsManager;

    void Start()
    {
        m_InputsManager = GetComponent<InputsManager>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        pos = m_InputsManager._MousePosition;
        pos.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
