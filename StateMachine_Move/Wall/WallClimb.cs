using Godot;
using System;

public partial class WallClimb : State
{
    public override void Enter()
    {
        StateMachine.AttachedToPlatform();

        Player.Animation.Play("Wall_Climb");

        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.ClimbSpeed;

        if (StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
        }
        else if (StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
        }

        Player.Velocity = velocity;
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical >= 0)
        {
            if (InputManager.Instance.Horizon == 0)
            {
                StateMachine.TransState(State_Move.Wall_Slipper);
                return;
            }
            else
            {
                StateMachine.TransState(State_Move.Wall_Hold);
                return;
            }
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.ClimbSpeed;

        if (Input.IsActionPressed(GamepadInput.RT))
        {
            velocity.Y = Player.ClimbSpeed * 1.5f;

        }

        if (StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
        }
        else if (StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Up)
        {
            StateMachine.TransState(State_Move.Wall_Hold);
            return;
        }
    }
}
