using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class WanderingAI : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;
    float actualWanderTimer;
    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    public Animator anim;
    bool moving = true;
    float speed;
    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        setDestination();
        setTimer();
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime;

            if (timer >= actualWanderTimer)
            {
                
                timer = 0;
                moving = false;
            }
            if(Vector3.Distance(transform.position, agent.destination) < .65f)
            {
                setDestination();
                setTimer();
            }
            if (transform.position.x < agent.destination.x)
                transform.localScale = new Vector3(1, 1, 1);
            else 
                transform.localScale = new Vector3(-1, 1, 1);


        }
        else
        {
            agent.speed = 0;
        }
        anim.SetBool("Moving", moving);
    }

    void setDestination()
    {
        
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        if (Vector3.Distance(newPos, transform.position) < 4)
            setDestination();
    }
    void setTimer()
    {
        actualWanderTimer = wanderTimer * Random.Range(.65f, 1.35f);
    }

    void StartMoving()
    {
        moving = true;
        setDestination();
        agent.speed = speed;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}