using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    int currentHealth;
    public AnimtorController playerAnimScript;

    public Image healthbar;
    float BarMaxWidth;
    float barSpeed = 1;
    float curDamage;
    public Color highHealth, mediumHealth, lowHealth;
    public Sprite healthySpr, medSpr, lowSpr, deadSpr;
    public Image icon;
    // Start is called before the first frame update
    void Start()
    {
        BarMaxWidth = healthbar.rectTransform.rect.width;
        currentHealth = maxHealth;
        Debug.Log("barMAx"+BarMaxWidth);
    }

   // (current/maxHealth)*maxWidth = x
    public void changeHealth(int damage)// negative for damage positive for healing
    {
        curDamage = Mathf.Abs(damage);
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

    private void Update()
    {
        if(currentHealth > 0)
        {
            healthbar.rectTransform.sizeDelta = Vector2.Lerp(healthbar.rectTransform.sizeDelta, new Vector2(((float)currentHealth / (float)maxHealth) * BarMaxWidth, healthbar.rectTransform.rect.height), Time.deltaTime*(barSpeed+(float)curDamage/(float)maxHealth)*4);

            //healthbar.rectTransform.sizeDelta = new Vector2(((float)currentHealth / (float)maxHealth) * BarMaxWidth, healthbar.rectTransform.rect.height);
        }
        else
        {
            icon.sprite = deadSpr;
            healthbar.rectTransform.sizeDelta = new Vector2(0, healthbar.rectTransform.rect.height);
        }

        if ((float)currentHealth / (float)maxHealth >= .75f)
        {
            icon.sprite = healthySpr;
            healthbar.color = Color.Lerp(healthbar.color, highHealth, Time.deltaTime);
        }
        else if((float)currentHealth / (float)maxHealth >= .35f)
        {
            icon.sprite = medSpr;
            healthbar.color = Color.Lerp(healthbar.color, mediumHealth, Time.deltaTime);
        }
        else if (currentHealth <=0)
        {
            icon.sprite = deadSpr;
        }
        else
        {
            icon.sprite = lowSpr;
            healthbar.color = Color.Lerp(healthbar.color, lowHealth, Time.deltaTime);
        }

    }

}
