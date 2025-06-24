using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motherBearOwlspawn : MonoBehaviour
{

    public GameObject motherBearOwl;
    public GameObject motherShadow;
    boss bossScript;
    motherBoss motherScript;
    public float bonusCooldown = 3;
    public bool spawning = false;
    // Start is called before the first frame update
    void Start()
    {
        bossScript = GetComponentInParent<boss>();
        motherScript = GetComponentInParent<motherBoss>();
        //SpawnBearOwl();
    }

    void SpawnBearOwl()
    {
        if (bossScript.currentHealth > 0)
        {
            spawning = true;
            Vector3 spawnPos = new Vector3(motherShadow.transform.position.x, motherShadow.transform.position.y - 1, 0);
            GameObject spawnedBear = Instantiate(motherBearOwl, spawnPos, Quaternion.identity);
            Transform bearForm = spawnedBear.GetComponentInChildren<enemyAnimator>().gameObject.transform;
            spawnedBear.GetComponentInChildren<enemyAnimator>().gameObject.transform.position = new Vector3(bearForm.position.x, transform.position.y, bearForm.position.z);

            if (bossScript.currentHealth < bossScript.maxHealth * .45f)
            {
                spawnedBear.transform.Find("bearOwl/BearOwl Sprite/flames").gameObject.SetActive(true);
            }
            else
                spawnedBear.transform.Find("bearOwl/BearOwl Sprite/flames").gameObject.SetActive(false); 
        }
        

    }

    public void endAttack()
    {
        spawning = false;
        motherScript.SetBonusCooldown(bonusCooldown);
        motherScript.attackOver();
    }


}
