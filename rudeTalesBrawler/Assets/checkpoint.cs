using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] GameObject[] enemyWaves;
    public GameObject entranceWall, exitWall, extraEnable;
    public int playerCount;
    GameObject[] playerStore;
    int currentPlayersIn = 0;
    bool canStart = false;
    bool ended = false;
    public int currentWave = 0;

    public bool freeCam;
    public bool autoStart;
    public cameraController camScript;
    
    // Start is called before the first frame update
    void Start()
    {
        if (autoStart)
            canStart = true;
    }

    public void setPlayerCount(int count)
    {
        playerStore = new GameObject[count];
        playerCount = count;
        Debug.Log("checkpointCountUpdate "+playerStore.Length);
    }

    // Update is called once per frame
    void Update()
    {

        

        if(canStart && !ended)
        {
            
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
                else                 //All enemies defeated
                {
                    Debug.Log("checkpoint cleared!");
                    ended = true;
                    if(!freeCam)
                        camScript.CamFollowCharacter(false);
                    if(exitWall!=null)
                        exitWall.SetActive(false);
                    if (extraEnable != null)
                    {
                        extraEnable.SetActive(true);
                    }

                    GameObject managerObj = GameObject.FindGameObjectWithTag("LevelManager");
                    managerObj.GetComponent<levelManager>().respawnDeadPlayers();


                }
            }
        }
        if (ended)
        {
            //Debug.Log("ended");
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

            if(currentPlayersIn == playerCount && !ended)
            {
                canStart = true;
                if (entranceWall!=null)
                    entranceWall.SetActive(true);
                camScript.LockedCamera(this.gameObject, false);
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
