using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedMenuUI : MonoBehaviour
{
    public GameObject transition1;
    public GameObject transition2;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        var step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transition2.transform.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, transition2.transform.position) < 0.001f)
        {
            transform.position = transition1.transform.position;
        }
    }
}
