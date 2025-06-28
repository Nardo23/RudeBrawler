using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daggerFix : StateMachineBehaviour
{
    AnimatorClipInfo[] m_CurrentClipInfo;
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
        animator = animator.GetComponent<Animator>();
        if(animator.GetFloat("specialNum") == 0)
        {
            //Debug.Log("daggerSpecial "+ animator.GetFloat("specialNum"));
            //animator.ResetTrigger("special");
            
            //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.1f && controls.SpecialAttackStartState)
            if(animator.GetBool("specialing") == false && animator.GetBool("attacking") == false)
            {
                if (controls.SpecialAttackStartState || controls.AttackState)
                {
                    animator.Play("idle", layerIndex);
                }
                else
                {
                    animator.ResetTrigger("special");

                }
                
            }
        }
        else if(animator.GetFloat("specialNum") == 2 && animator.GetBool("grounded"))
        {
            animator.Play("idle", layerIndex);
        }
    }
  

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
