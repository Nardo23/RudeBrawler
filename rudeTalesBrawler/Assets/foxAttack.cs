using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foxAttack : MonoBehaviour
{
    public Animator anim;
   
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Chicken")
        {
            anim.SetTrigger("Attack");
        }
    }
}
