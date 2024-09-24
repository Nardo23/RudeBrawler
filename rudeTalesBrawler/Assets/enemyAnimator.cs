using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class enemyAnimator : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    public Enemy enemyScript;
    public Rigidbody2D charRb;
    public NavMeshAgent agent;
    public float attackFrequency = 1.5f;
    public int numberOfAttackAnims;
    float timer = 0;
    public Vector3 deltaPosition;
    bool alive = true;
    public GameObject Dropable;
    public bool hit = false;
    public int Armor;
    public int bonusArmor;
    public float hitParticleYPos=.5f;
    ColoredFlash flashScript;
    void Awake()
    {
        anim = GetComponent<Animator>();
        timer = Random.Range(0, attackFrequency);
        flashScript = GetComponent<ColoredFlash>();
    }

    void InteractingBegin()
    {
        enemyScript.isInteracting=true;
    }
    void InteractingEnd()
    {
        enemyScript.isInteracting = false;
       if(!enemyScript.inRange)
        {
            anim.SetBool("Moving", true);
        }
    }
    void stopMovement()
    {
        charRb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
            tick();
    }
    
   
    void tick()
    {
        anim.SetBool("Grounded", enemyScript.onBase);

        //check if moving

        //Debug.Log("gob vel: " + agent.velocity);

        if (!enemyScript.isInteracting)
        {
            if (Mathf.Abs(agent.velocity.x) > .4f || Mathf.Abs(agent.velocity.y) > .4f)
            {
                anim.SetBool("Moving", true);
            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        if (enemyScript.doingSpecial)
        {
            anim.SetTrigger("Specialing");
            Debug.Log("EnemySpecialStart");
        }
        if (enemyScript.arrived)
        {
            anim.SetTrigger("Specialing");
            enemyScript.arrived = false;
        }
        else if (enemyScript.inRange)
        {
            attack();
        }



    }
    void endSpecial()
    {
        enemyScript.doingSpecial = false;
        enemyScript.SpecialMoving = false;
        enemyScript.forceEnd = false;

    }
    void forceEndSpecial()
    {
        enemyScript.forceEnd = true;
    }
    void startSpecialMoving()
    {
        enemyScript.SpecialMoving = true;
    }

    void attack()
    {
        
        timer += Time.deltaTime;

        if(timer> attackFrequency)
        {
            timer = 0f;
            int attackNum = Random.Range(1, numberOfAttackAnims+1);
            //Debug.Log(attackNum);
            enemyScript.faceTarget();
            if (!enemyScript.isInteracting)
            {
                anim.SetInteger("AttackCount", attackNum);
                anim.SetTrigger("Attack");
            }
            

        }

    }

    void endHit()
    {
        hit = false;
    }
    void dropItem()
    {
        if (Dropable != null)
        {
            Instantiate(Dropable, transform.position, Quaternion.identity);
        }
    }
    public void hurt(float xPos, float knock, float stopDuration, float damageType, int strength)
    {
        
        if(xPos < transform.position.x)
        {
            enemyScript.faceLeft();
        }
        else
        {
            enemyScript.faceRight();
        }
        if (hit)
            knock = 0;

        if(strength> Armor) // full damage and knockback and hurt animation
        {
            anim.SetTrigger("Hit");
            anim.SetFloat("hitType", damageType);
        }
        else// color flash instead of hurt animation
        {
            flashScript.Flash();
        }
        if (strength == (Armor+bonusArmor))// full damage knockback no animation;
        {
            
        }
        if (strength - (Armor + bonusArmor) == -1)//full damage half knockback no animation
        {
            knock = knock * .5f;
        }
        if(strength- (Armor + bonusArmor) == -2)//full damage no knockback no animation
        {
            knock = 0;
        }
        if(strength- (Armor + bonusArmor) <= -3) //half damage no animation no knockback
        {
            knock = 0;
        }

        enemyScript.closestTarget();
        //anim.Play("hit");
        
        Debug.Log("hit");
        if (stopDuration > 0)
        {
            enemyScript.enableHitStop(stopDuration, anim, knock);
        }
        enemyScript.knockback(knock);

    }

    public void die()
    {
        //Debug.Log("die");
        anim.SetTrigger("Die");
        anim.SetBool("Moving", false);
        alive = false;
    }
    void deleteEnemy()
    {
        Destroy(charRb.gameObject);
    }

    private void OnAnimatorMove()
    {
        deltaPosition = anim.deltaPosition/ Time.deltaTime;
        //if(attacking)
        //charRb.velocity = deltaPosition;
    }
}
