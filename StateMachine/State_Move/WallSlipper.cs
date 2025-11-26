using Godot;
using System;

public partial class WallSlipper : State
{
    public override void Enter()
    {
        if (FSM.IsWallActionUnlocked)
        {
            FSM.RestoreAirAction();
        }

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


        else if (FSM.InputManager.InputDir.X == -FSM.ActionDirection)
        {
            FSM.TryTransState(StateName.Fall);
        }
        else if (FSM.InputManager.InputDir.Y < 0)
        {
            FSM.TryTransState(StateName.WallClimb);
        }
        else if (FSM.InputManager.InputDir.X == FSM.ActionDirection)
        {
            FSM.TryTransState(StateName.WallHold);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        if (FSM.velocity.Y < 0)
        {
            FSM.velocity.Y = Mathf.MoveToward(FSM.velocity.Y, FSM.MaxSlipperSpeed, (float)((FSM.JumpAccel + FSM.SlipperAccel) * delta));
        }
        else if (FSM.velocity.Y < FSM.MaxSlipperSpeed)
        {
            FSM.velocity.Y = Mathf.MoveToward(FSM.velocity.Y, FSM.MaxSlipperSpeed, (float)((FSM.SlipperAccel) * delta));
        }
        else
        {
            FSM.velocity.Y = FSM.MaxSlipperSpeed;
        }

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
