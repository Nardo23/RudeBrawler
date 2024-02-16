using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ySortCharacter : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] rends;
    // Start is called before the first frame update
    public float offset;
    

    // Update is called once per frame
    void Update()
    {
        foreach (SpriteRenderer r in rends)
        {
            r.sortingOrder = (int)(500f - (offset * 10f + transform.position.y * 10f));
        }
    }
}
