using Godot;
using System;

public partial class LedgeClimb : State
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
            FSM.TryTransState(StateName.Jump);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        if (FSM.Player.IsOnWall())
        {
            FSM.velocity.Y = FSM.WallClimbSpeed_Fast;
        }
        else
        {
            FSM.velocity.Y = -FSM.WallClimbSpeed;
        }

        FSM.velocity.X = FSM.SprintSpeed * FSM.ActionDirection;

        FSM.Player.Velocity = FSM.velocity;
    }
    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.Jump);
        }
    }
}
