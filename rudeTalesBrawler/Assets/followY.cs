using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followY : MonoBehaviour
{
    public Transform target;
    public float yOffset;
    // Start is called before the first frame update
  
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, target.transform.position.y+yOffset, transform.position.z);
    }
}
