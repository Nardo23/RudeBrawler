using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDamage : MonoBehaviour
{
    public bool isPlayer;

    public int damage; // value changed by different attack animations
    public float yRange; // also changed by attack anims
    public float knockbackForce; // ditto
    public float hitStopDuration; // ditto
    public bool checkFromParent = true; //yrange check should occur from parent object ie the shadow position. only disable for projectiles
    public GameObject checkObject; // used if checkFromParen = false
    public int damageType; // 0 = phsical, 1 = electric determens what hurt animation to play
    public int attackStrength = 3; // compare this to the the target's armor to determine if the character takes reduced damage or knockback
    Transform yCheckTransform;
    
    public bool hitOnce = false; // if true can only hurt one target
    public GameObject hitParticles;
    
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
                    if (Mathf.Abs(u.transform.parent.transform.position.y- yCheckTransform.position.y) <= yRange+u.bonusYSize)// check if bases are within a certain y range
                    {
                        
                        if(transform.parent!=null)                 
                            u.hurt(transform.parent.position.x, knockbackForce, hitStopDuration, damageType, attackStrength);
                        else
                            u.hurt(transform.position.x, knockbackForce, hitStopDuration, damageType, attackStrength);

                        if (hitStopDuration > 0 && GetComponentInParent<CharacterMovement>()!=null)
                        {
                            GetComponentInParent<CharacterMovement>().enableHitStop(hitStopDuration);
                        }
                            
                        GameObject particles = Instantiate(hitParticles, new Vector2(collision.transform.position.x, collision.transform.position.y + u.hitParticleYPos), Quaternion.identity);
                        particles.transform.parent = collision.transform.parent;


                        if(attackStrength - (u.Armor+u.bonusArmor) <= -3)
                        {
                            damage = damage /2;
                        }
                        enemyHealth h = collision.gameObject.GetComponentInParent<enemyHealth>();
                        if (h != null)
                        {
                            h.changeHealth(-damage);
                        }
                        if (hitOnce)
                        {
                            GetComponent<Collider2D>().enabled = false;
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
                    if (Mathf.Abs(q.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange+q.bonusYSize)// check if bases are within a certain y range
                    {
                        q.hurt(transform.position.x);

                        GameObject particles = Instantiate(hitParticles, new Vector2(collision.transform.position.x, collision.transform.position.y ), Quaternion.identity);
                        particles.transform.parent = collision.transform.parent;

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
                            c.hurt(transform.position.x, knockbackForce);
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
