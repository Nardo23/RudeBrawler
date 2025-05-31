using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentacleBeam : MonoBehaviour
{
    Animator anim;
    levelManager levelManagerScript;
    bool pickedTarget = false;
    public motherBoss motherScript;
    public float leftX, rightX;
    public Camera cam;
    Transform target;
    public float bonusCooldown;

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
        target = levelManagerScript.livingPlayers[Random.Range(0, levelManagerScript.livingPlayers.Length)].transform;
        
        transform.rotation = Quaternion.identity;
        
        if(target.transform.position.x > cam.transform.position.x)
        {
            transform.position = new Vector3(leftX, target.transform.position.y, transform.position.z);
            transform.Rotate(0, 180, 0);
        }
        else
        {
            transform.position = new Vector3(rightX, target.transform.position.y, transform.position.z);
        }


        anim.Play("tentacleBeam");
    }

    void attackOver()
    {
        //tell boss script the attack is done
        motherScript.attackOver();
        motherScript.SetBonusCooldown(bonusCooldown);
    }

    void endBeam()
    {
        this.gameObject.SetActive(false);
        
    }
}
