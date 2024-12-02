using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffects : MonoBehaviour
{
    public AudioSource materialSor, stepSor, attackSor;
    [SerializeField]
    AudioClip[] footstepsMaterial;
    [SerializeField]
    AudioClip[] footstepsStep;
    [SerializeField]
    AudioClip[] attack;
    [SerializeField]
    AudioClip[] hurt;
    public Vector2 materialPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 stepPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 attackPitchRange = new Vector2(.9f, 1.1f);
    public Vector2 hurtPitchRange = new Vector2(.9f, 1.1f);
    public float stepDelay;

    AudioClip prevM, Mclip,Fclip;
    // Start is called before the first frame update

    private void Start()
    {
        prevM = footstepsMaterial[2];
    }

    void Step()
    {
        materialSor.pitch = Random.Range(materialPitchRange.x, materialPitchRange.y);
        Mclip = footstepsMaterial[Random.Range(0, footstepsMaterial.Length)];
        materialSor.PlayOneShot(Mclip);

        stepSor.pitch = Random.Range(stepPitchRange.x, stepPitchRange.y);
        Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
        if(prevM == Fclip)
        {
             Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
            if (prevM == Fclip)
            {
                Fclip = footstepsStep[Random.Range(0, footstepsStep.Length)];
            }
        }
        prevM = Fclip;
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

    void Hurt()
    {
        attackSor.pitch = Random.Range(hurtPitchRange.x, hurtPitchRange.y);
        attackSor.PlayOneShot(hurt[Random.Range(0, hurt.Length)]);
    }
}
