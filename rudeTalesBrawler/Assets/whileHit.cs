using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whileHit : StateMachineBehaviour
{
    protected enemyAnimator animScript;
    protected AnimtorController PAnimScript;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animScript = animator.transform.GetComponent<enemyAnimator>();
        if (animScript != null)
        {
            //Debug.Log("ploop");
            animScript.hit = true;
        }

        PAnimScript = animator.transform.GetComponent<AnimtorController>();
        if (PAnimScript != null)
        {
            PAnimScript.hit = true;
        }
    }
   

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animScript = animator.transform.GetComponent<enemyAnimator>();
        if (animScript != null)
        {
            //Debug.Log("unploop");
            animScript.hit = false;
        }
        PAnimScript = animator.transform.GetComponent<AnimtorController>();
        if (PAnimScript != null)
        {
            Debug.Log("ploop");
            PAnimScript.hit = false;
        }
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
