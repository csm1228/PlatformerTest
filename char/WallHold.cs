using Godot;
using System;

public partial class WallHold : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.ActionDirection = -FSM.Player.GetWallNormal().X;
    }

    public override void Exit()
    {

    }

    public override void HandleTransState(double delta)
    {
        if (FSM.Player.IsOnFloor())
        {
            FSM.TryTransState(StateName.Grounded);
        }
        else if (FSM.InputManager.JumpBuffer > 0)
        {
            FSM.TryTransState(StateName.WallJump);
        }
        else if (FSM.ActionDirection != FSM.TerrainDetector.GetWallDirection())
        {
            FSM.TryTransState(StateName.Fall);
        }
        else if (FSM.TerrainDetector.GetLedgeDirection() != 0)
        {
            FSM.TryTransState(StateName.LedgeClimb);
        }

        else if (FSM.InputManager.InputDir.X == -FSM.ActionDirection)
        {
            FSM.TryTransState(StateName.Fall);
        }
        else if (FSM.InputManager.InputDir.Y < 0)
        {
            FSM.TryTransState(StateName.WallClimb);
        }
        else if (FSM.InputManager.InputDir.X == 0)
        {
            FSM.TryTransState(StateName.WallSlipper);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.Y = 0;

        FSM.Player.Velocity = FSM.velocity;
    }
    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.WallJump);
        }
    }
}
