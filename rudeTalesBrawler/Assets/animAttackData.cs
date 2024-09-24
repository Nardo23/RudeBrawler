using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animAttackData : MonoBehaviour
{
    // Start is called before the first frame update
    public hitboxDamage dmgScript;
   
    public void setDmg(int dmg)
    {
        dmgScript.damage = dmg;
    }
    public void setYRange(float yRange)
    {
        dmgScript.yRange = yRange;
    }
    public void setKnockback(float knockBack)
    {
        dmgScript.knockbackForce = knockBack;
    }
    public void setAttackdata(int damage, float yRange, float knockBack, float hitStopDuration, int attackStrenght, int dmgType)
    {
        dmgScript.damage = damage;
        dmgScript.yRange = yRange;
        dmgScript.knockbackForce = knockBack;
        dmgScript.hitStopDuration = hitStopDuration;
        dmgScript.attackStrength = attackStrenght;
        dmgScript.damageType = dmgType;

    }

    
}
