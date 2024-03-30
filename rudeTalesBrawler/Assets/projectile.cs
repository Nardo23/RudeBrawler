using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public Transform target;
    public Vector3 targetPos;
    public float xOffset = 0;
    public GameObject shadow;
    public GameObject projectileObj;
    public float speed = 8;
    float projectileTime;
    float shadowSpeed;

    bool fired = false;

    public void fire( Transform targ, Transform ProjectileRotation, Transform shadowLocation)
    {
        projectileObj.transform.rotation = ProjectileRotation.transform.rotation;
        shadow.transform.position = shadowLocation.position;
        target = targ;
        fired = true;
        if (target != null)
        {
            targetPos = new Vector3(target.transform.position.x + xOffset, target.transform.position.y, target.transform.position.z);
            projectileTime = Vector3.Distance(projectileObj.transform.position, targetPos) / speed;
            shadowSpeed = Vector3.Distance(shadow.transform.position, targetPos) / (projectileTime);
        }
    }
    
    void Update()
    {
        if(fired)
        {
            projectileObj.transform.position = Vector3.MoveTowards(projectileObj.transform.position, targetPos, speed * Time.deltaTime);
            //shadow.transform.position = new Vector3(projectileObj.transform.position.x, shadow.transform.position.y, shadow.transform.position.z);
            //float shadowY = 
            // distance/speed = time
            // ptime = distance/speed

           
            shadow.transform.position = Vector3.MoveTowards(shadow.transform.position, targetPos, shadowSpeed * Time.deltaTime);
        }
    }
}
