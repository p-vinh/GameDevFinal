using PlayerController.InputsManager;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Vector3 m_MoveDirection = Vector3.zero;
    private Camera m_Camera = null;

    [SerializeField]
    private float m_PlayerSpeed = 10f;

    [Range(60f, 160f)]
    [SerializeField]
    private float m_PlayerRotationSpeed = 100f;

    private InputsManager m_InputsManager;

    [SerializeField]
    private Rigidbody m_Rigidbody;

    /*[SerializeField]
    private float m_AttackRange = 300f;*/

    [SerializeField]
    private Transform m_Crosshair;


    private void Start()
    {
        m_Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        m_InputsManager = GetComponent<InputsManager>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void PlayerMovement()
    {
        m_MoveDirection = CalculateDirection();
        m_Rigidbody.velocity = m_MoveDirection * m_PlayerSpeed;
    }

    private void PlayerRotation()
    {
        Vector3 rotateDirection = CalculateDirection();
        if (rotateDirection == Vector3.zero)
        {
            rotateDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, m_PlayerRotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }

    private Vector3 CalculateDirection()
    {
        Vector3 vector = Vector3.zero;
        //vector = m_Camera.transform.forward * m_InputsManager._VerticalInput; 
        vector = Vector3.forward * m_InputsManager._VerticalInput;

        //vector = vector + m_Camera.transform.right * m_InputsManager._HorizontalInput;
        vector = vector + Vector3.right * m_InputsManager._HorizontalInput;
        vector.Normalize();
        vector.y = 0;
        return vector;
    }

    public void RotateTowardsMouse()
    {
        Ray ray = m_Camera.ScreenPointToRay(m_InputsManager._MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * m_PlayerRotationSpeed);
        }
    }

    public void UpdateCrosshairPosition()
    {
        Ray ray = m_Camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            m_Crosshair.position = raycastHit.point + Vector3.up * 0.1f;
        }
    }
}