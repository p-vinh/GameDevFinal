using UnityEngine;
using Cinemachine;

public class CameraRotationController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public float rotationSpeed = 1f;

    private void Update()
    {
        virtualCamera.transform.rotation = Quaternion.identity;
    }
}