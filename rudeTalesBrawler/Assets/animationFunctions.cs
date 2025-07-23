using UnityEngine;

public class animationFunctions : MonoBehaviour
{
    public GameObject disableObj;
    public GameObject enableObj;
    public Animator anim;
    void disableOb()
    {
        disableObj.SetActive(false);
    }

    void enableOb()
    {
        enableObj.SetActive(true);
    }

   void playAnimator()
    {
        anim.SetTrigger("play");
    }
}
