using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    [SerializeField]
    public AudioClip[] hitSounds;
    public bool hitOnce = false; // if true can only hurt one target
    public GameObject hitParticles;
    public Vector2 specificHurtPitchRange = new Vector2(.9f, 1.1f);
    //public bool isBasic = false;

    public int attackID =0;
    public int prevID;
    public bool multiHit;
    public GameObject[] prevTarget = new GameObject[20];
    GameObject[] emptyArray = new GameObject[20];
    int prevTargetIndex = 0;
    bool hitValid = false;
    bool hitPreviously(GameObject target)
    {
        //if (prevTarget != null)
        {
            bool found = false;
            foreach (GameObject ob in prevTarget)
            {
                if (target == ob)
                    found = true;
            }
            if (found)
                return true;
            else
                return false;
        }
        //return false;
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (prevID != attackID|| multiHit)
        {
            prevTarget = new GameObject[20];
            //Debug.Log("clearingPRevTargets");
            prevTargetIndex = 0;
        }
        //Debug.Log(transform.parent.name + " collided with " + collision.transform.parent.name); // CAREFUL this breaks projectiles that dont have parents
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
                    if (Mathf.Abs(u.transform.parent.transform.position.y- yCheckTransform.position.y) <= yRange+u.bonusYSize && !hitPreviously(collision.gameObject))// check if bases are within a certain y range
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

                        soundEffects s = collision.gameObject.GetComponentInParent<soundEffects>();
                        if(s != null && hitSounds != null)
                        {
                            s.recievedHit = true;
                            s.specificHit = hitSounds;
                            s.specificHurtPitchRange = specificHurtPitchRange;
                            s.Hurt();
                        }
                        prevTarget[prevTargetIndex] = collision.gameObject;
                        prevTargetIndex++;
                        hitValid = true;
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
                    if (Mathf.Abs(q.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange+q.bonusYSize && !hitPreviously(collision.gameObject))// check if bases are within a certain y range
                    {
                        q.hurt(transform.position.x);

                        GameObject particles = Instantiate(hitParticles, new Vector2(collision.transform.position.x, collision.transform.position.y ), Quaternion.identity);
                        particles.transform.parent = collision.transform.parent;

                        enemyHealth h = collision.gameObject.GetComponentInParent<enemyHealth>();
                        if (h != null)
                        {
                            h.changeHealth(-damage);
                        }

                        soundEffects s = collision.gameObject.GetComponentInParent<soundEffects>();
                        if (s != null && hitSounds != null)
                        {
                            s.recievedHit = true;
                            s.specificHit = hitSounds;
                            s.specificHurtPitchRange = specificHurtPitchRange;
                            s.Hurt();
                        }
                        prevTarget[prevTargetIndex] = collision.gameObject;
                        prevTargetIndex++;
                        hitValid = true;
                    }
                    
                }
            }
            if(collision.tag == "BossHurt")
            {
                //Debug.Log(transform.parent.name + " bosshurt");
                if (checkFromParent)
                {
                    yCheckTransform = transform.parent.transform;
                }
                else
                {
                    yCheckTransform = checkObject.transform;
                }
                boss bossScript = collision.GetComponentInParent<boss>();
                if (bossScript != null)
                {
                    if (Mathf.Abs(bossScript.transform.position.y - yCheckTransform.position.y) <= yRange + bossScript.bonusSize && !hitPreviously(collision.gameObject))// check if bases are within a certain y range
                    {
                        GameObject particles = Instantiate(hitParticles, new Vector2(collision.transform.position.x, collision.transform.position.y), Quaternion.identity);
                        particles.transform.parent = collision.transform.parent;
                        bossScript.changeHealth(-damage);
                        prevTarget[prevTargetIndex] = collision.gameObject;
                        prevTargetIndex++;
                        hitValid = true;

                        soundEffects s = collision.gameObject.GetComponentInParent<soundEffects>();
                        if (s != null && hitSounds != null)
                        {
                            s.recievedHit = true;
                            s.specificHit = hitSounds;
                            s.specificHurtPitchRange = specificHurtPitchRange;
                            s.Hurt();
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
                    if (Mathf.Abs(c.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange && !hitPreviously(collision.gameObject))// check if bases are within a certain y range
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
                        prevTarget[prevTargetIndex] = collision.gameObject;
                        prevTargetIndex++;
                        hitValid = true;
                    }
                }

                
            }
        }
        if (collision.tag == "BasicHurt")
        {
            if (checkFromParent)
            {
                yCheckTransform = transform.parent.transform;
            }
            else
            {
                yCheckTransform = checkObject.transform;
            }
            basicEnemyHealth basicScript = collision.GetComponentInParent<basicEnemyHealth>();
            if (basicScript != null)
            {
                if (Mathf.Abs(basicScript.transform.position.y - yCheckTransform.position.y) <= yRange+basicScript.bonusWidth && !hitPreviously(collision.gameObject))
                {
                    GameObject particles = Instantiate(hitParticles, new Vector2(collision.transform.position.x, collision.transform.position.y), Quaternion.identity);
                    particles.transform.parent = collision.transform.parent;
                    basicScript.changeHealth(-damage);
                    basicScript.knockback(knockbackForce, transform.position.x);
                    prevTarget[prevTargetIndex] = collision.gameObject;
                    prevTargetIndex++;
                    hitValid = true;

                    soundEffects s = collision.gameObject.GetComponentInParent<soundEffects>();
                    if (s != null && hitSounds != null)
                    {
                        s.recievedHit = true;
                        s.specificHit = hitSounds;
                        s.specificHurtPitchRange = specificHurtPitchRange;
                        s.Hurt();
                    }
                }
            }

        }
        if(hitValid)
            prevID = attackID;
        hitValid = false;
        if (prevTargetIndex > 20)
            prevTargetIndex = 0;

    }
}
