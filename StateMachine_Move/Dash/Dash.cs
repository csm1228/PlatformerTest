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
            if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
                return;
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.CheckWall();

            if (Player.IsOnWall())
            {
                GD.Print("Dash to Wall_Hold");
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
            else if (StateMachine.IsOnLedge())
            {
                GD.Print("Dash to Ledge_Grab");
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                return;
            }
            else
            {
                GD.Print("Dash to Fall");
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
