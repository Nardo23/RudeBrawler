using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtorController : MonoBehaviour
{
    Animator anim;
    public Rigidbody2D CharRb;

    bool moving;
    bool attacking;
    public PlayerInput input;
    float idleTime =0f;
    public float maxTimeTillIdle = 3f;
   
    Controls controls = new Controls();
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        controls = input.GetInput();
        if (controls.AttackState)
        {
            startAttack();
        }

        if(Mathf.Abs(controls.HorizontalMove) > 0 || Mathf.Abs(controls.VerticalMove) > 0)
        {
            moving = true;
        }
        else 
        {
            moving = false;
        }
        idleTimer();


        anim.SetBool("attacking", attacking);
        anim.SetBool("moving", moving);
    }
    
    void idleTimer()
    {
      
        if ( Mathf.Abs(controls.HorizontalMove) > 0 || Mathf.Abs(controls.VerticalMove) > 0 ||attacking)
        {
            idleTime = 0f;
            endReturn();
        }
        else
        {
            idleTime += Time.deltaTime;
        }
        if( idleTime > maxTimeTillIdle)
        {
            anim.SetBool("idleReturn", true);
            idleTime = 0;
        }
    }
    void endReturn()
    {
        anim.SetBool("idleReturn", false);
    }

    void endAttack()
    {
        attacking = false;
    }
    void startAttack()
    {
        attacking = true;
    }

}
