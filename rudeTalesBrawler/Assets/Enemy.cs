using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Rigidbody2D rb;
    float AgentSpeed;
    public float attackDistance;
    public bool isInteracting = false;
    [SerializeField] private Rigidbody2D charRB;
    public float gravityScale= -3f;
    public Vector3 charDefaultRelPos; 
    private Vector3 baseDefPos;
    [SerializeField] public bool onBase = false;
    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask detectLayer;
    public bool inRange = false;
    [HideInInspector]public bool facingRight = false;
    [SerializeField] GameObject[] players;
    bool started = false;
    levelManager levelManagerScript;
    public float knockbackMultiplyer =1;
    bool Stoped = false;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        randomTarget();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        AgentSpeed = agent.speed;
        rb = GetComponent<Rigidbody2D>();
        charRB.gravityScale = gravityScale;
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
    }

    void randomTarget()
    {     
        if (started)
        {
            target = levelManagerScript.livingPlayers[Random.Range(0, levelManagerScript.livingPlayers.Length)].transform;
        }
        else
        {
            if(players[0]!=null)
                target = players[Random.Range(0, players.Length)].transform;
        }
    }
    public void closestTarget() // called when hit by attack
    {
        float dist = Vector2.Distance(transform.position, target.position);
        foreach (GameObject p in players)
        {
            if( Vector2.Distance(p.transform.position, transform.position) < dist)
            {
                dist = Vector2.Distance(p.transform.position, transform.position);
                target = p.transform;
            }
        }
    }

    public void enableHitStop(float duration, Animator anim, float knock)
    {
        StartCoroutine(hitStop(duration, anim, knock));
    }
    IEnumerator hitStop(float duration, Animator anim, float knock)
    {
        float curTime = 0f;
        Stoped = true;
        rb.isKinematic = true;
        charRB.isKinematic = true;
        anim.enabled = false;
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            yield return null;
        }
        
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        anim.enabled = true;
        rb.isKinematic = false;
        Stoped = false;
        knockback(knock);
        yield return null;
    }

    public void knockback(float force)
    {
        if (facingRight)
        {
            rb.AddForce((Vector2.left * force*knockbackMultiplyer), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce((Vector2.right * force*knockbackMultiplyer), ForceMode2D.Impulse);
        }
        
        //Debug.Log("KNOCKED "+ force * 1 * knockbackMultiplyer);
    }
    private void Update()
    {
        if (!Stoped)
        {
            started = true;

            if(target.GetComponentInChildren<AnimtorController>() != null)
            {
                if (target.GetComponentInChildren<AnimtorController>().alive == false)
                {
                    randomTarget();
                }
            }
            

            if (!onBase && charRB.velocity.y <= 4) //pay attention to the float used to determine when to check for base
            {
                detectBase();
            }
            if (isInteracting)
            {
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                charRB.velocity = Vector2.zero;
            }
            else
            {
                gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                movement();
            }


            if (onBase)
            {
                if (agent.velocity.x < 0)
                {

                    faceLeft();

                }
                else if (agent.velocity.x > 0)
                {

                    faceRight();

                }
            }
           else
            {
                charRB.isKinematic = false;
                charRB.gravityScale = gravityScale;

            }

            if (charRB.transform.localPosition != charDefaultRelPos)
            {
                //print("pos diff- local: " + charRB.transform.localPosition + "  --default: " + charDefaultRelPos );
                var charTransform = charRB.transform;
                charTransform.localPosition = new Vector2(charDefaultRelPos.x,
                    charTransform.localPosition.y);
            }

        }
        
        

    }

    public void faceLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        facingRight = false;
    }
    public void faceRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        facingRight = true;
    }

    public void faceTarget()
    {
        if(target.transform.position.x < transform.position.x)
        {
            faceLeft();
        }
        if(target.transform.position.x > transform.position.x)
        {
            faceRight();
        }
    }
    void movement()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float distancex = Mathf.Abs(transform.position.x - target.position.x);
        float distancey = Mathf.Abs(transform.position.y - target.position.y);
        if (distancex > attackDistance || distancey > attackDistance*.3f)
        {
            agent.SetDestination(target.position);
            agent.speed = AgentSpeed;
            inRange = false;
        }
        else
        {
            agent.speed = 0;
            inRange = true;
        }
        
        
    }

    void detectBase()
    {
        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.position, -Vector2.up, detectionDistance, detectLayer);
        if (hit.collider != null)
        {
            onBase = true;
            charRB.isKinematic = true;
            //currentJumps = 0;
            charRB.transform.localPosition = new Vector3(charRB.transform.localPosition.x, charDefaultRelPos.y, charRB.transform.localPosition.z);
            charRB.velocity = Vector2.zero;
            rb.velocity = Vector2.zero;

            charRB.gravityScale = 0;
            //Debug.Log("setting velocity to zero");
        }
        if (charRB.transform.localPosition.y + charDefaultRelPos.y < rb.transform.localPosition.y)
        {
            charRB.transform.localPosition = new Vector3(charRB.transform.localPosition.x, charDefaultRelPos.y, charRB.transform.localPosition.z);
            onBase = true;
            charRB.isKinematic = true;
            charRB.gravityScale = 0;
            //currentJumps = 0;
            //Debug.Log("Gobbbbbb");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject);
    }

}
