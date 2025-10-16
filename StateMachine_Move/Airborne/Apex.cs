using Godot;
using System;

public partial class Apex : SubState
{
    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        }
        if (Player.Velocity.Y >= 0)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        }
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
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
        else
        {
            velocity.X = 0;
        }

        velocity.Y += 200;

        Player.Velocity = velocity;
    }
}
