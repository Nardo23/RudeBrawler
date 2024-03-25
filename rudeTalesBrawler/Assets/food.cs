using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    public int health = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerHealth p = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if (p != null)
            {
                p.changeHealth(health);
                Destroy(this.gameObject);
            }
        }
    }
}
