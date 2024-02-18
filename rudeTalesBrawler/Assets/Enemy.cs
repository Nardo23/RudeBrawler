using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
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
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        AgentSpeed = agent.speed;
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (!onBase && charRB.velocity.y <= 4) //pay attention to the float used to determine when to check for base
        {
            detectBase();
        }
        if (isInteracting)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            //charRB.velocity = Vector2.zero;
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            movement();
        }


        if (onBase)
        {
            if (agent.velocity.x<0 )
            {

                faceLeft();
                
            }
            else if (agent.velocity.x >0)
            {

                faceRight();

            }
        }

    }

    public void faceLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void faceRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void movement()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackDistance)
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
        if (!onBase)
        {
            charRB.isKinematic = false;
            charRB.gravityScale = gravityScale;

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
            Debug.Log("setting velocity to zero");
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
