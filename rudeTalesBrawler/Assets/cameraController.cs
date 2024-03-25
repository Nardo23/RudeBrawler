using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject currentTarget;
    public GameObject characterTarget;
    public float speed = 2.0f;
    public float deadZoneX, deadZoneY = 3;
    public bool followMode = true;
    public bool followY = false;
    float distanceX, distanceY;
    Vector3 prevPos;


    private void Start()
    {
        prevPos = transform.position;
        currentTarget = characterTarget;
    }

    public void LockedCamera(GameObject target, bool enableYfollow)
    {
        //followMode = false;
        followY = enableYfollow;
        currentTarget = target;
    } 
    public void CamFollowCharacter(bool enableYfollow)
    {
        followMode = true;
        currentTarget = characterTarget;
        followY = enableYfollow;
    }

    void Update()
    {
        Vector3 position = this.transform.position;
        if (followMode)
        {
            distanceX = Mathf.Abs(this.transform.position.x - currentTarget.transform.position.x);
            distanceY = Mathf.Abs(this.transform.position.y - currentTarget.transform.position.y);

            if (distanceX > deadZoneX || prevPos != transform.position)
            {
                Debug.Log("lerping");
                position.x = Mathf.Lerp(this.transform.position.x, currentTarget.transform.position.x, (speed + distanceX * .7f) * Time.deltaTime);
            }

            if (followY)
            {
                if (distanceY > deadZoneY || prevPos != transform.position)
                {
                    position.y = Mathf.Lerp(this.transform.position.y, currentTarget.transform.position.y, (speed + distanceY * .7f) * Time.deltaTime);
                }

            }
            this.transform.position = position;

        }
        else
        {
            position.x = Mathf.Lerp(this.transform.position.x, currentTarget.transform.position.x, (speed*.25f ) * Time.deltaTime);
            if (followY)
            {
                position.y = Mathf.Lerp(this.transform.position.y, currentTarget.transform.position.y, (speed*.25f) * Time.deltaTime);
            }

            prevPos = transform.position;
        };
    }
}
