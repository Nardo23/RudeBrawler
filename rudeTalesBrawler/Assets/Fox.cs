using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : MonoBehaviour
{
    public string targetTag = "Chicken";
    public float leftBound, rightBound;
    public float topBound, bottomBound;
    public bool goLeft = true;
    Vector3 targetPosition;
    public float targetX, TargetY;
    GameObject[] targets;
    public GameObject currentTarget;
    bool noTargets = true;
    public float ySpeed =3;
    private Transform target;
    private NavMeshAgent agent;
    Vector2 tempTarget;
    // Start is called before the first frame update

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (Mathf.Abs(transform.position.x - leftBound) < Mathf.Abs(transform.position.x - rightBound))
        {
            goLeft = false;
        }
        else
            goLeft = true;
        getTarget();
    }

    void getTarget()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        if(targets.Length == 0)
        {
            noTargets = true;
            tempTarget = new Vector2(0, Random.Range(topBound, bottomBound));
            return;
        }
        if (currentTarget == null)
        {
            currentTarget = targets[0];
        }
        foreach(GameObject obj in targets)
        {
            if(Mathf.Abs(transform.position.y - currentTarget.transform.position.y)> Mathf.Abs(transform.position.y - obj.transform.position.y))
            {
                currentTarget = obj;
            }
        }
        noTargets = false;
    }
    
    private void Update()
    {
        if (Mathf.Abs(transform.position.x - targetX) < 2)
        {
            goLeft = !goLeft;
            getTarget();
        }
        if (goLeft)
        {
            targetX = leftBound;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            targetX = rightBound;
            transform.localScale = new Vector3(1, 1, 1);
        }
                 
        if (!noTargets && currentTarget == null)
            getTarget();
        if (!noTargets)
        {
            tempTarget = Vector2.MoveTowards(transform.position, currentTarget.transform.position, ySpeed * Time.deltaTime);
            
        }
        TargetY = tempTarget.y;
        agent.destination = new Vector3(targetX, TargetY, 0);

    }


}
