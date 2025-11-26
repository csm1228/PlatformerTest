using Godot;
using System;

public partial class Sprint : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.FixActionDirection();
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
            FSM.TryTransState(StateName.SprintJump);
        }
        else if (!Input.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.Grounded);
        }
        else if (FSM.ActionDirection == -FSM.InputManager.InputDir.X)
        {
            FSM.TryTransState(StateName.Decel);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.SprintSpeed * FSM.ActionDirection;

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.SprintJump);
        }
        else if (@event.IsActionReleased(InputName.dash))
        {
            FSM.TryTransState(StateName.Grounded);
        }
    }
}
