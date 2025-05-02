using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 direction;
    public float speed = 2;
    public Vector2 moveTime; // min and max for random moveTime
    float curMoveTime;
    float timer = 0;
    bool moving = true;
    public float magnitude = 10;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pickDirection();
        pickMoveTime();
    }

    void pickDirection()
    {
        if (Random.Range(1f, 10f) < 5)
            direction = new Vector2(1, Random.Range(-.5f, .5f)).normalized;
        else
            direction = new Vector2(Random.Range(-.5f, .5f), 1).normalized;
    }
    void pickMoveTime()
    {
        curMoveTime = Random.Range(moveTime.x, moveTime.y);
    }
    void startMoving() // call this from idle animation
    {
        moving = true;
        pickMoveTime();
        pickDirection();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Debug.Log("direction: "+ direction);
            if(timer < curMoveTime)
            {
                timer += Time.deltaTime;
                Vector2 targetVel = direction * speed * Time.deltaTime;
                rb.velocity = Vector2.Lerp(rb.velocity, targetVel, 10*Time.deltaTime);
                if (rb.velocity.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);

            }
            else
            {
                moving = false;
                rb.velocity = Vector2.zero;
            }
            
        }
        anim.SetBool("Moving", moving);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (moving)
        {
            // if (direction.x > direction.y)
            // direction = new Vector2(direction.x * Random.Range(-.6f, -1.4f), direction.y * Random.Range(.6f, 1.4f)).normalized;
            //else
            //direction = new Vector2(direction.x * Random.Range(.6f, 1.4f), direction.y * Random.Range(-.6f, -1.4f)).normalized;

            //var force = transform.position - collision.transform.position;
            //force.Normalize();
            //direction = direction.normalized * force*1.2f;


            // normalize force vector to get direction only and trim magnitude

            // rb.AddForce(force, ForceMode2D.Impulse);

            direction = direction*Random.Range(-.4f,-1.6f);
            direction = direction.normalized;
        }
        
    }

}
