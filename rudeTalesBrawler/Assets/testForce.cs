using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testForce : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
            
        }
    }
}
