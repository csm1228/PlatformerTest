using Godot;
using System;

public partial class Fall : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Fall");

        if (InputManager.Instance.Horizon < 0)
        {
            Player.Animation.FlipH = true;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            Player.Animation.FlipH = false;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (InputManager.Instance.Horizon < 0)
        {
            velocity.X = -Player.WalkSpeed;
            Player.Animation.FlipH = true;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            velocity.X = Player.WalkSpeed;
            Player.Animation.FlipH = false;
        }
        else
        {
            velocity.X = 0;
        }

        if (velocity.Y < Player.MaxFallSpeed)
        {
            velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Fall);
        }
        else
        {
            velocity.Y = Player.MaxFallSpeed;
        }

        Player.Velocity = velocity;
    }
}
