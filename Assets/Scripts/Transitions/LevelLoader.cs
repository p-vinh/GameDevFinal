using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;

    public void fadeTransition()
    {
        transition.SetTrigger("Start");
    }
}
