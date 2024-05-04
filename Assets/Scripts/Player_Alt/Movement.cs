using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.InputsManager;

public class Movement : MonoBehaviour
{

    Rigidbody rigidBody;
    public float speed = 4;
    Animator anim;
    public GameObject gun;
    public GameObject sword;
    public bool carryGun;
    private bool attackAnimDone = true;
    private BoxCollider swordCollider;
    private Gun gunScript;

    //Added input manager 
    public Camera m_Camera = null;
    public InputsManager m_InputsManager;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        m_InputsManager = GetComponent<InputsManager>();
        swordCollider = sword.GetComponent<BoxCollider>();
        gunScript = gun.GetComponent<Gun>();
        anim.SetBool("carryGun", carryGun);


        if (!carryGun) //Has sword
        {
            gun.SetActive(false);
            sword.SetActive(true);
        }
        else //Has gun
        {
            gun.SetActive(true);
            sword.SetActive(false);
        }
    }

    void Update()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        //Point a ray from mouse to camera, use this to rotate player to look at mouse
        //print(m_InputsManager._MousePosition);
        Ray ray = Camera.main.ScreenPointToRay(m_InputsManager._MousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0;

            transform.LookAt(transform.position + targetDirection, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * m_PlayerRotationSpeed);
        }

        //If sprint mode, change anim to run
        if (isShiftKeyDown)
        {
            speed = 5;
        }
        else
        {
            speed = 3;
        }
        anim.SetBool("Shift", isShiftKeyDown);

        if (Input.GetMouseButton(0))
        {
            //If has gun, call this function here
            if (carryGun)
            {
                attackGun();
            }
            else //If has sword, call this function here
            {
                attackSword();
            }

        }



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //m_MoveDirection = CalculateDirection();
        //m_Rigidbody.velocity = m_MoveDirection * m_PlayerSpeed;

        m_InputsManager.HandleInputs();

        if (attackAnimDone && !carryGun)
        {
            rigidBody.velocity = new Vector3(m_InputsManager._HorizontalInput * speed, 0, m_InputsManager._VerticalInput * speed);
        }

        if (carryGun)
        {
            rigidBody.velocity = new Vector3(m_InputsManager._HorizontalInput * speed, 0, m_InputsManager._VerticalInput * speed);
        }


        anim.SetFloat("Forward", m_InputsManager._HorizontalInput);
        anim.SetFloat("Turn", m_InputsManager._VerticalInput);

    }

    void attackGun()
    {
        gunScript.Shoot();
        attackAnimDone = false;
        anim.SetBool("Attack", true);

    }

    void attackSword()
    {
        if (attackAnimDone)
        {
            attackAnimDone = false;
            swordCollider.enabled = true;
            anim.SetBool("Attack", true);
        }
    }

    void finishedAttackAnim()
    {
        anim.SetBool("Attack", false);
        attackAnimDone = true;
        swordCollider.enabled = false;
    }


    private Vector3 CalculateDirection()
    {
        //vector = m_Camera.transform.forward * m_InputsManager._VerticalInput; 
        Vector3 vector = Vector3.forward * m_InputsManager._VerticalInput;

        //vector = vector + m_Camera.transform.right * m_InputsManager._HorizontalInput;
        vector = vector + Vector3.right * m_InputsManager._HorizontalInput;
        vector.Normalize();
        vector.y = 0;
        return vector;
    }
}
