using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (Input.GetButtonDown("Enter"))
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


    }
}
