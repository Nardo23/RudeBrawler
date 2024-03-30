using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    Transform target;
    
    [SerializeField] GameObject[] players;
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
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        initialxScale = rootObject.transform.localScale.x;
    }
    void randomTarget()
    {
        target = players[Random.Range(0, players.Length)].transform;
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
        go.GetComponent<projectile>().fire(target, ThrowingSprite.transform,shadow.transform);
        
        go.transform.localScale = projectileSize;
        if(rootObject.transform.localScale.x < 0)
        {
            go.transform.localScale = new Vector3(projectileSize.x * -1, projectileSize.z*-1, projectileSize.z);
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
        if (rotating)
        {
            rotateToTarget();
        }
        else
        {
            ThrowingSprite.transform.rotation = Quaternion.RotateTowards(ThrowingSprite.transform.rotation, Quaternion.identity, rotationSpeed *1.5f* Time.deltaTime);
        }
    }
}
