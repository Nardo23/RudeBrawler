using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineProjectile : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public bool hitOnce;
    public bool isPlayer;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < lifeTime)
        {
            timer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            fizzle();
        }


    }

    void hit() // projectile hits a target and dosnt keep going
    {
        Destroy(this.gameObject);
    }
    void fizzle() //  projectile expires without hitting anything
    {
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitOnce)
        {
            if (isPlayer)
            {
                if (collision.tag == "EnemyHurt")
                {
                    hit();
                }
            }
            else
            {
                if (collision.tag == "PlayerHurt")
                {
                    hit();
                }
            }
        }
        
        
    }
}
