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
            if (moveScript.onBase)
            {
                Fireball();
                if (animScript.attacking == false)
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
    
    public float sideThreshold = .1f;
    public void Fireball()
    {
        if(controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold && !animScript.specialing)
        {
            Debug.Log("fireball A");
            
            wasCharging = true;
            moveScript.canMove = false;
            anim.SetTrigger("special");
            anim.SetFloat("specialNum", 0);
        }
        if (controls.SpecialAttackState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold)
        {
            
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
        else if (!controls.SpecialAttackState && animScript.specialing)
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
    
    float xmove = 0;
    float ymove = 0;
    public void MistyStep()
    {
        if (controls.SpecialAttackStartState && canMisty)// reset when character is grounded
        {



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
            jumpingRB.drag = 3;
            charRb.velocity = Vector2.zero;
            jumpingRB.velocity = Vector2.zero;
            charRb.AddForce(new Vector2(TPdirection.x*1.2f, 0), ForceMode2D.Impulse);
            
            jumpingRB.AddForce(new Vector2(0, TPdirection.y*.5f ), ForceMode2D.Impulse);
                
            


        }
         
    }

    public void endMistyStep()
    {
        moveScript.canMove = true;
        charRb.velocity = new Vector2(xmove*2f, controls.VerticalMove);
        jumpingRB.velocity = new Vector2(0,ymove*4);
    }

}
