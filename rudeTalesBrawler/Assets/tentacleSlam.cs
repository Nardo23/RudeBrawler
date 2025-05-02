using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleSlam : MonoBehaviour
{
    Animator anim;
    bool started = false;
    bool pickedTarget = false;
    public float chaseTime = 3, chaseSpeed;
    float timer = 0;
    levelManager levelManagerScript;
    Transform target;
    public motherBoss motherScript;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
    }
    
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
        started = true;
        timer = 0;
        pickedTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            
            if(timer < chaseTime)
            {
                timer += Time.deltaTime;
                targetPlayer();
            }
            else
            {
                anim.Play("tentacleSlam");
            }
        }      
    }
    
    void targetPlayer()
    {
        if (!pickedTarget)
        {
            target = levelManagerScript.livingPlayers[Random.Range(0, levelManagerScript.livingPlayers.Length)].transform;
            pickedTarget = true;
        }
        float x = Mathf.Lerp(transform.position.x, target.transform.position.x, chaseSpeed * Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        

    }

    
    //These two functions are called from the animation
    void attackOver()
    {
        //tell boss script the attack is done
        motherScript.attackOver();
    }

    void endSlam()
    {
        this.gameObject.SetActive(false);
        started = false;
    }

    

}
