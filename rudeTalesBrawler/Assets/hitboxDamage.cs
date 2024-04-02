using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDamage : MonoBehaviour
{
    public bool isPlayer;

    public int damage; // value changed by different attack animations
    public float yRange; // also changed by attack anims
    public bool checkFromParent = true; //yrange check should occur from parent object ie the shadow position. only disable for projectiles
    public GameObject checkObject; // used if checkFromParen = false
    Transform yCheckTransform;
    public bool hitOnce = false; // if true can only hurt one target
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.tag == "EnemyHurt")
            {
                
                enemyAnimator u = collision.gameObject.GetComponentInParent<enemyAnimator>();
                if(u != null)
                {
                    if (checkFromParent)
                    {
                        yCheckTransform = transform.parent.transform;
                    }
                    else
                    {
                        yCheckTransform = checkObject.transform;
                    }
                    if (Mathf.Abs(u.transform.parent.transform.position.y- yCheckTransform.position.y) <= yRange)// check if bases are within a certain y range
                    {
                        
                        //Debug.Log("ggg");                  
                        u.hurt(transform.position.x);
                        
                        enemyHealth h = collision.gameObject.GetComponentInParent<enemyHealth>();
                        if (h != null)
                        {
                            h.changeHealth(-damage);
                        }
                    }
                }              

            }
            if(collision.tag == "SimpleEnemyHurt")
            {
                simpleEnemyAnimator q = collision.gameObject.GetComponentInParent<simpleEnemyAnimator>();
                if(q != null)
                {
                    if (checkFromParent)
                    {
                        yCheckTransform = transform.parent.transform;
                    }
                    else
                    {
                        yCheckTransform = checkObject.transform;
                    }
                    if (Mathf.Abs(q.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange)// check if bases are within a certain y range
                    {
                        q.hurt(transform.position.x);

                        enemyHealth h = collision.gameObject.GetComponentInParent<enemyHealth>();
                        if (h != null)
                        {
                            h.changeHealth(-damage);
                        }
                    }
                    
                }
            }
        }
        else
        {
            if (collision.tag == "PlayerHurt")
            {
                Debug.Log(this.name);
                AnimtorController c = collision.gameObject.GetComponentInParent<AnimtorController>();
                if(c != null)
                {
                    if (checkFromParent) 
                    {
                        yCheckTransform = transform.parent.transform;
                    }
                    else
                    {
                        yCheckTransform = checkObject.transform;
                    }
                    if (Mathf.Abs(c.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange)// check if bases are within a certain y range
                    {
                        if (c != null)
                        {
                            c.hurt(transform.position.x);
                        }
                        PlayerHealth p = collision.gameObject.GetComponentInParent<PlayerHealth>();
                        if (p != null)
                        {
                            p.changeHealth(-damage);
                            
                        }
                        if (hitOnce)
                        {
                            GetComponent<Collider2D>().enabled = false;
                        }
                    }
                }

                
            }
        }
        
    }
}
