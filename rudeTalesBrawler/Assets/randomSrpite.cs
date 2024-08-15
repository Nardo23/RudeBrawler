using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSrpite : MonoBehaviour
{
    [SerializeField] public Sprite[] sprites;
    Sprite rolledSprite;
    [SerializeField] public SpriteRenderer[] Rends;
    // Start is called before the first frame update
    void Start()
    {
        rolledSprite = sprites[Random.Range(0, sprites.Length)];
        foreach(SpriteRenderer r in Rends)
        {
            r.sprite = rolledSprite;
        }
    }

}
