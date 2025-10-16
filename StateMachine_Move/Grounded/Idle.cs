using Godot;
using System;

public partial class Idle : SubState
{
    public override void HandleTransState(double delta)
    {
        if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
        {
            StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
        }
    }

    public override void HandlePhysics(double delta)
    {

        Vector2 velocity = Player.Velocity;

        velocity.X = 0;

        Player.Velocity = velocity;
    }
}
