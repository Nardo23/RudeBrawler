using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffects : MonoBehaviour
{
    public AudioSource materialSor, stepSor, attackSor, sweetenerSor, specificHurtSor;
    [SerializeField]
    AudioClip[] footstepsMaterial;
    [SerializeField]
    AudioClip[] footstepsStep;
    [SerializeField]
    AudioClip[] attack;
    [SerializeField]
    AudioClip[] hurt;
    [SerializeField]
    AudioClip[] sweetener;
    [SerializeField]
    AudioClip[] misc;
    public AudioClip[] specificHit;
    public Vector2 materialPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 stepPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 attackPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 hurtPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 sweetPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 miscPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 specificHurtPitchRange = new Vector2(.9f, 1.1f);
    public float stepDelay;

    public bool recievedHit;

    AudioClip prevM, prevF, Mclip,Fclip;
    // Start is called before the first frame update
    bool second = false;
    private void Start()
    {
        
    }

    void Step()
    {
        if (!second)
        {
            prevM = footstepsMaterial[2];
            prevF = footstepsStep[2];
            second = true;
        }
        materialSor.pitch = Random.Range(materialPitchRange.x, materialPitchRange.y);
        Mclip = footstepsMaterial[Random.Range(0, footstepsMaterial.Length)];
        if(Mclip == prevM)
        {
            Mclip = footstepsMaterial[Random.Range(0, footstepsMaterial.Length)];
            if (Mclip == prevM)
            {
                Mclip = footstepsMaterial[Random.Range(0, footstepsMaterial.Length)];
            }
        }
        materialSor.PlayOneShot(Mclip);

        stepSor.pitch = Random.Range(stepPitchRange.x, stepPitchRange.y);
        Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
        if(prevF == Fclip)
        {
             Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
            if (prevF == Fclip)
            {
                Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
            }
        }
        prevF = Fclip;
        prevM = Mclip;
        StartCoroutine(CoPlayDelayedClip(Fclip, stepSor, stepDelay));
    }

    IEnumerator CoPlayDelayedClip( AudioClip clip, AudioSource sor, float delay)
    {
        yield return new WaitForSeconds(delay);
        sor.PlayOneShot(clip);
    }

    void Attack()
    {
        attackSor.pitch = Random.Range(attackPitchRange.x, attackPitchRange.y);
        attackSor.PlayOneShot(attack[Random.Range(0, attack.Length)]);
    }

    void Sweetener()
    {
        sweetenerSor.pitch = Random.Range(sweetPitchRange.x, sweetPitchRange.y);
        sweetenerSor.PlayOneShot(sweetener[Random.Range(0, sweetener.Length)]);
    }
    public void Hurt()
    {
        if(hurt != null)
        {
            attackSor.pitch = Random.Range(hurtPitchRange.x, hurtPitchRange.y);
            attackSor.PlayOneShot(hurt[Random.Range(0, hurt.Length)]);
        }
        
        if (recievedHit)
        {
            Debug.Log("hitsound");
            specificHurtSor.pitch = Random.Range(specificHurtPitchRange.x, specificHurtPitchRange.y);
            recievedHit = false;
            specificHurtSor.PlayOneShot(specificHit[Random.Range(0, specificHit.Length)]);   
        }
    }

    void PlayMisc(int index)
    {
        attackSor.pitch = Random.Range(miscPitchRange.x, miscPitchRange.y);
        attackSor.PlayOneShot(misc[index]);
    }

    void PlayMiscRange(string clipRangeString)
    {
        string[] x = clipRangeString.Split(',');
        attackSor.pitch = Random.Range(miscPitchRange.x, miscPitchRange.y);
        bool a = int.TryParse(x[0], out int c);
        bool b = int.TryParse(x[1], out int d);
        if(a && b)
            attackSor.PlayOneShot(misc[Random.Range(int.Parse(x[0]), int.Parse(x[1]))]);
    }

}


