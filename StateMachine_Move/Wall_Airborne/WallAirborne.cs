using Godot;
using System;

public partial class WallAirborne : SuperState
{
    [Export] private SubState Wall_Jump { get; set; }
    [Export] private SubState Wall_Apex { get; set; }

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
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
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

        CurrentSubState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        CurrentSubState.HandleReleasedEvent(action);
    }
}
