using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("backspace")||Input.GetButtonDown("Start")|| Input.GetButtonDown("StartP2"))
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown("=")||Input.GetButtonDown("Select")|| Input.GetButtonDown("SelectP2"))
        {
            SceneManager.LoadScene("characterSelectTest");
        }
       
    }
    



}

