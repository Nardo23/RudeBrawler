using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterSelect : MonoBehaviour
{
    [SerializeField]
    public Transform[] iconPositions;
    public Transform[] LevelPointers;
    public GameObject levelCursor;
    int levelIndex;
    public GameObject P1Cursor, P2Cursor , P1Check, P2Check;
    public GameObject P3Cursor, P4Cursor, P3Check, P4Check;
    int P1count = 0, P2count = 0, P3count = 0, P4count = 0;
    float timer1 = 99, timer2 = 99, timer3 = 99, timer4 = 99,timera =99, timerb=99, timerc =99, timerd=99;
    float cursorSpeed = .2f;
    bool p1Selected, p2Selected, p3Selected, p4Selected;
    public GameObject startButton;
    [SerializeField] GameObject[] P1Icons, P2Icons, P3Icons, P4Icons;
    Animator anim1, anim2, anim3, anim4;
    public bool hovering = false;
    int levelNumber;
    public GameObject characterSelectArt, LevelSelect, levelSelectArt;
    bool inCharacterselect = true;
    // Start is called before the first frame update
    void Start()
    {
        P1Cursor.transform.position = new Vector3(iconPositions[0].position.x - .65f, iconPositions[0].position.y, 0);
        P2Cursor.transform.position = new Vector3(iconPositions[0].position.x + .65f, iconPositions[0].position.y, 0);
        P3Cursor.transform.position = new Vector3(iconPositions[0].position.x - .65f, iconPositions[0].position.y, 0-3.5f);
        P4Cursor.transform.position = new Vector3(iconPositions[0].position.x + .65f, iconPositions[0].position.y, 0-3.5f);
        anim1 = P1Cursor.GetComponent<Animator>();
        anim2 = P2Cursor.GetComponent<Animator>();
        anim3 = P3Cursor.GetComponent<Animator>();
        anim4 = P4Cursor.GetComponent<Animator>();
        anim2.Play("cursorBounce", -1, .2f);
        anim3.Play("cursorBounce", -1, .2f);
        anim4.Play("cursorBounce", -1, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inCharacterselect)
        {
            if (!p1Selected)
            {
                if (timer1 > cursorSpeed)
                {
                    if (Input.GetAxis("Horizontal") > .2f) //P1
                    {
                        P1count++;
                        if (P1count > iconPositions.Length - 1)
                            P1count = 0;
                        timer1 = 0;
                    }
                    else if (Input.GetAxis("Horizontal") < -.2f)
                    {
                        P1count--;
                        if (P1count < 0)
                            P1count = iconPositions.Length - 1;
                        timer1 = 0;
                    }

                }
                else
                {
                    timer1 += Time.deltaTime;
                    if (Input.GetAxis("Horizontal") == 0)
                    {
                        timer1 = 99;
                    }
                }
            }

            if (!p2Selected)
            {
                if (timer2 > cursorSpeed)
                {
                    if (Input.GetAxis("HorizontalP2") > .2f) //P2
                    {
                        P2count++;
                        if (P2count > iconPositions.Length - 1)
                            P2count = 0;
                        timer2 = 0;
                    }
                    else if (Input.GetAxis("HorizontalP2") < -.2f)
                    {
                        P2count--;
                        if (P2count < 0)
                            P2count = iconPositions.Length - 1;
                        timer2 = 0;
                    }

                }
                else
                {

                    timer2 += Time.deltaTime;
                    if (Input.GetAxis("HorizontalP2") == 0)
                    {
                        timer2 = 99;
                    }
                }
            }
            if (!p3Selected)
            {
                if (timer3 > cursorSpeed)
                {
                    if (Input.GetAxis("HorizontalP3") > .2f) //P3
                    {
                        P3count++;
                        if (P3count > iconPositions.Length - 1)
                            P3count = 0;
                        timer3 = 0;
                    }
                    else if (Input.GetAxis("HorizontalP3") < -.2f)
                    {
                        P3count--;
                        if (P3count < 0)
                            P3count = iconPositions.Length - 1;
                        timer3 = 0;
                    }

                }
                else
                {

                    timer3 += Time.deltaTime;
                    if (Input.GetAxis("HorizontalP3") == 0)
                    {
                        timer3 = 99;
                    }
                }
            }
            if (!p4Selected)
            {
                if (timer4 > cursorSpeed)
                {
                    if (Input.GetAxis("HorizontalP4") > .2f) //P4
                    {
                        P4count++;
                        if (P4count > iconPositions.Length - 1)
                            P4count = 0;
                        timer4 = 0;
                    }
                    else if (Input.GetAxis("HorizontalP4") < -.2f)
                    {
                        P4count--;
                        if (P4count < 0)
                            P4count = iconPositions.Length - 1;
                        timer4 = 0;
                    }

                }
                else
                {

                    timer4 += Time.deltaTime;
                    if (Input.GetAxis("HorizontalP4") == 0)
                    {
                        timer4 = 99;
                    }
                }
            }
        }
        else
        {
            if (timera > cursorSpeed)// PLAYER1
            {
                if (Input.GetAxis("Horizontal") > .2f)
                {
                    levelIndex++;
                    if (levelIndex > LevelPointers.Length - 1)
                        levelIndex = 0;
                    timera = 0;

                }
                else if (Input.GetAxis("Horizontal") < -.2f)
                {
                    levelIndex--;
                    if (levelIndex < 0)
                        levelIndex = LevelPointers.Length - 1;
                    timera = 0;
                }
            }            
            else
            {
               timera += Time.deltaTime;
               if (Input.GetAxis("HorizontalP2") == 0)
               {
                   timerb = 99;
               }
                if (Input.GetAxis("HorizontalP3") == 0)
                {
                    timerc = 99;
                }
                if (Input.GetAxis("HorizontalP4") == 0)
                {
                    timerd = 99;
                }

            }
            if (timerb > cursorSpeed)// PLAYER2
            {
                if (Input.GetAxis("HorizontalP2") > .2f)
                {
                    levelIndex++;
                    if (levelIndex > LevelPointers.Length - 1)
                        levelIndex = 0;
                    timerb = 0;
                }
                else if (Input.GetAxis("HorizontalP2") < -.2f)
                {
                    levelIndex--;
                    if (levelIndex < 0)
                        levelIndex = LevelPointers.Length - 1;
                    timerb = 0;
                }
            }
            else
            {
                timerb += Time.deltaTime;
                if(Input.GetAxis("HorizontalP2") == 0)
                {
                    timerb = 99;
                }
                if (Input.GetAxis("HorizontalP3") == 0)
                {
                    timerc = 99;
                }
                if (Input.GetAxis("HorizontalP4") == 0)
                {
                    timerd = 99;
                }
            }
            if(timerc > cursorSpeed)// PLAYER3
            {
                if (Input.GetAxis("HorizontalP3") > .2f)
                {
                    levelIndex++;
                    if (levelIndex > LevelPointers.Length - 1)
                        levelIndex = 0;
                    timerc = 0;
                }
                else if (Input.GetAxis("HorizontalP3") < -.2f)
                {
                    levelIndex--;
                    if (levelIndex < 0)
                        levelIndex = LevelPointers.Length - 1;
                    timerc = 0;
                }
            }
            else
            {
                timerc += Time.deltaTime;
                if (Input.GetAxis("HorizontalP2") == 0)
                {
                    timerb = 99;
                }
                if (Input.GetAxis("HorizontalP3") == 0)
                {
                    timerc = 99;
                }
                if (Input.GetAxis("HorizontalP4") == 0)
                {
                    timerd = 99;
                }
            }
            if(timerd > cursorSpeed)// PLAYER4
            {
                if (Input.GetAxis("HorizontalP4") > .2f)
                {
                    levelIndex++;
                    if (levelIndex > LevelPointers.Length - 1)
                        levelIndex = 0;
                    timerd = 0;
                }
                else if (Input.GetAxis("HorizontalP4") < -.2f)
                {
                    levelIndex--;
                    if (levelIndex < 0)
                        levelIndex = LevelPointers.Length - 1;
                    timerd = 0;
                }
            }
            else
            {
                timerd += Time.deltaTime;
                if (Input.GetAxis("HorizontalP2") == 0)
                {
                    timerb = 99;
                }
                if (Input.GetAxis("HorizontalP3") == 0)
                {
                    timerc = 99;
                }
                if (Input.GetAxis("HorizontalP4") == 0)
                {
                    timerd = 99;
                }
            }
        }
        levelCursor.transform.position = LevelPointers[levelIndex].position;
        levelNumber = levelIndex + 1;

        P1Cursor.transform.position = new Vector3(iconPositions[P1count].position.x - .65f, iconPositions[P1count].position.y, 0);
        P2Cursor.transform.position = new Vector3(iconPositions[P2count].position.x + .65f, iconPositions[P2count].position.y, 0);
        P3Cursor.transform.position = new Vector3(iconPositions[P3count].position.x - .65f, iconPositions[P3count].position.y - 3.5f, 0);
        P4Cursor.transform.position = new Vector3(iconPositions[P4count].position.x + .65f, iconPositions[P4count].position.y - 3.5f, 0);
        if (Input.GetButtonDown("Enter")|| Input.GetButtonDown("JumpP2") || Input.GetButtonDown("JumpP3")|| Input.GetButtonDown("JumpP4"))
        {
            if (!inCharacterselect)
                StartGame();
        }
        if (p1Selected)
        {
            if (Input.GetButtonDown("Enter"))
                goToLevelSelect();
        }
        if (p2Selected)
        {
            if (Input.GetKeyDown("]"))
                goToLevelSelect();
        }
        if (Input.GetButtonDown("Enter"))
        {
            p1Selected = true;
            P1Icons[P1count].SetActive(true);
            P1Check.SetActive(true);
            anim1.SetBool("Selected", true);
            
            
        }
        if (Input.GetButtonDown("Attack") && !hovering)
        {
            p1Selected = true;
            P1Icons[P1count].SetActive(true);
            P1Check.SetActive(true);
            anim1.SetBool("Selected", true);
        }
        if (Input.GetButtonDown("Back"))
        {
            if (inCharacterselect)
            {
                p1Selected = false;
                P1Icons[P1count].SetActive(false);
                P1Check.SetActive(false);
                anim1.SetBool("Selected", false);
            }
            else
                gotoCharacterSelect();
            
        }

        if (Input.GetButtonDown("JumpP2"))
        {
            p2Selected = true;
            P2Icons[P2count].SetActive(true);
            P2Check.SetActive(true);
            anim2.SetBool("Selected", true);
            
        }
        if (Input.GetButtonDown("AttackP2"))
        {
            if (inCharacterselect)
            {
                p2Selected = false;
                P2Icons[P2count].SetActive(false);
                P2Check.SetActive(false);
                anim2.SetBool("Selected", false);
            }
            else
                gotoCharacterSelect();
        }

        if (Input.GetButtonDown("JumpP3"))
        {
            p3Selected = true;
            P3Icons[P3count].SetActive(true);
            P3Check.SetActive(true);
            anim3.SetBool("Selected", true);

        }
        if (Input.GetButtonDown("AttackP3"))
        {
            if (inCharacterselect)
            {
                p3Selected = false;
                P3Icons[P3count].SetActive(false);
                P3Check.SetActive(false);
                anim3.SetBool("Selected", false);
            }
            else
                gotoCharacterSelect();
        }

        if (Input.GetButtonDown("JumpP4"))
        {
            p4Selected = true;
            P4Icons[P4count].SetActive(true);
            P4Check.SetActive(true);
            anim4.SetBool("Selected", true);

        }
        if (Input.GetButtonDown("AttackP4"))
        {
            if (inCharacterselect)
            {
                p4Selected = false;
                P4Icons[P4count].SetActive(false);
                P4Check.SetActive(false);
                anim4.SetBool("Selected", false);
            }
            else
                gotoCharacterSelect();
        }

        if (p1Selected || p2Selected || p3Selected || p4Selected)
        {
            if(inCharacterselect)
                startButton.SetActive(true);
        }
        else
            startButton.SetActive(false);

        if (p1Selected)
        {
            if (Input.GetButtonDown("Start"))
            {
                if (inCharacterselect)
                {
                    goToLevelSelect();
                }
                else
                    StartGame();
            }
                
        }
        if (p2Selected)
        {
            if (Input.GetButtonDown("StartP2"))
            {
                if (inCharacterselect)
                {
                    goToLevelSelect();
                }
                else
                    StartGame();
            }
                
        }
        if (p3Selected)
        {
            if (Input.GetButtonDown("StartP3"))
            {
                if (inCharacterselect)
                {
                    goToLevelSelect();
                }
                else
                    StartGame();
            }

        }
        if (p4Selected)
        {
            if (Input.GetButtonDown("StartP4"))
            {
                if (inCharacterselect)
                {
                    goToLevelSelect();
                }
                else
                    StartGame();
            }

        }
    }
    public  void hoveringOn()
    {
        hovering = true;
    }
    public void hoveringOff()
    {
        hovering = false;
    }
    public string setCharacter(int IconIndex)
    {
        if (IconIndex == 1)//Albee
            return "A";
        else if (IconIndex == 2)//Stirfry
            return "S";
        else if (IconIndex == 3)//Debonesby
            return "D";
        else
        {
            
            return "";
        }
            
    }

    void goToLevelSelect() 
    {
        inCharacterselect = false;
        startButton.SetActive(false);
        LevelSelect.SetActive(true);
        characterSelectArt.SetActive(false);
        levelSelectArt.SetActive(true);
    }
    void gotoCharacterSelect()
    {
        inCharacterselect = true;
        startButton.SetActive(false);
        LevelSelect.SetActive(false);
        characterSelectArt.SetActive(true);
        levelSelectArt.SetActive(false);
    }
    public void setLevel(int levelnum)
    {
        levelNumber = levelnum;
    }

    public void StartGame()
    {
        if (p1Selected)
            MainManager.Instance.P1Char = setCharacter(P1count + 1);
        else
            MainManager.Instance.P1Char = setCharacter(99);
        if(p2Selected)
            MainManager.Instance.P2Char = setCharacter(P2count + 1);
        else
            MainManager.Instance.P2Char = setCharacter(99);
        if (p3Selected)
            MainManager.Instance.P3Char = setCharacter(P3count + 1);
        else
            MainManager.Instance.P3Char = setCharacter(99);
        if (p4Selected)
            MainManager.Instance.P4Char = setCharacter(P4count + 1);
        else
            MainManager.Instance.P4Char = setCharacter(99);

        Destroy(GameObject.FindGameObjectWithTag("MainTheme"));
        SceneManager.LoadScene("forestTest2");  
        if(levelNumber==1)
            SceneManager.LoadScene("forestTest2");
        else
            SceneManager.LoadScene("MotherTest1");
    }

}
