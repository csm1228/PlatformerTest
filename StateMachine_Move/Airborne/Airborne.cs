using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Airborne : SuperState
{
    [Export] private SubState Jump { get; set; }
    [Export] private SubState Apex { get; set; }
    [Export] private SubState Fall { get; set; }
    [Export] private SubState Wall_Jump { get; set; }
    public override void HandleTransState(double delta)
    {
        // 다른 SuperState로 전환할 필요가 있는지 먼저 검사
        if (Player.IsOnFloor() && Player.Velocity.Y <= 0)
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                if (StateMachine.IsInputLorR())
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
        else if (Input.IsActionJustPressed(GamepadInput.RT) && StateMachine.CooldownManager.IsDashReady)
        {
            StateMachine.FixActionDirection();
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
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
