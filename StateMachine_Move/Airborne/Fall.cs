using Godot;
using System;

public partial class Fall : SubState
{
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

        if (velocity.Y < Player.Gravity)
        {
            velocity.Y += (float)(7000 * delta);
        }
        else
        {
            velocity.Y = Player.Gravity;
        }

        Player.Velocity = velocity;
    }
}
