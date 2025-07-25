using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eruptUnstuck : StateMachineBehaviour
{
    PlayerInput inputScript;
    Controls controls = new Controls();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inputScript = animator.GetComponentInParent<PlayerInput>();


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controls = inputScript.GetInput();
        if (animator.GetFloat("specialNum") == 0)
        {
            //Debug.Log("animatorInt: " + animator.GetInteger("specialNum"));
            if (!controls.SpecialAttackState)
            {
                animator.SetTrigger("special");
            }
        }
        else
            animator.ResetTrigger("special");
        
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("special");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
