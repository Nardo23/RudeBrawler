using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDamage : MonoBehaviour
{
    public bool isPlayer;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.tag == "EnemyHurt")
            {
                
                enemyAnimator u = collision.gameObject.GetComponentInParent<enemyAnimator>();
                if (u != null)
                {
                    Debug.Log("ggg");                  
                    u.hurt(transform.position.x);
                }

            }
        }
        else
        {
            if (collision.tag == "PlayerHurt")
            {

            }
        }
        
    }
}
