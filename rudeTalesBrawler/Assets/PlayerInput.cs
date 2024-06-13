using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string HorizontalControls;
    [SerializeField] private string VerticalControls;

    [SerializeField] private string JumpButton;
    [SerializeField] private string AttackButton;
    [SerializeField] private string SpecialButton;
    //[SerializeField] private KeyCode test;
    Controls controls = new Controls();

    public Controls GetInput()
    {
        controls.HorizontalMove = Input.GetAxis(HorizontalControls);
        controls.VerticalMove = Input.GetAxis(VerticalControls);
        controls.JumpState = Input.GetButtonDown(JumpButton);
        controls.AttackState = Input.GetButtonDown(AttackButton);
        controls.SpecialAttackState = Input.GetButton(SpecialButton);
        controls.SpecialAttackStartState = Input.GetButtonDown(SpecialButton);
        return controls;
    } 
}
