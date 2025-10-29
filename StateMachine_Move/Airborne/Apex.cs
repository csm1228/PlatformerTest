using Godot;
using System;

public partial class Apex : SubState
{
    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Player.Velocity.Y >= 0)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
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
        else
        {
            velocity.X = 0;
        }

        // 수직 속도가 점차 감소. 점프보다 더 빠르게 감소함.
        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Apex);

        Player.Velocity = velocity;
    }
}
