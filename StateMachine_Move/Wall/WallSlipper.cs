using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WallSlipper : SubState
{
    public override void HandleTransState(double delta)
    {
        if (Input.IsActionPressed(GamepadInput.Up))
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Climb);
        }
        else if ((Player.LastHoldingWallDirection == Char.LREnum.Left && Input.IsActionPressed(GamepadInput.Left) || Player.LastHoldingWallDirection == Char.LREnum.Right && Input.IsActionPressed(GamepadInput.Right)))
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (velocity.Y < Player.WallSlipperSpeed)
        {
            velocity.Y += (float)(Player.WallSlipperDelta * delta);
        }
        else
        {
            velocity.Y = Player.WallSlipperSpeed;
        }

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
