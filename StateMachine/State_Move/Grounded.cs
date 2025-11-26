using Godot;
using System;

public partial class Grounded : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();
    }

    public override void Exit()
    {
        
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
        else if (Input.IsActionPressed(InputName.down))
        {
            FSM.TryTransState(StateName.Crouch);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.WalkSpeed * FSM.InputManager.InputDir.X;

        FSM.Player.Velocity = FSM.velocity;

    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.Dash);
        }
        else if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.Jump);
        }
        else if (@event.IsActionPressed(InputName.map))
        {
            FSM.TryTransState(StateName.Map);
        }
        else if (@event.IsActionPressed(InputName.down))
        {
            FSM.TryTransState(StateName.Crouch);
        }
    }
}
