using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicIntro : MonoBehaviour
{
    bool playingIntro = true;
    AudioSource sor;
    public AudioClip mainLoop;
    public AudioClip intro;
    
    // Start is called before the first frame update
    private void Start()
    {
        sor = GetComponent<AudioSource>();
        //sor.loop = false;
        sor.PlayOneShot(intro);
        sor.clip = mainLoop;
        sor.loop = true;
        sor.PlayDelayed(intro.length-.24f);
        
    }

  
}
