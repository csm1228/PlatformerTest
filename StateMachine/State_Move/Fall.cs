using Godot;
using System;

public partial class Fall : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void HandleTransState(double delta)
    {
        if (FSM.Player.IsOnFloor() && FSM.Player.Velocity.Y >= 0)
        {
            if (Input.IsActionPressed(InputName.dash))
            {
                FSM.TryTransState(StateName.Sprint);
            }
            else
            {
                FSM.TryTransState(StateName.Grounded);
            }
        }
        else if (FSM.TerrainDetector.GetLedgeDirection() != 0 && FSM.TerrainDetector.GetLedgeDirection() == FSM.InputManager.InputDir.X)
        {
            FSM.TryTransState(StateName.LedgeClimb);
        }

        else if (FSM.TerrainDetector.GetWallDirection() != 0)
        {
            if (FSM.TerrainDetector.GetWallDirection() == FSM.InputManager.InputDir.X)
            {
                FSM.TryTransState(StateName.WallHold);
            }
            else
            {
                FSM.TryTransState(StateName.WallSlipper);
            }
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.WalkSpeed * FSM.InputManager.InputDir.X;

        if (FSM.velocity.Y < FSM.MaxFallSpeed)
        {
            FSM.velocity.Y += (float)(FSM.FallAccel * delta);
        }
        else
        {
            FSM.velocity.Y = FSM.MaxFallSpeed;
        }

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.DashInAir);
        }
        else if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.DoubleJump);
        }
        else if (@event.IsActionReleased(InputName.jump))
        {
            FSM.TryTransState(StateName.Fall);
        }
    }
}
