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
    public GameObject spawnOnHit;
    public float yRange;
    public bool checkFromParent = true;
    public GameObject checkObject;
    Transform yCheckTransform;
    public GameObject flyingObj;
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
        if(spawnOnHit!= null)
        {
            GameObject splode = Instantiate(spawnOnHit, transform.position, Quaternion.identity);
            if(transform.rotation !=Quaternion.identity)
            {
                splode.transform.Rotate(0, 180, 0);
            }
        }
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
                if (collision.tag == "EnemyHurt" || collision.tag == "BasicHurt"|| collision.tag == "BossHurt"|| collision.tag == "SimpleEnemyHurt" )
                {
                    if (checkFromParent)
                    {
                        yCheckTransform = transform.parent.transform;
                    }
                    else
                    {
                        yCheckTransform = checkObject.transform;
                    }
                    if (collision.tag == "BossHurt")
                    {
                        boss bossS = collision.GetComponentInParent<boss>();
                        if (Mathf.Abs(bossS.transform.position.y - yCheckTransform.position.y) <= yRange + bossS.bonusSize)
                            hit();
                    }
                    else if (collision.tag == "EnemyHurt")
                    {
                        if (Mathf.Abs(collision.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange + collision.GetComponentInParent<enemyAnimator>().bonusYSize)
                            hit();
                    }
                    else if(collision.tag == "SimpleEnemyHurt")
                    {
                        if (Mathf.Abs(collision.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange + collision.GetComponentInParent<simpleEnemyAnimator>().bonusYSize)
                            hit();
                    }
                    else if (Mathf.Abs(collision.transform.parent.transform.position.y - yCheckTransform.position.y) <= yRange )// check if bases are within a certain y range
                    {
                        hit();
                    }
                        
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
