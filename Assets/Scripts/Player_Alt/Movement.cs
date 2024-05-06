using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.InputsManager;

public class Movement : MonoBehaviour
{

    Rigidbody rigidBody;
    Animator anim;
    public GameObject gun;
    public GameObject sword;
    public bool carryGun;
    public float fireDelay;
    private bool attackAnimDone = true;
    private BoxCollider swordCollider;
    private Gun gunScript;

    //Added input manager 
    public Camera m_Camera = null;
    public InputsManager m_InputsManager;
    private bool canFire = true; //Adds delay

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
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
        if (GameObject.FindGameObjectWithTag("MainCamera") == null) return;

        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        //If sprint mode, change anim to run
        if (isShiftKeyDown)
        {
            PlayerStats.Instance.MovementSpeed = 5;
        }
        else
        {
            PlayerStats.Instance.MovementSpeed = 4;
        }
        anim.SetBool("Shift", isShiftKeyDown);



        if (m_InputsManager._MouseLeftClick)
        {
            //If has gun, call this function here
            if (carryGun && canFire)
            {
                attackGun();
            }
            else //If has sword, call this function here
            {
                attackSword();
            }

        }
    }

    void FixedUpdate()
    {
        m_InputsManager.HandleInputs();
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(m_InputsManager._MousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.red);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }

        if (rigidBody.velocity.magnitude < 0.1f)
        {
            rigidBody.velocity = Vector3.zero;
        }

        if (attackAnimDone && !carryGun)
        {
            rigidBody.velocity = new Vector3(m_InputsManager._HorizontalInput * PlayerStats.Instance.MovementSpeed, 0, m_InputsManager._VerticalInput * PlayerStats.Instance.MovementSpeed);
        }

        if (carryGun)
        {
            rigidBody.velocity = new Vector3(m_InputsManager._HorizontalInput * PlayerStats.Instance.MovementSpeed, 0, m_InputsManager._VerticalInput * PlayerStats.Instance.MovementSpeed);
        }


        anim.SetFloat("Forward", m_InputsManager._HorizontalInput);
        anim.SetFloat("Turn", m_InputsManager._VerticalInput);

    }

    private void attackGun()
    {
        canFire = false;
        gunScript.Shoot();
        attackAnimDone = false;
        anim.SetBool("Attack", true);
        Invoke("restartFire", fireDelay);
    }

    private void restartFire()
    {
        canFire = true;
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

}
