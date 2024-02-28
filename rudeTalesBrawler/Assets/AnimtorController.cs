using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtorController : MonoBehaviour
{
    Animator anim;
    public Rigidbody2D CharRb;
    public Rigidbody2D jumpRb;
    public CharacterMovement moveScript;
    public Vector3 deltaPosition;

    bool moving;
    bool attacking;
    bool attackbuffered = false;
    public PlayerInput input;
    float idleTime =0f;
    public float maxTimeTillIdle = 3f;
    int attackCount = 0;
    public int attackCountMax;
    bool prevGrounded;
    bool alive = true;
    Controls controls = new Controls();
    float enemyXposFromHit;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        prevGrounded = moveScript.onBase;
        
    }

    // Update is called once per frame
    void Update()
    {
        controls = input.GetInput();

        //check for attack
        if (controls.AttackState && !attacking)
        {
            //attackbuffered = false;
            startAttack();
        }
        else if(controls.AttackState && attacking)
        {
            attackbuffered = true;
        }
        if(attackbuffered && !attacking)
        {
            attackbuffered = false;
            startAttack();
        }


        //check if moving
        if (moving)
        {
            if (Mathf.Abs(CharRb.velocity.x) > 1f || Mathf.Abs(CharRb.velocity.y) > 1f)
            {
                moving = true;

            }
            else
            {
                moving = false;
            }
        }
        else
        {
            if(Mathf.Abs(controls.HorizontalMove) > 0 || Mathf.Abs(controls.VerticalMove) > 0)
            {
                moving = true;
            
            }
            else 
            {
                moving = false;
            }
        }
        
        if(moving && !attacking)
        {
            attackCount = 0;
        }

        idleTimer();

        anim.SetBool("attacking", attacking);
        anim.SetBool("moving", moving);
        anim.SetInteger("attackCount", attackCount);
        anim.SetBool("grounded", moveScript.onBase);
        anim.SetFloat("jumpVel", jumpRb.velocity.y);

        landedCheck();// has to be before this if but after everything else
        if(moveScript.onBase && attacking)
        {
            //CharRb.velocity = Vector3.zero;
            moveScript.canMove = false;
        }
        
    }
    
    void landedCheck()
    {
        if (!moveScript.onBase)
        {
            attackCount = 0;
            attackbuffered = false;
        }
        if(!prevGrounded && moveScript.onBase)
        {
            attacking = false;
            anim.SetBool("attacking", attacking);
        }

        prevGrounded = moveScript.onBase;
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
            attackCount = 0;
        }
    }
    void endReturn()
    {
        anim.SetBool("idleReturn", false);
    }

    void endAttack()
    {
        //Debug.Log("piss");
        moveScript.canMove = true;
        attacking = false;
    }
    void startAttack()
    {
        if (attackCount > attackCountMax)
            attackCount = 1;
        attackCount++;
        
        anim.SetInteger("attackCount", attackCount);
        
        attacking = true;
        if (attackCount > attackCountMax)
            attackCount = 1;
        //Debug.Log("attackCount: " + attackCount);
    }

    public void hurt(float xPos)
    {
        if (xPos < transform.position.x)
        {
            if (moveScript.facingRight) //hit from behind
            {
                anim.SetTrigger("HitB");
            }
            else                        //hit from front
            {
                anim.SetTrigger("HitF");
            }
        }
        else
        {
            if(moveScript.facingRight)  //hit from front
            {
                anim.SetTrigger("HitF");
            }
            else                        //hit from behind
            {
                anim.SetTrigger("HitB");
            }
        }
        moveScript.canMove = false;  // add endattack function to ends of hit animation to allow character to move again
        enemyXposFromHit = xPos;
    }
    public void die()
    {
        if (enemyXposFromHit > CharRb.transform.position.x) // enemy on right
        {
            if (!moveScript.facingRight)
            {
                moveScript.flip();
            }
        }
        else                                               // enemy on left
        {
            if (moveScript.facingRight)
                moveScript.flip();

        }

        Debug.Log("die");
        anim.SetTrigger("Die");
        anim.SetBool("moving", false);
        alive = false;
        moveScript.canMove = false; // will have to reEnable on respawn
        
    }
    private void OnAnimatorMove()
    {
        //deltaPosition = anim.deltaPosition/ Time.deltaTime*10;
        //if(attacking)
           // CharRb.velocity = deltaPosition;
    }
    void animMove(float distance)
    {
        float i = 1;
        if (CharRb.gameObject.transform.rotation.y < 0)
            i = -1;
        //CharRb.velocity = Vector3.zero;
        CharRb.velocity = new Vector3(i * distance, 0, 0);
        //Debug.Log("distance: " + distance);
    }

}
