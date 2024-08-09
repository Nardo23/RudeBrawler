using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] public string HorizontalControls;
    [SerializeField] public string VerticalControls;

    [SerializeField] public string JumpButton;
    [SerializeField] public string AttackButton;
    [SerializeField] public string SpecialButton;
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
