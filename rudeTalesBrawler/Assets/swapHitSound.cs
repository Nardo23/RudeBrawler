using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapHitSound : MonoBehaviour
{
    public hitboxDamage damageScript;

    [SerializeField]
    public AudioClip[] clips1, clips2;
    public Vector2 pitchRange1 = new Vector2(.9f, 1.1f), pitchRange2 = new Vector2(.9f, 1.1f);

    void hitsound1()
    {
        damageScript.hitSounds = clips1;
        damageScript.specificHurtPitchRange = pitchRange1;
    }
    void hitsound2()
    {
        damageScript.hitSounds = clips2;
        damageScript.specificHurtPitchRange = pitchRange2;
    }
}
