using Godot;
using System;

public partial class Map : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.EmitSignal(StateMachine_Move.SignalName.OpenMap);
    }

    public override void Exit()
    {
        FSM.EmitSignal(StateMachine_Move.SignalName.CloseMap);
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
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.MapWalkSpeed * FSM.InputManager.InputDir.X;

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.Jump);
        }
        else if (@event.IsActionReleased(InputName.map))
        {
            FSM.TryTransState(StateName.Grounded);
        }
    }
}
