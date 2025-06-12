using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackId : MonoBehaviour
{
    public hitboxDamage dmgScript;
    

    void increaseId()
    {
        dmgScript.attackID++;
        if (dmgScript.attackID > 9)
            dmgScript.attackID = 0;
    }
}
