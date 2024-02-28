using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    int currentHealth;
    public AnimtorController playerAnimScript;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

   
    public void changeHealth(int damage)// negative for damage positive for healing
    {
        currentHealth += damage;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            playerAnimScript.die();
        }
        Debug.Log("health: " + currentHealth);
    }
}
