using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public bool simpleEnemy = false;
    public simpleEnemyAnimator simpleEnemyAnimScript;
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
            if (simpleEnemy)
            {
                Debug.Log("simpDeath");
                simpleEnemyAnimScript.die();
            }
            else
            {
                enemyAnimScript.die();
            }
            
        }
        Debug.Log("health: " + currentHealth);
    }

}
