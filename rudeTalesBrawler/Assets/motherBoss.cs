using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motherBoss : MonoBehaviour
{
    public int numOfAttacks;
    int currentAttack;
    public GameObject tentacleSlamObj, tentacleBeamObj, tentacleRiddle;
    boss bossScript;
    bool startVulnerable;
    Animator anim;
    public Animator orrificeAnim;
    // Start is called before the first frame update
    void Start()
    {
        bossScript = GetComponent<boss>();
        anim = GetComponent<Animator>();
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
                    if (tentacleSlamObj.activeSelf)
                    {
                        
                        break;
                    }
                    tentacleSlamObj.SetActive(true);
                    bossScript.attackReady = false;
                    break;
                case 2:
                    if (tentacleBeamObj.activeSelf)
                    {
                        
                        break;
                    }
                    tentacleBeamObj.SetActive(true);
                    bossScript.attackReady = false;
                    break;
                case 3:
                    if (tentacleRiddle.activeSelf)
                    {
                        
                        break;
                    }
                    tentacleRiddle.SetActive(true);
                    bossScript.attackReady = false;
                    break;
                case 4:
                    if (!orrificeAnim.GetComponent<motherBearOwlspawn>().spawning)
                    {
                        //Debug.Log("number of bears: "+GameObject.FindGameObjectsWithTag("MotherBearOwl").Length);
                        if (GameObject.FindGameObjectsWithTag("MotherBearOwl").Length < 2)
                        {
                            orrificeAnim.SetTrigger("Spawn");
                            bossScript.attackReady = false;
                            break;
                        }                    
                    }                        
                    break;
            }
        }
        if (bossScript.vulnerable)
        {
            if (!startVulnerable)
            {
                startVulnerable = true;
                anim.SetTrigger("Slam");
            }
        }
        else
        {
            if (startVulnerable)
            {
                startVulnerable = false;
                anim.SetTrigger("Rise");
                spawnBearOwl();
            }

        }


    }
    
    public void spawnBearOwl()
    {
        if (GameObject.FindGameObjectsWithTag("MotherBearOwl").Length < 2)
            orrificeAnim.SetTrigger("Spawn");
    }

    public void attackOver()//individual attacks call this when they end
    {
        bossScript.endCurrentAttack();
    }
    public void SetBonusCooldown(float bonus)
    {
        bossScript.bonusCooldown = bonus;
    }

}
