using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public enemyAnimator enemyAnimScript;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void changeHealth(int damage)// negative for damage positive for healing
    {
        currentHealth += damage;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if(currentHealth <= 0)
        {
            enemyAnimScript.die();
        }
        Debug.Log("health: " + currentHealth);
    }

}
