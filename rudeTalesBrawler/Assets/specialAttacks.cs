using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specialAttacks : MonoBehaviour
{
    public PlayerInput input;
    Controls controls = new Controls();
    public CharacterMovement moveScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        controls = input.GetInput();
        Fireball();
    }


    public float  minSpeed,maxSpeed, minLifeTime, maxLifetime;
    public int MinFireballDamage, MaxFireballDamage;
    float  speed, lifetime;
    int damage;
    Vector2 size;
    public float MaxChargeTime;
    float chargeTime;
    public Vector2 minBallSize, maxBallSize;
    bool wasCharging = false;
    public GameObject projectile;
    public GameObject chargeSprite;
    void Fireball()
    {
        if (controls.SpecialAttackState)
        {
            chargeSprite.SetActive(true);
            wasCharging = true;
            if(chargeTime< MaxChargeTime)
            {
                chargeTime += Time.deltaTime;
                damage = (int) Mathf.Lerp(MinFireballDamage, MaxFireballDamage, chargeTime / MaxChargeTime);
                speed = Mathf.Lerp(minSpeed, maxSpeed, chargeTime / MaxChargeTime);
                lifetime = Mathf.Lerp(minLifeTime, maxLifetime, chargeTime / MaxChargeTime);
                size = Vector2.Lerp(minBallSize, maxBallSize, chargeTime / MaxChargeTime);
                chargeSprite.transform.localScale =  size*.5f;
            }
            else
            {
                damage = MaxFireballDamage;
                speed = maxSpeed;
                lifetime = maxLifetime;
                size = maxBallSize;
            }
            
        }
        else
        {
            
            if (wasCharging)
            {
                wasCharging = false;
                //fire;
                GameObject ball = Instantiate(projectile, transform.position, Quaternion.identity);
                ball.transform.localScale = ball.transform.localScale*size;
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

            }
        }
        
    }


}
