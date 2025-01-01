using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialAttacks : MonoBehaviour
{
    public PlayerInput input;
    Controls controls = new Controls();
    public CharacterMovement moveScript;
    public Rigidbody2D charRb;
    Rigidbody2D jumpingRB;
    public GameObject characterSprite;
    AnimtorController animScript;
    Animator anim;
    float cooldown = .26f;
    bool onCooldown = false;
    float coolTimer;
    public float sideThreshold = .1f;

    // Start is called before the first frame update
    void Start()
    {
        jumpingRB = characterSprite.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animScript = GetComponent<AnimtorController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        controls = input.GetInput();
        if (animScript.CharacterID == "D")
        {
            if (moveScript.onBase && moveScript.landLag <= 0)
            {
                Fireball();
                
                if (animScript.attacking == false )
                {
                    //Fireball();
                    lightning();
                }
                canMisty = true;
            }
            else
            {
                MistyStep();
            }
            if (animScript.hit)
                animScript.enableAirDrag();
        }
        if(animScript.CharacterID == "S")
        {
            if (moveScript.onBase && moveScript.landLag <= 0)
            {
                
                groundDagger();
                daffy();
                    
            }
            else
            {
                
                airDaggers();
            }
        }
        if (animScript.CharacterID == "A")
        {
            if(moveScript.onBase && moveScript.landLag <= 0)
            {

                daffy(); ; //Albee can use daffy function to player her neutral special animation
                erupt();
                canMisty = true;
            }
            else
            {
                flipKick();
            }
            if (animScript.hit)
                animScript.enableAirDrag();
        }

    }

    [Header("Fireball")]
    public float minSpeed, maxSpeed, minLifeTime, maxLifetime;
    public int MinFireballDamage, MaxFireballDamage;
    float speed, lifetime;
    int damage;
    Vector2 size;
    public float MaxChargeTime;
    float chargeTime;
    public Vector2 minBallSize, maxBallSize;
    bool wasCharging = false;
    public GameObject fireball1, fireball2, fireball3;
    GameObject spawnedFireball;
    
    public void Fireball()
    {
        
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold && !animScript.specialing &&!anim.GetBool("attacking"))
        {
            Debug.Log("fireball A");
            
            wasCharging = true;
            moveScript.canMove = false;
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 0);
            
        }
        if (anim.GetBool("hit") == true)
        {
            
            animScript.specialing = false;
            wasCharging = false;
            chargeTime = 0;
            speed = minSpeed;
            lifetime = minLifeTime;
            damage = MinFireballDamage;
            size = minBallSize;
            chargeTime = 0;
            coolTimer = 0;
            //onCooldown = true;
            moveScript.canMove = true;
        }
        if (controls.SpecialAttackState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold && anim.GetFloat("specialNum")==0 && !anim.GetBool("Hit"))
        {
            //wasCharging = true;
            //moveScript.canMove = false;

            if (chargeTime < MaxChargeTime)
            {
                chargeTime += Time.deltaTime;
                damage = (int)Mathf.Lerp(MinFireballDamage, MaxFireballDamage, chargeTime / MaxChargeTime);
                speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / MaxChargeTime);
                lifetime = Mathf.Lerp(minLifeTime, maxLifetime, chargeTime / MaxChargeTime);
                size = Vector2.Lerp(minBallSize, maxBallSize, chargeTime / MaxChargeTime);
                if (chargeTime / MaxChargeTime >= .48f) //11/25 comes from the 11th frame of the 25 frame charge animation
                {
                    size = Vector2.Lerp(minBallSize, maxBallSize, (chargeTime-0.48f) / MaxChargeTime);
                    size = new Vector3(size.y, size.y, 0);
                    damage = damage / 2;
                }
            }
            else
            {
                damage = MaxFireballDamage;
                speed = maxSpeed;
                lifetime = maxLifetime;
                size = maxBallSize;
            }
            
        }
        else if (!controls.SpecialAttackState && animScript.specialing && !anim.GetBool("Hit"))
        {
            
            if (wasCharging)
            {
                
                Debug.Log("fireball B");
                wasCharging = false;
                //fire;
                anim.SetTrigger("special");
                if (chargeTime/MaxChargeTime < 0.48f) //.44 =11/25 comes from the 11th frame of the 25 frame charge animation
                {
                    spawnedFireball = fireball1;
                }
                else if(chargeTime / MaxChargeTime < 0.88f)
                {
                    spawnedFireball = fireball2;
                }
                else
                {
                    spawnedFireball = fireball3;
                }
                GameObject ball = Instantiate(spawnedFireball, transform.position, Quaternion.identity);
                ball.transform.GetChild(0).transform.localScale = ball.transform.localScale * size;
                ball.transform.GetChild(1).transform.localScale = ball.transform.localScale * size;
                //ball.transform.localScale = ball.transform.localScale * size;
                ball.transform.GetChild(0).transform.position = new Vector3(transform.position.x+1f, characterSprite.transform.position.y + 2f, 0);
                ball.transform.GetChild(1).transform.position = new Vector3(transform.position.x + 1f, ball.transform.position.y, 0);
                if (!moveScript.facingRight)
                {
                    ball.transform.Rotate(0, 180, 0);
                    speed = -speed;
                }
                lineProjectile linePro = ball.GetComponent<lineProjectile>();
                hitboxDamage DamageScript = ball.GetComponentInChildren<hitboxDamage>();
                DamageScript.damage = damage;
                linePro.speed = speed;
                linePro.lifeTime = lifetime;

                //reset values to minimum
                
                speed = minSpeed;
                lifetime = minLifeTime;
                damage = MinFireballDamage;
                size = minBallSize;
                chargeTime = 0;
                coolTimer = 0;
                //onCooldown = true;
                moveScript.canMove = true;                
            }
        }
        
    }
    public GameObject punchSquare;
    IEnumerator punch()
    {
        moveScript.canMove = false;

        coolTimer = 0;
        float t = 0;
        while (t < .25f)
        {
            t += Time.deltaTime;
            punchSquare.SetActive(true);
            yield return null;
        }
        punchSquare.SetActive(false);
        moveScript.canMove = true;
        onCooldown = true;
        yield return null;
    }
    [Header("lightning")]
    public GameObject lightningBolt;
    public float lightningXPos;
    public void lightning()
    {
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) > sideThreshold)
        {
            Debug.Log("lightning");

            moveScript.canMove = false;
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 1);
            GameObject lightn = null;
            if (controls.HorizontalMove > 0)
            {
                lightn = Instantiate(lightningBolt, new Vector3(transform.position.x + lightningXPos,transform.position.y,0),Quaternion.identity); // lightning bolt to the right
            }
            else
            {
                lightn = Instantiate(lightningBolt, new Vector3(transform.position.x - lightningXPos, transform.position.y, 0), Quaternion.identity); // lightning bolt to the left
                lightn.transform.Rotate(0, 180, 0);
            }
        }
    }
    [Header("MistyStep")]
    public float TPforce;
    public float TPtime;
    public bool canMisty = true;
    Vector2 TPdirection = Vector2.zero;
    public ParticleSystem mistyParticles;
    public GameObject mistyCloud;
    public float mistyLandLag;
    float xmove = 0;
    float ymove = 0;
    public void MistyStep()
    {
        if (controls.SpecialAttackStartState && canMisty)// reset when character is grounded
        {
            animScript.specialing = true;


            if (controls.HorizontalMove > 0)
                xmove = 1;
            else if (controls.HorizontalMove < 0)
                xmove = -1;
            else
                xmove = 0;
            if (controls.VerticalMove > 0)
                ymove = 1;
            else if (controls.VerticalMove < 0)
                ymove = -1;
            else
                ymove = 0;
            canMisty = false;
            Debug.Log("xmove; "+ xmove+ " ymove: "+ ymove+ " HORI: "+controls.HorizontalMove+ " VERT: "+controls.VerticalMove);
            
            moveScript.canMove = false;
            animScript.disableAirDrag();
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 2);

            TPdirection = new Vector2(xmove,ymove);
            if (TPdirection == Vector2.zero)
                TPdirection = new Vector2(0, 1);
            TPdirection = TPdirection.normalized * TPforce;
            jumpingRB.drag = 2;
            charRb.velocity = Vector2.zero;
            jumpingRB.velocity = Vector2.zero;
            charRb.AddForce(new Vector2(TPdirection.x*1.4f, 0), ForceMode2D.Impulse);
            
            jumpingRB.AddForce(new Vector2(0, TPdirection.y*.45f ), ForceMode2D.Impulse);

            moveScript.landLag = mistyLandLag;


        }
         
    }
    public void flipKick()
    {
        
        if (controls.SpecialAttackStartState && canMisty)// reset when character is grounded{
        {
            Debug.Log("flipKick");
            if (moveScript.facingRight)
            {
                if (controls.HorizontalMove < 0)
                {
                    xmove = -1;
                    moveScript.flip();
                }                    
                else
                    xmove = 1;
            }
            else
            {
                if (controls.HorizontalMove > 0)
                {
                    xmove = 1;
                    moveScript.flip();
                }
                else
                    xmove = -1;               
            }
      
            canMisty = false;

            moveScript.canMove = false;
            animScript.disableAirDrag();
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 2);
            TPdirection = new Vector2(xmove, 0) * TPforce;
            jumpingRB.drag = 3;
            charRb.velocity = Vector2.zero;
            jumpingRB.velocity = Vector2.zero;
            charRb.AddForce(new Vector2(TPdirection.x * 1.4f, 0), ForceMode2D.Impulse);

            //jumpingRB.AddForce(new Vector2(0, TPdirection.y * .45f), ForceMode2D.Impulse);

            moveScript.landLag = mistyLandLag;
        }
    }
    void endFlipKick()
    {
        moveScript.canMove = true;
    }
    public void endMistyStep()
    {
        moveScript.canMove = true;
        charRb.velocity = new Vector2(xmove*2f, controls.VerticalMove);
        jumpingRB.velocity = new Vector2(0,ymove*4);
        
    }
    void PLayMistyParticles()
    {
        GameObject cloud = null;
        cloud = Instantiate(mistyCloud, transform.position, Quaternion.identity);
        cloud.transform.GetChild(0).transform.position = new Vector3(characterSprite.transform.position.x+.5f, characterSprite.transform.position.y+1.5f, 0);
        mistyParticles.Play();
    }
    [Header("AirDaggers")]
    public GameObject airDagger;
    public Transform daggerSpawn;
    void airDaggers()
    {
        if (controls.SpecialAttackStartState && !animScript.specialing)
        {
            
            GameObject dagger = Instantiate(airDagger, transform.position, Quaternion.identity);           
            if (!moveScript.facingRight)         
                dagger.transform.Rotate(0, 180, 0);
            dagger.GetComponent<angleProjectile>().SetProjectile(daggerSpawn, charRb.transform, -30);
            
            dagger = Instantiate(airDagger, transform.position,Quaternion.identity);           
            if (!moveScript.facingRight)
                dagger.transform.Rotate(0, 180, 0);
            dagger.GetComponent<angleProjectile>().SetProjectile(daggerSpawn, charRb.transform, -45);
            
            dagger = Instantiate(airDagger, transform.position, Quaternion.identity);            
            if (!moveScript.facingRight)
                dagger.transform.Rotate(0, 180, 0);
            dagger.GetComponent<angleProjectile>().SetProjectile(daggerSpawn, charRb.transform, -70);

            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 2);
        }
    }
    [Header("GroundDaggers")]
    public GameObject groundDag;
    void groundDagger()
    {
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold)
        {
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 0);
                            
        }            
    }

    void fireDagger()//called from the throw animation
    {
        GameObject dagger = Instantiate(groundDag, transform.position, Quaternion.identity);
        if (!moveScript.facingRight)
        {
            dagger.transform.Rotate(0, 180, 0);
            dagger.GetComponent<lineProjectile>().speed *= -1;
        }
    }
    void animJumpingOn()
    {
        moveScript.jumpableAnim = true;
    }
    void animJumpingOff()
    {
        moveScript.jumpableAnim = false;
    }
    void daffy()
    {
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) > sideThreshold)
        {
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 1);
        }
    }

    [Header("Erupt")]
    public GameObject smallErupt;
    public GameObject bigErupt;
    GameObject currentErupt;
    float scale = 1;
    public Vector2 rockOffset;
    public Animator crackedGroundAnim;

    void erupt()
    {
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold && !anim.GetBool("attacking"))
        {
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 0);
            wasCharging = true;
            crackedGroundAnim.speed = 1;
        }
        else if (!controls.SpecialAttackState && animScript.specialing)
        {
            if (wasCharging)
            {
                wasCharging = false;
                
                anim.SetTrigger("special");
                
            }
        }
        if (anim.GetBool("hit")) 
        {
            
            animScript.specialing = false;
            wasCharging = false;
        }

        
    }
    private void setErupt(int level)
    {
        if(level == 0)
        {
            currentErupt = smallErupt;
            scale = .9f;
        }
        else if(level == 1)
        {
            currentErupt = smallErupt;
            scale = 1.1f;
        }
        else if (level == 2)
        {
            currentErupt = bigErupt;
            scale = 1.05f;
        }
        else if (level == 3)
        {
            currentErupt = bigErupt;
            scale = 1.3f;
        }
    }
    void pauseCracked()
    {
        crackedGroundAnim.speed = 0;
    }
    void spawnRock()
    {
        
        Vector3 rockPos;
        if (moveScript.facingRight)
            rockPos = new Vector3(transform.position.x+rockOffset.x, transform.position.y+rockOffset.y, 0);
        else
            rockPos = new Vector3(transform.position.x - rockOffset.x, transform.position.y + rockOffset.y, 0);

        GameObject rock = Instantiate(currentErupt, rockPos, Quaternion.identity);
        if(moveScript.facingRight)
            rock.transform.localScale = new Vector3 (scale,scale,scale);
        else
        {
            rock.transform.localScale = new Vector3(scale*-1, scale, scale);
        }
    }


}
