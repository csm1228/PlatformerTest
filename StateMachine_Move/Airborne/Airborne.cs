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
        else if (Input.IsActionJustPressed(GamepadInput.RT) && StateMachine.CooldownManager.IsDashReady)
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
            return;
        }
        else if (Player.IsOnWallOnly())
        {
            StateMachine.CheckWall();
            
            if (Player.LastHoldingWallDirection == Char.LREnum.Left)
            {
                if (Player.RayCast_Lower_Left.IsColliding())
                {
                    if (Player.RayCast_Upper_Left.IsColliding())
                    {
                        StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                        return;
                    }
                    else
                    {
                        StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                        return;
                    }
                }
                else
                {
                    StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                    return;
                }
            }
            else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
            {
                if (Player.RayCast_Lower_Right.IsColliding())
                {
                    if (Player.RayCast_Upper_Right.IsColliding())
                    {
                        StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                        return;
                    }
                    else
                    {
                        StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                        return;
                    }
                }
                else
                {
                    StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                    return;
                }
            }
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
