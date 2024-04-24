using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public int totalPlayers;
    public int livingPlayersCount;
    [SerializeField]
    public checkpoint[] checkpoints;
    GameObject[] allPlayers;
    public GameObject[] livingPlayers;

    // Start is called before the first frame update
    void Start()
    {

        livingPlayersCount = totalPlayers;
        setCheckpointCounts();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        updateLivingPlayers();
    }

    public void updateLivingCount(int change) //call this from player animator when player dies or respawns
    {
        livingPlayersCount += change;
        setCheckpointCounts();
        updateLivingPlayers();
    }

    void updateLivingPlayers()
    {
        livingPlayers = new GameObject[livingPlayersCount];
        int i = 0;
        foreach (GameObject obj in allPlayers)
        {
            if (obj.GetComponentInChildren<AnimtorController>().alive && i < livingPlayersCount)
            {
                livingPlayers[i] = obj;
                i++;
            }
        }
        
    }

    void setCheckpointCounts()
    {
        foreach(checkpoint check in checkpoints)
        {
            check.setPlayerCount(livingPlayersCount);
        }
    }

    
}
