using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.InputsManager;

public class Movement : MonoBehaviour
{
    
    Rigidbody rigidBody;
    public float speed = 4;
    Animator anim;
    Vector3 lookPos;
    public bool carryGun;
    public bool carrySword;
    private bool attackAnimDone = true;


    //Added input manager 
    public Camera m_Camera = null;
    public InputsManager m_InputsManager;
    private Vector3 m_MoveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        m_InputsManager = GetComponent<InputsManager>();
    }

    void Update()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //Point a ray from mouse to camera, use this to rotate player to look at mouse
        //print(m_InputsManager._MousePosition);
        Ray ray = Camera.main.ScreenPointToRay(m_InputsManager._MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,100))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0;

            transform.LookAt(transform.position + targetDirection, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * m_PlayerRotationSpeed);
        }

        //If sprint mode, change anim to run
        if(isShiftKeyDown)
        {
            speed = 5;
        }
        else
        {
            speed = 3;
        }
        anim.SetBool("Shift",isShiftKeyDown);

        if(Input.GetMouseButtonDown(0))
        {
            //If has gun, call this function here
            if(carryGun)
            {
                attackGun();
            }
            //If has sword, call this function here

        }
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //m_MoveDirection = CalculateDirection();
        //m_Rigidbody.velocity = m_MoveDirection * m_PlayerSpeed;

        m_InputsManager.HandleInputs();

        if(attackAnimDone)
        {
            rigidBody.velocity = new Vector3(m_InputsManager._HorizontalInput*speed, 0, m_InputsManager._VerticalInput * speed);
        }

        anim.SetFloat("Forward",m_InputsManager._HorizontalInput);
        anim.SetFloat("Turn",m_InputsManager._VerticalInput);
       
    }

    void attackGun()
    {
        if(attackAnimDone)
        {
            attackAnimDone = false;
            anim.SetBool("Attack",true);
        }
        
    }

    void attackSword()
    {

    }

    void finishedAttackAnim()
    {
        anim.SetBool("Attack",false);
        attackAnimDone = true;
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
}
