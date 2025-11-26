using Godot;
using System;

public partial class Crouch : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.Hitbox.Disabled = true;
        FSM.Hitbox_Crouch.Disabled = false;
    }

    public override void Exit()
    {
        FSM.Hitbox.Disabled = false;
        FSM.Hitbox_Crouch.Disabled = true;
    }

    public override void HandleTransState(double delta)
    {
        if (!FSM.Player.IsOnFloor())
        {
            FSM.TryTransState(StateName.Fall);
        }
        else if (FSM.InputManager.JumpBuffer > 0)
        {
            FSM.TryTransState(StateName.Jump);
        }
        else if (!Input.IsActionPressed(InputName.down))
        {
            FSM.TryTransState(StateName.Grounded);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.CrouchSpeed * FSM.InputManager.InputDir.X;

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.Jump);
        }
        else if (@event.IsActionReleased(InputName.down))
        {
            FSM.TryTransState(StateName.Grounded);
        }
    }
}
