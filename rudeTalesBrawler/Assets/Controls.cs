using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls
{
    float horizontalMove;
    float verticalMove;

    bool jumpState;
    bool attackState;
    bool specialAttackState;
    bool specialAttackStartState;
    public float HorizontalMove { get => horizontalMove; set => horizontalMove = value; }
    public float VerticalMove { get => verticalMove; set => verticalMove = value; }
    public bool JumpState { get => jumpState; set => jumpState = value; }
    public bool AttackState { get => attackState; set => attackState = value; }
    public bool SpecialAttackState { get => specialAttackState; set => specialAttackState = value; }
    public bool SpecialAttackStartState { get => specialAttackStartState; set => specialAttackStartState = value; }

}
