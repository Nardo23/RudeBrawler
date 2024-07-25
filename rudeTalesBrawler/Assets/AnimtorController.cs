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
    public bool attacking;
    bool attackbuffered = false;
    public PlayerInput input;
    float idleTime =0f;
    public float maxTimeTillIdle = 3f;
    int attackCount = 0;
    public int attackCountMax;
    int prevAttackCount;
    bool prevGrounded;
    public bool alive = true;
    Controls controls = new Controls();
    float enemyXposFromHit;
    public bool resetRunOnTurn = false;
    float prevRotation; // for determing when the sprite turns
    levelManager levelManagerScript;
    
    public bool canDoublejump;
    public bool hit = false;
    public string CharacterID; // A =albee, B = bellow, C =cordelia, D= deBonesby, S= stirfry
    public specialAttacks specialScript;
    public bool specialing = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        prevGrounded = moveScript.onBase;
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();


    }

    void HitStopOn()
    {
        moveScript.canStop = true;
    }
    void HitStopOff()
    {
        moveScript.canStop = false;
    }
    void startAirJump()
    {
        moveScript.startjump();
    }
    void boost()
    {
        moveScript.boost();
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
        if (specialing)
        {
            attackbuffered = false;
        }
        if(controls.SpecialAttackStartState && !attacking)
        {
            
            if(specialScript!= null)
            {
                attacking = true;
                specialing = true;
                Debug.Log("attacking = true");
                if(CharacterID == "D")
                {
                    //specialScript.Fireball();
                }
            }
        }
        


        //check if moving
        if (false)
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

        if (resetRunOnTurn)
        {
            if (prevRotation != CharRb.transform.rotation.y)
            {
                if(moveScript.onBase && moving && !attacking)
                {
                    anim.SetTrigger("turn");
                    Debug.Log("turn");
                }
            }
        }

        

        idleTimer();

        if (canDoublejump)
        {
            anim.SetInteger("jumpCount", moveScript.currentJumps);
        }

        anim.SetBool("attacking", attacking);
        anim.SetBool("specialing", specialing);
        anim.SetBool("moving", moving);
        anim.SetInteger("attackCount", attackCount);
        anim.SetBool("grounded", moveScript.onBase);
        anim.SetFloat("jumpVel", jumpRb.velocity.y);

        if (!moveScript.onBase && canDoublejump)
        {
            if (controls.JumpState && moveScript.currentJumps > 0)
            {
                Debug.Log("airrrr");
                anim.SetTrigger("airJump");
            }
        }
        if (canDoublejump)
        {
            if (!moveScript.onBase)
            {
                if (controls.JumpState)
                {
                    anim.SetBool("canGroundJump", false);
                }

            }
            else {
                anim.SetBool("canGroundJump", true); ;
            }
        }
        
        landedCheck();// has to be before this if but after everything else
        if(moveScript.onBase && attacking)
        {
            //CharRb.velocity = Vector3.zero;
            moveScript.canMove = false;
        }

        if(attacking == false && hit ==false && alive)
        {
            moveScript.canMove = true;
        }

        prevRotation = CharRb.transform.rotation.y;
        
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
    void endSpecial()
    {
        specialing = false;
    }
    private void ResetAttackCount()
    {
        attackCount = 0;
    }
    void startAttack()
    {
        anim.SetTrigger("attack");
        
        prevAttackCount = attackCount;
        if (attackCount > attackCountMax)
            attackCount = 1;
        attackCount++;
        
        anim.SetInteger("attackCount", attackCount);
        
        attacking = true;
        if (attackCount > attackCountMax)
            attackCount = 1;
        //Debug.Log("attackCount: " + attackCount);
    }

    public void endHit()
    {
        anim.SetBool("hit", false);
        moveScript.canMove = true;
        hit = false;
    }
   
    public void hurt(float xPos, float knock)
    {
        moveScript.canMove = false;  // add endhit function to ends of hit animation to allow character to move again
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
            if (!hit)
                moveScript.knockback(knock, false);
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
            if (!hit)
            {

            }
                moveScript.knockback(knock, true);
        }
        
        
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

      
        anim.SetTrigger("Die");
        anim.SetBool("moving", false);
        alive = false;
        moveScript.canMove = false; // will have to reEnable on respawn
        levelManagerScript.updateLivingCount(-1); // subtracts one from living player count
    }
    public void Respawn() // incomplete; isnt called by anything yet
    {
        moveScript.canMove = true;
        levelManagerScript.updateLivingCount(1);
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
        CharRb.velocity = new Vector3(i * distance+ 0,0, 0);
        //Debug.Log("distance: " + distance);
    }

}
