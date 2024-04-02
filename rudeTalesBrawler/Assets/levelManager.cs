using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public int totalPlayers;
    public int livingPlayers;
    [SerializeField]
    public checkpoint[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {

        livingPlayers = totalPlayers;
        setCheckpointCounts();
    }

    public void updateLivingCount(int change) //call this from player animator when player dies or respawns
    {
        livingPlayers += change;
        setCheckpointCounts();
    }

    void setCheckpointCounts()
    {
        foreach(checkpoint check in checkpoints)
        {
            check.setPlayerCount(livingPlayers);
        }
    }

    
}
