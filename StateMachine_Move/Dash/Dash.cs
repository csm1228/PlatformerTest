using Godot;
using System;

public partial class Dash : SuperState
{
    [Export] private SubState Dash_Grounded { get; set; }
    [Export] private SubState Dash_Fall { get; set; }
    [Export] private SubState Dash_InAir { get; set; }

    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
    }

    public override void HandleTransState(double delta)
    {
        // 공중에서 벽에 박으면 벽에 붙음
        if (Player.IsOnWallOnly())
        {
            StateMachine.TransWall();
            return;
        }
        // 구석에 박히면 Walk 아니면 Idle. 지상 대쉬가 벽에 박으면 멈추는 효과도 있음
        else if (Player.IsOnWall() && Player.IsOnFloor())
        {
            StateMachine.TransToWalkOrIdle();
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
        CurrentSubState.HandlePressedEvent(action);
    }
}
