using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string HorizontalControls;
    [SerializeField] private string VerticalControls;

    [SerializeField] private KeyCode JumpButton;
    [SerializeField] private KeyCode AttackButton;

    Controls controls = new Controls();

    public Controls GetInput()
    {
        controls.HorizontalMove = Input.GetAxis(HorizontalControls);
        controls.VerticalMove = Input.GetAxis(VerticalControls);
        controls.JumpState = Input.GetKeyDown(JumpButton);
        controls.AttackState = Input.GetKeyDown(AttackButton);

        return controls;
    } 
}
