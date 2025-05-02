using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motherBoss : MonoBehaviour
{
    public int numOfAttacks;
    int currentAttack;
    public GameObject tentacleSlamObj, tentacleBeamObj;
    boss bossScript;
    // Start is called before the first frame update
    void Start()
    {
        bossScript = GetComponent<boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossScript.attackReady)
        {
            currentAttack = bossScript.pickAttack(numOfAttacks);
            switch (currentAttack)
            {
                case 1: 
                    tentacleSlamObj.SetActive(true);
                    bossScript.attackReady = false;
                    break;
                case 2:
                    tentacleBeamObj.SetActive(true);
                    bossScript.attackReady = false;
                    break;
            }
        }


    }
    
    public void attackOver()//individual attacks call this when they end
    {
        bossScript.endCurrentAttack();
    }


}
