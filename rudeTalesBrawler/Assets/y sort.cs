using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ysort : MonoBehaviour
{
    public float offset;


    private void Start()
    {
        GetComponent<Renderer>().sortingOrder = (int)(500f - (offset * 10f + transform.position.y * 10f));
    }
}
