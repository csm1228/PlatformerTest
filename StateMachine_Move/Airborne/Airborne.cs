using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Airborne : SuperState
{
    [Export] private SubState Jump { get; set; }
    [Export] private SubState Apex { get; set; }
    [Export] private SubState Fall { get; set; }
    [Export] private SubState Wall_Jump { get; set; }

    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
        InputManager.Instance.ActionReleased += HandleReleasedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
        InputManager.Instance.ActionReleased -= HandleReleasedEvent;
    }

    public override void HandleTransState(double delta)
    {
        // 착지 시점에 입력에 따라 상태 변경
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                if (InputManager.Instance.Horizon != 0)
                {
                    Player.ActionDirection = Player.LastInputDirection;
                }
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.TransWall();
            return;
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            // 공중 대쉬는, 대쉬 가능 여부만 검사
            if (StateMachine.CanDash)
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
                return;
            }
        }
        // 더블 점프를 별개의 점프로 만드는 것이 나을 것 같음. 지금은 비활성화
        else if (action == GamepadInput.Face_Down)
        {
            if (StateMachine.CanDoubleJump && StateMachine.IsDoubleJumpUnlocked)
            {
                StateMachine.CanDoubleJump = false;
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
                return;
            }
        }

        CurrentSubState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        CurrentSubState.HandleReleasedEvent(action);
    }
}
