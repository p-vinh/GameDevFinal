using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    Vector3 pos;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        pos = Input.mousePosition;
        pos.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
