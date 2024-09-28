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
    public float gravityScale = -3f;
    public Vector3 charDefaultRelPos;
    private Vector3 baseDefPos;
    [SerializeField] public bool onBase = false;
    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask detectLayer;
    public bool inRange = false;
    [HideInInspector] public bool facingRight = false;
    [SerializeField] GameObject[] players;
    bool started = false;
    levelManager levelManagerScript;
    public float knockbackMultiplyer = 1;
    bool Stoped = false;
    public bool isBearOwl = false;
    float specialTimer =5f;
    public float SpecialTime =10;
    public bool doingSpecial;
    float acceleration;
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
        acceleration = agent.acceleration;
    }

    void randomTarget()
    {
        if (started)
        {
            target = levelManagerScript.livingPlayers[Random.Range(0, levelManagerScript.livingPlayers.Length)].transform;
        }
        else
        {
            if (players[0] != null)
                target = players[Random.Range(0, players.Length)].transform;
        }
    }
    public void closestTarget() // called when hit by attack
    {
        float dist = Vector2.Distance(transform.position, target.position);
        foreach (GameObject p in players)
        {
            if (Vector2.Distance(p.transform.position, transform.position) < dist)
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
        anim.speed = 0;
        //anim.enabled = false;
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        while (curTime < duration)
        {
            
            //anim.enabled = false;
            curTime += Time.deltaTime;
            yield return null;
        }
        anim.speed = 1;
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
            rb.AddForce((Vector2.left * force * knockbackMultiplyer), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce((Vector2.right * force * knockbackMultiplyer), ForceMode2D.Impulse);
        }

        //Debug.Log("KNOCKED "+ force * 1 * knockbackMultiplyer);
    }
    private void Update()
    {
        if (!Stoped)
        {
            started = true;

            if (target.GetComponentInChildren<AnimtorController>() != null)
            {
                if (target.GetComponentInChildren<AnimtorController>().alive == false) //if target is dead pick a new target
                {
                    randomTarget();
                }
            }


            if (!onBase && charRB.velocity.y <= 4) //pay attention to the float used to determine when to check for base
            {
                detectBase();
            }
            if (isInteracting && agent.enabled && !SpecialMoving && !liningUp)
            {
                gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                charRB.velocity = Vector2.zero;
            }
            else
            {
                agent.enabled = true;
                gameObject.GetComponent<NavMeshAgent>().isStopped = false;
                if(!doingSpecial)
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
            if (isBearOwl)
            {
                bearOwlSpecial();
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
        if (target.transform.position.x < transform.position.x)
        {
            faceLeft();
        }
        if (target.transform.position.x > transform.position.x)
        {
            faceRight();
        }
    }
    void movement()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        float distancex = Mathf.Abs(transform.position.x - target.position.x);
        float distancey = Mathf.Abs(transform.position.y - target.position.y);
        if (distancex > attackDistance || distancey > attackDistance * .3f)
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

    public bool StartSpecial = false;
    Vector3 BearTargetPos, BearStartPos;
    public bool SpecialMoving = false;
    public bool arrived = false;
    public float bearFlySpeed = 10f;
    public bool forceEnd=false;
    bool liningUp = false;
    void bearOwlSpecial()
    {
        //Debug.Log("specialTimer: " + specialTimer);
        
        if (specialTimer < SpecialTime)
        {
             if (!doingSpecial) 
                specialTimer += Time.deltaTime;
        }
        else
        {
            if(!doingSpecial)
                bearTargeting();

        }
        if (StartSpecial)
        {
            float xMod, yMod =0;
            if (BearTargetPos.x > transform.position.x)
                xMod = +3;
            else
                xMod = -3;
            
            BearTargetPos = new Vector3(BearTargetPos.x + xMod, BearTargetPos.y + yMod, 0);

                specialTimer = 0f;
            isInteracting = true;
            StartSpecial = false;
            BearStartPos = transform.position;
            liningUp = true;
            doingSpecial = true;  //turned off by animatorScript function when special animation ends
            //Debug.Log("EnemySpecialStart !!!!");
        }
        if (doingSpecial )
        {
            //agent.enabled = false;
            if (SpecialMoving) 
            {
                agent.acceleration = 50;
                liningUp = false;
                //Vector3.MoveTowards(transform.position, BearTargetPos, (Vector3.Distance(transform.position, BearTargetPos) * .5f) * Time.deltaTime);
                agent.SetDestination(BearTargetPos);
                agent.speed = (Vector3.Distance(BearStartPos, BearTargetPos)+4);
                if (forceEnd)
                    agent.speed = 0;
                if (Vector3.Distance(transform.position, BearTargetPos) < 2)
                {
                    arrived = true;
                }
            }
            else
            {
                Debug.Log("lining up");
                agent.SetDestination(new Vector3 (transform.position.x, BearTargetPos.y,0));
                agent.speed = 5;
            }
            

        }
        else
        {
            agent.enabled = true;
            agent.acceleration = acceleration;
        }
        
        void bearTargeting()
        {
            if (Vector3.Distance(transform.position, target.position) > 9 && Mathf.Abs(transform.position.y - target.position.y) <5)
            {
                BearTargetPos = target.position;
                StartSpecial = true;
            }
            if (levelManagerScript.livingPlayers.Length > 1)
            {
                foreach (GameObject p in levelManagerScript.livingPlayers)
                {
                    foreach (GameObject p2 in levelManagerScript.livingPlayers)
                    {
                        if (p2 != p)
                        {
                            if (Vector3.Distance(p.transform.position, p2.transform.position) < 7.5f)
                            {
                                BearTargetPos = new Vector3((p.transform.position.x + p2.transform.position.x) * .5f, (p.transform.position.y + p2.transform.position.y) * .5f, 0);

                                if (Vector3.Distance(transform.position, BearTargetPos) > 13 && Mathf.Abs(transform.position.y - BearTargetPos.y) < 5)
                                    StartSpecial = true;

                                foreach (GameObject p3 in levelManagerScript.livingPlayers)
                                {
                                    if (p3 != p && p3 != p2)
                                    {
                                        if (Vector3.Distance(p3.transform.position, BearTargetPos) < 5 && Mathf.Abs(transform.position.y - BearTargetPos.y) < 5)
                                        {
                                            BearTargetPos = new Vector3((p.transform.position.x + p2.transform.position.x + p3.transform.position.x) * .3333f, (p.transform.position.y + p2.transform.position.y + p3.transform.position.y) * .3333f, 0);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                if (!StartSpecial)
                {
                    foreach (GameObject p in levelManagerScript.livingPlayers)
                    {
                        if (Vector3.Distance(transform.position, p.transform.position) > 9)
                        {
                            BearTargetPos = p.transform.position;
                            StartSpecial = true;
                        }
                    }
                }

            }

        }

    }
}
