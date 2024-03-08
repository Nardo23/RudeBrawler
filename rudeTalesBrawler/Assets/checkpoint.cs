using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] GameObject[] enemyWaves;
    public GameObject entranceWall, exitWall;
    public int playerCount;
    GameObject[] playerStore;
    int currentPlayersIn = 0;
    bool canStart = false;
    bool ended = false;
    public int currentWave = 0;
    
    public cameraController camScript;
    // Start is called before the first frame update
    void Start()
    {
        playerStore = new GameObject[playerCount];
    }

    // Update is called once per frame
    void Update()
    {

        

        if(canStart && !ended)
        {
            camScript.LockedCamera(this.gameObject, false);
            if (enemyWaves[currentWave].activeSelf == false)
            {
                enemyWaves[currentWave].SetActive(true);
            }
                
            if (enemyWaves[currentWave].transform.childCount == 0)
            {
                if(currentWave < enemyWaves.Length-1)
                {
                    currentWave++;
                }
                else
                {
                    ended = true;
                    camScript.CamFollowCharacter(false);
                    exitWall.SetActive(false);
                }
            }
        }
        if (ended)
        {
            Debug.Log("ended");
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bool alreadyEntered = false;
            foreach (GameObject obj in playerStore)
            {
                if(obj == collision.gameObject)
                {
                    alreadyEntered = true;
                }
                else
                {
                    alreadyEntered = false;
                }
            }
            if (!alreadyEntered)
            {
                playerStore[currentPlayersIn] = collision.gameObject;
                currentPlayersIn++;
            }

            if(currentPlayersIn == playerCount)
            {
                canStart = true;
                entranceWall.SetActive(true);
                Debug.Log("all in");
            }

        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            int counter = 0;
            foreach(GameObject obj in playerStore)
            {
                
                if(obj == collision.gameObject)
                {
                    playerStore[counter] = null; 
                    currentPlayersIn--;
                }
                counter++;
            }
        }
    }

}
