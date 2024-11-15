using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterSelect : MonoBehaviour
{
    [SerializeField]
    public Transform[] iconPositions;
    public GameObject P1Cursor, P2Cursor , P1Check, P2Check;
    int P1count = 0, P2count = 0;
    float timer1 = 99, timer2 = 99;
    float cursorSpeed = .2f;
    bool p1Selected, p2Selected;
    public GameObject startButton;
    [SerializeField] GameObject[] P1Icons, P2Icons;
    Animator anim1, anim2;
   public bool hovering = false;
    // Start is called before the first frame update
    void Start()
    {
        P1Cursor.transform.position = new Vector3(iconPositions[0].position.x - .65f, iconPositions[0].position.y, 0);
        P2Cursor.transform.position = new Vector3(iconPositions[0].position.x + .65f, iconPositions[0].position.y, 0);
        anim1 = P1Cursor.GetComponent<Animator>();
        anim2 = P2Cursor.GetComponent<Animator>();
        anim2.Play("cursorBounce", -1, .2f);
    }

    // Update is called once per frame
    void Update()
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
        
        

        P1Cursor.transform.position = new Vector3(iconPositions[P1count].position.x - .65f, iconPositions[P1count].position.y, 0);
        P2Cursor.transform.position = new Vector3(iconPositions[P2count].position.x + .65f, iconPositions[P2count].position.y, 0);
        if (p1Selected)
        {
            if (Input.GetButtonDown("Enter"))
                StartGame();
        }
        if (p2Selected)
        {
            if (Input.GetKeyDown("]"))
                StartGame();
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
            p1Selected = false;
            P1Icons[P1count].SetActive(false);
            P1Check.SetActive(false);
            anim1.SetBool("Selected", false);
            
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
            p2Selected = false;
            P2Icons[P2count].SetActive(false);
            P2Check.SetActive(false);
            anim2.SetBool("Selected", false);
           
        }

        if (p1Selected || p2Selected)
        {
            startButton.SetActive(true);
        }
        else
            startButton.SetActive(false);

        if (p1Selected)
        {
            if (Input.GetButtonDown("Start"))
                StartGame();
        }
        if (p2Selected)
        {
            if (Input.GetButtonDown("StartP2"))
                StartGame();
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
        Destroy(GameObject.FindGameObjectWithTag("MainTheme"));
        SceneManager.LoadScene("forestTest2");       
    }

}
