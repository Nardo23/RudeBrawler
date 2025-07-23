using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airGust : MonoBehaviour
{
    public float force;
    public GameObject character;
    public float yRange, deflectionyRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "EnemyHurt")
        {
            Debug.Log("blowing");
            Enemy enemyScrpt = collision.GetComponentInParent<Enemy>();
            if (enemyScrpt != null)
            {              
                if (Mathf.Abs(enemyScrpt.transform.position.y - character.transform.position.y) <= yRange)// check if bases are within a certain y range
                {
                    Vector2 gustDirection = new Vector2(enemyScrpt.gameObject.transform.position.x - character.transform.position.x, enemyScrpt.gameObject.transform.position.y - character.transform.position.y).normalized;

                    enemyScrpt.gameObject.GetComponent<Rigidbody2D>().AddForce((gustDirection * force * enemyScrpt.knockbackMultiplyer), ForceMode2D.Force);
                }
            }
        }
        if(collision.tag == "BasicHurt")
        {
            if(Mathf.Abs(collision.transform.parent.position.y - character.transform.position.y) <= yRange)
            {
                Vector2 gustDirection = new Vector2(collision.transform.parent.position.x - character.transform.position.x, collision.transform.parent.position.y - character.transform.position.y).normalized;
                collision.GetComponentInParent<Rigidbody2D>().AddForce((gustDirection * force * collision.GetComponentInParent<basicEnemyHealth>().knockbackMultiplyer), ForceMode2D.Force);
            }
        }

    }

    public GameObject reflectProjectile;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ProjectileEnetered"+ collision);
        if (collision.tag == "EnemyProjectile")
        {
            
            projectile enemyProjectileScrpt = collision.GetComponentInParent<projectile>();           

            if (Mathf.Abs(enemyProjectileScrpt.shadow.transform.position.y - character.transform.position.y) <= deflectionyRange)// check if bases are within a certain y range
            {
                GameObject reflectedPro = Instantiate(reflectProjectile, enemyProjectileScrpt.shadow.transform.position, Quaternion.identity);
                reflectedPro.GetComponent<lineProjectile>().flyingObj.transform.position = new Vector3(reflectedPro.transform.position.x, enemyProjectileScrpt.projectileObj.transform.position.y, 0);

                if (!character.GetComponent<CharacterMovement>().facingRight)
                {
                    reflectedPro.GetComponent<lineProjectile>().speed *= -1;
                    reflectedPro.transform.localScale = new Vector3(reflectedPro.transform.localScale.x * -1, reflectedPro.transform.localScale.y, reflectedPro.transform.localScale.z);
                }
                
                Destroy(enemyProjectileScrpt.shadow);
                Destroy(enemyProjectileScrpt.gameObject);

            }

        }
    }

}
