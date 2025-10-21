using Godot;
using System;

public partial class Dash : SuperState
{
    [Export] private SubState Dash_Grounded { get; set; }
    [Export] private SubState Dash_Fall { get; set; }
    [Export] private SubState Dash_InAir { get; set; }

    public override void HandleTransState(double delta)
    {
        // 다른 SuperState로 전환할 필요가 있는지 먼저 검사
        // 벽 관련만
        if (Player.IsOnWall() && Player.IsOnFloor())
        {
            StateMachine.TransToWalkOrIdle();
            return;
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
}
