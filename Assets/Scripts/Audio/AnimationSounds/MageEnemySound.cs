using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemySound : StateMachineBehaviour {
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // If state is Attack, play attack sound
        if (stateInfo.IsName("Attack")) {
            if (stateInfo.normalizedTime % 1.0f > 0.49f && stateInfo.normalizedTime % 1.0f < 0.51) {
                animator.GetComponent<MageAI>().attackSound.Play();
            }//end if
        }//end if
    }//end OnStateEnter()

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // If state is Attack, play attack sound
        if (stateInfo.IsName("Attack")) {
            if (!animator.GetComponent<MageAI>().attackSound.isPlaying) {
                if (stateInfo.normalizedTime % 1.0f > 0.49f && stateInfo.normalizedTime % 1.0f < 0.51) {
                    animator.GetComponent<MageAI>().attackSound.Play();
                }//end if
            }//end if
        }//end if
    }//end OnStateUpdate()

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
