using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    levelManager levelManagerScript;
    public Sprite albeeFood, stirfryFood, bonesbyFood;
    public Sprite[] foodSprites;
    public GameObject healParticles;
    private void Start()
    {
       levelManagerScript = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<levelManager>();
       foodSprites = new Sprite [levelManagerScript.allPlayers.Length];
        int count = 0;

        if (levelManagerScript.albeeIn)
        {
            foodSprites[count] = albeeFood;
            count++;
        }
        if (levelManagerScript.stirfryIn)
        {
            foodSprites[count] = stirfryFood;
            count++;
        }
        if (levelManagerScript.debonesbyIn)
        {
            foodSprites[count] = bonesbyFood;
            count++;
        }
        if(count>0)
            randomFood();
    }

    void randomFood()
    {
        int r = Random.Range(0, foodSprites.Length);
        if (foodSprites[r] == null)
            randomFood();
        GetComponentInChildren<SpriteRenderer>().sprite = foodSprites[r];
        
        
    }

    public int health = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerHealth p = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if (p != null)
            {
                GameObject ob = Instantiate(healParticles, collision.transform.position, Quaternion.identity);
                ob.transform.parent = collision.transform;
                p.changeHealth(health);
                Destroy(this.gameObject);
            }
        }
    }
}
