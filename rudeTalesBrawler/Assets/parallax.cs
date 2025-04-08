using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float _startingPos, _startingPosY; //This is starting position of the sprites.
    private float _lengthOfSprite;    //This is the length of the sprites.
    public float AmountOfParallax;  //This is amount of parallax scroll. 
    public Camera MainCamera;   //Reference of the camera.
                                // Start is called before the first frame update
    public bool ParalaxY = false;


    public Vector3 offset = Vector3.zero;
    private void Start()
    {
        //Getting the starting X position of sprite.
        _startingPos = transform.position.x;
        _startingPosY = transform.position.y;
        //Getting the length of the sprites.
       
    }

    private void Update()
    {
        Vector3 Position = MainCamera.transform.position;
        float Temp = Position.x * (1 - AmountOfParallax);
        float Distance = Position.x * AmountOfParallax;

        float tempY = Position.y * (1 - AmountOfParallax);
        float DistanceY = Position.y * AmountOfParallax;

        if (ParalaxY)
        {
            Vector3 NewPositionY = new Vector3(_startingPos + Distance, _startingPosY + DistanceY, transform.position.z);
            transform.position = NewPositionY + offset;
        }
        else
        {
            Vector3 NewPosition = new Vector3(_startingPos + Distance, transform.position.y, transform.position.z);
            transform.position = NewPosition + offset;
        }
        
    }
}
