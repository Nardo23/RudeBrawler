using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialAttacks : MonoBehaviour
{
    public PlayerInput input;
    Controls controls = new Controls();
    public CharacterMovement moveScript;
    AnimtorController animScript;
    Animator anim;
    float cooldown = .26f;
    bool onCooldown = false;
    float coolTimer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animScript = GetComponent<AnimtorController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
           controls = input.GetInput();

           if (!onCooldown)
           {
               Fireball();
               if(controls.AttackState)
                   StartCoroutine(punch());
           }
           else
           {
               if (coolTimer < cooldown)           
                   coolTimer += Time.deltaTime;

               else
                   onCooldown = false;
           }
        */
        controls = input.GetInput();
        if (animScript.CharacterID == "D")
        {

            Fireball();
            if(animScript.attacking ==false)
                lightning();
        }

    }


    public float minSpeed, maxSpeed, minLifeTime, maxLifetime;
    public int MinFireballDamage, MaxFireballDamage;
    float speed, lifetime;
    int damage;
    Vector2 size;
    public float MaxChargeTime;
    float chargeTime;
    public Vector2 minBallSize, maxBallSize;
    bool wasCharging = false;
    public GameObject projectile;
    public GameObject chargeSprite;
    public float sideThreshold = .1f;
    public void Fireball()
    {
        if(controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) <= sideThreshold)
        {
            Debug.Log("fireball A");
            chargeSprite.SetActive(true);
            wasCharging = true;
            moveScript.canMove = false;
            anim.SetTrigger("special");
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
                chargeSprite.transform.localScale = size * .5f;
            }
            else
            {
                damage = MaxFireballDamage;
                speed = maxSpeed;
                lifetime = maxLifetime;
                size = maxBallSize;
            }

        }
        else if (!controls.SpecialAttackState)
        {
            
            if (wasCharging)
            {
                Debug.Log("fireball B");
                wasCharging = false;
                //fire;
                anim.SetTrigger("special");
                GameObject ball = Instantiate(projectile, transform.position, Quaternion.identity);
                ball.transform.localScale = ball.transform.localScale * size;
                if (!moveScript.facingRight)
                {
                    ball.transform.Rotate(0, 180, 0);
                    speed = -speed;
                }
                lineProjectile linePro = ball.GetComponent<lineProjectile>();
                hitboxDamage DamageScript = ball.GetComponent<hitboxDamage>();
                DamageScript.damage = damage;
                linePro.speed = speed;
                linePro.lifeTime = lifetime;

                //reset values to minimum
                chargeSprite.SetActive(false);
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

    public GameObject lightningBolt;
    public float lightningXPos;
    public void lightning()
    {
        if (controls.SpecialAttackStartState && Mathf.Abs(controls.HorizontalMove) > sideThreshold)
        {
            Debug.Log("lightning");

            moveScript.canMove = false;
            anim.SetTrigger("sideSpecial");
            if (controls.HorizontalMove > 0)
            {
                Instantiate(lightningBolt, new Vector3(transform.position.x + lightningXPos,transform.position.y,0),Quaternion.identity); // lightning bolt to the right
            }
            else
            {
                Instantiate(lightningBolt, new Vector3(transform.position.x - lightningXPos, transform.position.y, 0), Quaternion.identity); // lightning bolt to the left
            }
        }
    }

}
