using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls
{
    float horizontalMove;
    float verticalMove;
    bool jumpState;
    bool attackState;

    public float HorizontalMove { get => horizontalMove; set => horizontalMove = value; }
    public float VerticalMove { get => verticalMove; set => verticalMove = value; }
    public bool JumpState { get => jumpState; set => jumpState = value; }
    public bool AttackState { get => attackState; set => attackState = value; }
}
