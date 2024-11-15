using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TItleManager : MonoBehaviour
{
    // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start")|| Input.GetButtonDown("StartP2")|| Input.GetButtonDown("Attack")|| Input.GetButtonDown("AttackP2"))
        {
            SceneManager.LoadScene("characterSelectTest");
        }
    } 
}
