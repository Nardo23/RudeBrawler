using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDamage : MonoBehaviour
{
    public bool isPlayer;

    public int damage; // value changed by different attack animations

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.tag == "EnemyHurt")
            {
                
                enemyAnimator u = collision.gameObject.GetComponentInParent<enemyAnimator>();
                if (u != null)
                {
                    //Debug.Log("ggg");                  
                    u.hurt(transform.position.x);
                }
                enemyHealth h = collision.gameObject.GetComponentInParent<enemyHealth>();
                if(h != null)
                {
                    
                    h.changeHealth(-damage);
                }


            }
        }
        else
        {
            if (collision.tag == "PlayerHurt")
            {
                AnimtorController c = collision.gameObject.GetComponentInParent<AnimtorController>();
                if (c != null)
                {
                    c.hurt(transform.position.x);
                }
                PlayerHealth p = collision.gameObject.GetComponentInParent<PlayerHealth>();
                if (p != null)
                {
                    p.changeHealth(-damage);
                }

            }
        }
        
    }
}
