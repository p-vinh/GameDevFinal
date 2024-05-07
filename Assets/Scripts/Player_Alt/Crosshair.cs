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
    void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(m_InputsManager._MousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.position = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
        }
    }
}
