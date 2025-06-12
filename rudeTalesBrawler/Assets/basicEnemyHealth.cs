using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemyHealth : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public GameObject dropable;
    public Rigidbody2D rb;
    public float knockbackMultiplyer =1;
    public ColoredFlash flashScript;
    public float bonusWidth;
    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void changeHealth(int damage)// negative for damage positive for healing
    {
        currentHealth += damage;

        if (damage < 0)
        {
            flashScript.Flash();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            
            if (dropable != null)
            {
                Instantiate(dropable, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);

        }
        
    }

    public void knockback(float force, float xPos)
    {
        if (xPos<transform.position.x) //knockback right
        {
            rb.AddForce((Vector2.right * force * knockbackMultiplyer), ForceMode2D.Impulse);
        }
        else //knockback left
        {
            rb.AddForce((Vector2.left * force * knockbackMultiplyer), ForceMode2D.Impulse);
        }

        //Debug.Log("KNOCKED "+ force * 1 * knockbackMultiplyer);
    }

}
