using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    enum bossState {Idle, Attack, Vulnerable}
    

    bossState currentState = bossState.Attack;

    int attackCounter = 0;
    public int numberofattacks;
    public bool attackReady = true;
    bool attackFinished = true;
    public float cooldown =1;
    public float bonusCooldown = 0;
    float timer;
    int vulnerableDamage;
    public int maxVulnerableDamage;

    public int maxHealth;
    int currentHealth;
    public float bonusSize;
    public bool vulnerable = false;
    public float vulnerableTime=5;
    
    public ColoredFlash coloredFlashScript;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentState == bossState.Attack)
        {
            attackTick();
        }
        if(currentState == bossState.Vulnerable)
        {
            vulnerable = true;
            if (timer < vulnerableTime)
            {
                timer += Time.deltaTime;
            }
            else 
            {
                currentState = bossState.Attack;
                attackFinished = true;
                timer = 0;
                attackCounter = 0;
            }
        }
        else
        {
            vulnerable = false;
        }
        
    }


    void attackTick()
    {
        if (attackFinished)
        {
            if (timer < cooldown+bonusCooldown)
            {
                timer += Time.deltaTime;
            }
            else if (attackCounter< numberofattacks)
            {
                attackCounter += 1;
                bonusCooldown = 0;
                attackReady = true;
                Debug.Log("bossAttackReady");
                attackFinished = false;
            }
            else
            {
                currentState = bossState.Vulnerable;
                timer = 0;
            }
                
        }
        

        //pick attack from list
        //wait for attack to finish
        //wait for cooldowntimer,
        // increase attackCounter
        // if attack counter reaches certain number 1) play vulnerable attack 2) switch to vulnerable state 3) set attackCounter back to 0


    }
    public int pickAttack(int max)//boss sends its number of attacks
    {
        return Random.Range(1, max+1); //boss recieves a random attack
    }

    public void endCurrentAttack()
    {
        //called by boss when they finish an attack
        timer = 0;
        attackFinished = true;
        attackCounter ++;
    }

    void vulnerableTick()
    {
        //start timer
        // if boss takes more than X damage or timer elapses return to attack state
        // may want to go to next boss phase here


    }

    public void changeHealth(int damage)// negative for damage positive for healing
    {
        if(coloredFlashScript!=null && damage < 0)
        {
            coloredFlashScript.Flash();
        }
        currentHealth += damage;
        if(currentState == bossState.Vulnerable)
        {
            vulnerableDamage += damage;
            if(vulnerableDamage > maxVulnerableDamage)
            {
                currentState = bossState.Attack;
                attackFinished = true;
                timer = 0;
                attackCounter = 0;
            }
        }
        else
        {
            vulnerableDamage = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            //die
            currentState = bossState.Idle;
            Debug.Log("Boss Dead!");
        }
    }

}
