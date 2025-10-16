using Godot;
using System;

public partial class Walk : SubState
{
    public override void HandleTransState(double delta)
    {
        if (!Input.IsActionPressed(GamepadInput.Left) && !Input.IsActionPressed(GamepadInput.Right))
        {
            StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
        }
    }

    public override void HandlePhysics(double delta)
    {

        Vector2 velocity = Player.Velocity;

        if (Input.IsActionPressed(GamepadInput.Left))
        {
            velocity.X = -Player.WalkSpeed;
        }
        else if (Input.IsActionPressed(GamepadInput.Right))
        {
            velocity.X = Player.WalkSpeed;
        }

        Player.Velocity = velocity;
    }
}
