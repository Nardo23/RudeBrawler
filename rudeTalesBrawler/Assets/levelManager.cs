using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class levelManager : MonoBehaviour
{
    public int totalPlayers;
    public int livingPlayersCount;
    [SerializeField]
    public checkpoint[] checkpoints;
    public GameObject[] allPlayers, deletetestPlayers;
    public GameObject[] livingPlayers;
    public bool albeeIn, stirfryIn, debonesbyIn;
    public bool testMode;
    public Transform P1Spawn, P2Spawn;
    public GameObject Albee, Stirfry, Debonesby;
    public Image heathBarMed, healthBarMedP2, healthBarMedP3, healthBarSmallP1, healthBarSmallP2, healthBarSmallP3;
    public Image IconP1, IconP2, IconP3;
    public bool gameoverMessage;
    public GameObject gameoverText;
    public bool gameover = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (MainManager.Instance != null)
        {            
            totalPlayers = 0;
            deletetestPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in deletetestPlayers)
            {
                Destroy(p);
            }
            if (ValidId(MainManager.Instance.P1Char)) //Player 1
            {
                GameObject char1 = SpawnCharacter(MainManager.Instance.P1Char);
                char1.transform.position = P1Spawn.position;
                setInput(char1.GetComponent<PlayerInput>(), 1);
                totalPlayers++;
                char1.name = "Player1";
                char1.GetComponent<PlayerHealth>().healthBarMed = heathBarMed;
                char1.GetComponent<PlayerHealth>().healthBarSmall = healthBarSmallP1;
                char1.GetComponent<PlayerHealth>().icon = IconP1;
                char1.SetActive(true);

            }
            else
            {
                healthBarSmallP1.transform.parent.gameObject.SetActive(false);
            }
            if (ValidId(MainManager.Instance.P2Char)) //Player 2
            {
                GameObject char2 = SpawnCharacter(MainManager.Instance.P2Char);
                char2.transform.position = P2Spawn.position;
                setInput(char2.GetComponent<PlayerInput>(), 2);
                totalPlayers++;
                char2.name = "Player2";
                char2.GetComponent<PlayerHealth>().healthBarMed = healthBarMedP2;
                char2.GetComponent<PlayerHealth>().healthBarSmall = healthBarSmallP2;
                char2.GetComponent<PlayerHealth>().icon = IconP2;
                char2.SetActive(true);
            }
            else
            {
                healthBarSmallP2.transform.parent.gameObject.SetActive(false);
            }
            healthBarMedP3.transform.parent.gameObject.SetActive(false);
        }
        livingPlayersCount = totalPlayers;
        setCheckpointCounts();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");

        updateLivingPlayers();
    }

    public bool ValidId(string Id)
    {
        if (Id == "A" || Id == "S" || Id == "D")
            return true;        
        else
            return false;
    }
    public GameObject SpawnCharacter(string Id)
    {
        if(Id == "A")
        {
            albeeIn = true;
            return (Instantiate(Albee, Vector3.zero, Quaternion.identity));
        }                
        else if(Id == "S")
        {
            stirfryIn = true;
            return (Instantiate(Stirfry, Vector3.zero, Quaternion.identity));
        }          
        else if (Id == "D")
        {
            debonesbyIn = true;
            return (Instantiate(Debonesby, Vector3.zero, Quaternion.identity));
        }          
        else
        {
            Debug.LogError("Spawning character with Bad Id, bozo");
            return (Instantiate(Albee, Vector3.zero, Quaternion.identity));
        }
          
    }

    public void respawnDeadPlayers()
    {
        foreach (GameObject p in allPlayers)
        {
            p.GetComponentInChildren<AnimtorController>().Respawn();
        }
    }


    void setInput(PlayerInput inputScript, int playerNumber)
    {
        inputScript.HorizontalControls = "Horizontal";
        inputScript.VerticalControls = "Vertical";
        inputScript.JumpButton = "Jump";
        inputScript.AttackButton = "Attack";
        inputScript.SpecialButton = "Special";
        if (playerNumber > 1)
        {
            Debug.Log("inputP2+");
            inputScript.HorizontalControls = inputScript.HorizontalControls + "P" + playerNumber;
            inputScript.VerticalControls = inputScript.VerticalControls + "P" + playerNumber;
            inputScript.JumpButton = inputScript.JumpButton + "P" + playerNumber;
            inputScript.AttackButton = inputScript.AttackButton + "P" + playerNumber;
            inputScript.SpecialButton = inputScript.SpecialButton + "P" + playerNumber;
        }
    }

    public void updateLivingCount(int change) //call this from player animator when player dies or respawns
    {
        livingPlayersCount += change;
        setCheckpointCounts();
        updateLivingPlayers();
        if(gameoverMessage && livingPlayersCount == 0)
        {
            gameoverText.SetActive(true);
            gameover = true;
        }
    }

    void updateLivingPlayers()
    {
        livingPlayers = new GameObject[livingPlayersCount];
        int i = 0;
        foreach (GameObject obj in allPlayers)
        {
            if (!testMode)
            {
                if (obj.GetComponentInChildren<AnimtorController>().alive && i < livingPlayersCount)
                {
                    livingPlayers[i] = obj;
                    Debug.Log("character: "+obj);
                    i++;
                }
            }          
            else if (testMode)
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
