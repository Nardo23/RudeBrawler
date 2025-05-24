using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corn : MonoBehaviour
{
    public Vector3 scale;
    Animator anim;

    private void Start()
    {
        transform.localScale = scale;
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Chicken")
        {
            collision.GetComponent<WanderingAI>().timer = 99;
            anim.Play("CornExplode");
        }
    }

    void despawn()
    {
        Destroy(this.gameObject);
    }

}
