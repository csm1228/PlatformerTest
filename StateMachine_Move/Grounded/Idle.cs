using Godot;
using System;

public partial class Idle : State
{
    public override void Enter()
    {
        Player.Animation.Play("Idle");
        StateMachine.AttachedToPlatform();
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Horizon != 0)
        {
            StateMachine.TransState(State_Move.Walk);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.X = 0;

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
