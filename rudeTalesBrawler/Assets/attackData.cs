using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackData : StateMachineBehaviour
{
    public int damage;
    public float yRange= .5f;
    public float knockbackForce = 1;
    public float hitStopDuration;
    public int attackStrength;
    [Tooltip("0 = physical, 1 = electric")]
    public int damageType; 
     hitboxDamage dmgScript;
    public bool multiHit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dmgScript = animator.transform.GetComponentInChildren<hitboxDamage>(true);
        if(dmgScript != null)
        {
            //Debug.Log(dmgScript.transform.parent.name + " found dmgScrpt");
            dmgScript.damage = damage;
            //Debug.Log("damageTest " + dmgScript.damage + ", " + damage);
            dmgScript.yRange = yRange;
            dmgScript.knockbackForce = knockbackForce;
            dmgScript.hitStopDuration = hitStopDuration;
            dmgScript.attackStrength = attackStrength;
            dmgScript.damageType = damageType;
            dmgScript.multiHit = multiHit;
            dmgScript.attackID++;
            if (dmgScript.attackID > 9)
                dmgScript.attackID = 0;
        }
        
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
