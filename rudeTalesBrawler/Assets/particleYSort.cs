using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleYSort : MonoBehaviour
{
    public GameObject rootObject;

    ParticleSystem par;
    ParticleSystemRenderer ren;
    public float offset = 0;

    private void Start()
    {
        par = GetComponent<ParticleSystem>();
        ren = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         ren.sortingOrder = (int)(500f - (offset * 10f + rootObject.transform.position.y * 10f));
    }
}
