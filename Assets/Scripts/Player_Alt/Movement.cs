using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody rigidBody;
    public float speed = 4;
    Animator anim;
    Vector3 lookPos;
    public bool carryGun;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //Point a ray from mouse to camera, use this to rotate player to look at mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }

        Vector3 lookDir = lookPos - transform.position;

        lookDir.y = 0; //prevents player looking up

        transform.LookAt(transform.position + lookDir, Vector3.up);

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;
        float vertical = Input.GetAxisRaw("Vertical") * speed;

        rigidBody.velocity = new Vector3(horizontal,0,vertical);

        anim.SetFloat("Forward",vertical);
        anim.SetFloat("Turn",horizontal);
       
    }
}
