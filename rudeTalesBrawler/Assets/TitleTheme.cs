using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTheme : MonoBehaviour
{
    public static TitleTheme Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
