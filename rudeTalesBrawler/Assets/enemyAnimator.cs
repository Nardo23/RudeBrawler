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
    void Start()
    {
        anim = GetComponent<Animator>();
        timer = Random.Range(0, attackFrequency);
    }

    void InteractingBegin()
    {
        enemyScript.isInteracting=true;
    }
    void InteractingEnd()
    {
        enemyScript.isInteracting = false;
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


        if (enemyScript.inRange)
        {
            attack();
        }
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
            anim.SetInteger("AttackCount", attackNum);
            anim.SetTrigger("Attack");

        }

    }

    public void hurt(float xPos)
    {
        
        if(xPos < transform.position.x)
        {
            enemyScript.faceLeft();
        }
        else
        {
            enemyScript.faceRight();
        }
        //anim.Play("hit");
        anim.SetTrigger("Hit");
        Debug.Log("hit");
    }

    public void die()
    {
        Debug.Log("die");
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
        charRb.velocity = deltaPosition;
    }
}
