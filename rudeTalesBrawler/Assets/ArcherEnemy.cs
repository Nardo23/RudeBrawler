using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    Transform target;
    
    GameObject[] players;
    public GameObject ThrowingSprite, SpawnPoint;
    public GameObject shadow;
    // Start is called before the first frame update
    public GameObject projectile;
    public float rotationSpeed;
    public bool rotating = false;
    public float rotationOffset;
    public Vector3 projectileSize;
    public GameObject rootObject;
    float initialxScale;
    public float TargetRange;
    bool idle =false;
    float xOffset, yOffset;
    public float maxOffset;

    Vector3 throwingSpriteStartPos;
    Animator anim;
    levelManager levelManagerScript;
    bool started = false;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        initialxScale = rootObject.transform.localScale.x;
        anim = GetComponent<Animator>();
        throwingSpriteStartPos = ThrowingSprite.transform.localPosition;
    }
    void randomTarget()
    {
        players = levelManagerScript.livingPlayers;

        target = players[Random.Range(0, players.Length)].transform;
        if(!idle && Vector2.Distance(transform.position, target.transform.position) > TargetRange)
        {
            foreach (GameObject obj in players)
            {
                if(Vector2.Distance(transform.position, obj.transform.position) <= TargetRange)
                {
                    target = obj.transform;
                    break;
                }
            }
        }
        xOffset = ((target.transform.position.x - transform.position.x) / TargetRange)*maxOffset;
        yOffset = ((target.transform.position.y - transform.position.y) / (TargetRange*.6f ))*maxOffset * .4f;
        Debug.Log("arrowOffsets: " + xOffset + ", " + yOffset);
         //(target.transform.position.x + xOffset, target.transform.position.y+yOffset, target.transform.position.z);
        if(target.position.x > transform.position.x)
        {
            rootObject.transform.localScale = new Vector3 (initialxScale * -1, rootObject.transform.localScale.y, rootObject.transform.localScale.z);
            //ThrowingSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            rootObject.transform.localScale = new Vector3(initialxScale, rootObject.transform.localScale.y, rootObject.transform.localScale.z);
            //ThrowingSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void shoot()
    {
        GameObject go =Instantiate(projectile, SpawnPoint.transform.position, Quaternion.identity );
        go.GetComponent<projectile>().fire(target, ThrowingSprite.transform,shadow.transform, xOffset,yOffset);
        
        go.transform.localScale = projectileSize;
        go.GetComponent<projectile>().shadow.transform.localScale = projectileSize;
        if(rootObject.transform.localScale.x < 0)
        {
            go.transform.localScale = new Vector3(projectileSize.x * -1, projectileSize.z*-1, projectileSize.z);
            go.GetComponent<projectile>().shadow.transform.localScale = new Vector3(projectileSize.x * -1, projectileSize.z * -1, projectileSize.z);
        }
    }

    void rotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - ThrowingSprite.transform.position.y, target.position.x - ThrowingSprite.transform.position.x) * Mathf.Rad2Deg;
        angle = angle + rotationOffset;
        if(rootObject.transform.localScale.x <0)
        {
            angle-= 180;
        }
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        ThrowingSprite.transform.rotation = Quaternion.RotateTowards(ThrowingSprite.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
        //ThrowingSprite.transform.rotation = Quaternion.Euler(0, 0, ThrowingSprite.transform.rotation.z);
    }

    void rotateOn()
    {
        rotating = true;
    }
    void rotateOff()
    {
        rotating=false;
    }


    // Update is called once per frame
    void Update()
    {
        int inRangeCount = 0;
        foreach (GameObject obj in players)
        {
            if(Vector2.Distance(transform.position, obj.transform.position)<= TargetRange)
            {
                inRangeCount++;
            }
        }
        if (inRangeCount > 0)
        {
            idle = false;
        }
        else
        {
            idle = true;
        }
        anim.SetBool("idle", idle);
        
        if (!idle)
        {
            if (rotating)
            {
                rotateToTarget();
            }
            else
            {
                ThrowingSprite.transform.rotation = Quaternion.RotateTowards(ThrowingSprite.transform.rotation, Quaternion.identity, rotationSpeed * 1.5f * Time.deltaTime);
            }
        }

        ThrowingSprite.transform.localPosition = throwingSpriteStartPos;
    }
}
