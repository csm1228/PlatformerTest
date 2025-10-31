using Godot;
using System;

public partial class WallClimb : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Wall_Climb");

        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.ClimbSpeed;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
            Player.Animation.FlipH = true;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
            Player.Animation.FlipH = false;
        }

        Player.Velocity = velocity;
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical >= 0)
        {
            if (InputManager.Instance.Horizon == 0)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Slipper);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.ClimbSpeed;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
            Player.Animation.FlipH = true;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
            Player.Animation.FlipH = false;
        }

        Player.Velocity = velocity;
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Up)
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
            return;
        }
    }
}
