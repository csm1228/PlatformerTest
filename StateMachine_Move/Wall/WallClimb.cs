using Godot;
using System;

public partial class WallClimb : SubState
{
    public override void HandleTransState(double delta)
    {
        if (!Input.IsActionPressed(GamepadInput.Up))
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.ClimbSpeed;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
        }

        Player.Velocity = velocity;
    }
}
