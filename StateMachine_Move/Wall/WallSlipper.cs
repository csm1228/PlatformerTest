using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WallSlipper : State
{
    public override void Enter()
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

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

        Player.Animation.Play("Wall_Slipper");
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(State_Move.Wall_Climb);
            return;
        }
        else if (InputManager.Instance.Horizon < 0 && StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(State_Move.Wall_Hold);
            return;
        }
        else if (InputManager.Instance.Horizon > 0 && StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            StateMachine.TransState(State_Move.Wall_Hold);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (velocity.Y < Player.WallSlipperSpeed)
        {
            velocity.Y += (float)(Player.Gravity * delta * 0.15);
        }
        else
        {
            velocity.Y = Player.WallSlipperSpeed;
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
}
