using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleEnemyAnimator : MonoBehaviour
{
    Animator anim;
    bool alive = true;
    public GameObject rootObject; //the parent of enemy, so we know what to delete when this enemy dies
    public Rigidbody2D charRb;
    public float bonusYSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("alive",alive);
    }

    public void hurt(float xPos)
    {

        if (xPos < transform.position.x)
        {
            if (transform.localScale.x < 0)
            {
                anim.SetTrigger("Hitb");
            }
            else
            {
                anim.SetTrigger("Hitf");
            }
                
        }
        else
        {
            if (transform.localScale.x < 0)
            {
                anim.SetTrigger("Hitf");
            }
            else
            {
                anim.SetTrigger("Hitb");
            }
        }
    }
    
    public void die()
    {
        alive = false;
        anim.SetBool("alive", alive);
        anim.SetTrigger("Die");
        if(rootObject.transform.localScale.x<0)
            rootObject.transform.localScale = new Vector3(rootObject.transform.localScale.x*-1, rootObject.transform.localScale.y, rootObject.transform.localScale.z);
        
        
    }
    void deleteEnemy()
    {
        Destroy(rootObject);
    }

    void animMove(float distance)
    {
        float i = 1;
        if (transform.localScale.x < 0)
        {
            i = -1;
        }
        charRb.velocity = new Vector3(i * distance + 0, 0, 0);
    }
    
}
