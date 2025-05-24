using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFoxSpawner : MonoBehaviour
{

    public Vector2 foxAmountRange, chickenAmountRange, cornAmountRange;
    float foxAmount, chickenAmount, cornAmount;
    public GameObject fox, corn, chicken, boat;
    public Vector2 CornForceX, cornForceY, ChickenForceX, ChickenForceY;
    public motherBoss motherScript;
    float timer;
    bool chickenSpawned, foxSpawned;
    public float bonusCooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnDisable()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(timer>.25f && !chickenSpawned)
        {
            chickenSpawned = true;
            spawnChickens();
            
        }
        else if (timer > .4f)
        {
            if (!foxSpawned)
            {
                SpawnFox();
                foxSpawned = true;
            }

        }
        if (timer < 2f)
        {
            timer += Time.deltaTime;

        }
        else
        {
            motherScript.SetBonusCooldown(bonusCooldown);
            motherScript.attackOver();            
            transform.parent.gameObject.SetActive(false);
        }
            
        

    }

    private void OnEnable()
    {
        SpawnBoat();
        spawnCorn();
        timer = 0;
        foxSpawned = false;
        chickenSpawned = false;
    }
    void spawnCorn()
    {
        cornAmount = Random.Range(cornAmountRange.x, cornAmountRange.y);

        for (int i = 0; i < cornAmount; i++)
        {
            GameObject cornInst = Instantiate(corn, transform.position, Quaternion.identity);
            float x = Random.Range(CornForceX.x, CornForceX.y);
            float y = Random.Range(cornForceY.x, cornForceY.y);
            Vector2 xy = new Vector2(x, y);
            cornInst.GetComponent<Rigidbody2D>().AddForce(xy, ForceMode2D.Impulse);
        }

    }

    void spawnChickens()
    {
        chickenAmount = Random.Range(chickenAmountRange.x, chickenAmountRange.y);

        for (int i = 0; i < chickenAmount; i++)
        {
            GameObject chickenInst = Instantiate(chicken, transform.position, Quaternion.identity);
            float x = Random.Range(ChickenForceX.x, ChickenForceX.y);
            float y = Random.Range(ChickenForceY.x, ChickenForceY.y);
            Vector2 xy = new Vector2(x, y);
            chickenInst.GetComponent<Rigidbody2D>().AddForce(xy, ForceMode2D.Impulse);
        }
    }

    void SpawnFox()
    {
        foxAmount = Random.Range(foxAmountRange.x, foxAmountRange.y);

        for (int i = 0; i < foxAmount; i++)
        {
            Instantiate(fox, new Vector3(transform.position.x, transform.position.y+i, 0), Quaternion.identity);
        }
    }

    void SpawnBoat()
    {
        GameObject boatInst = Instantiate(boat, transform.position, Quaternion.identity);
        boatInst.GetComponent<Rigidbody2D>().AddForce(new Vector2(350, 0), ForceMode2D.Impulse);
    }

}
