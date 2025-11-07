using Godot;
using System;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

public partial class WallHold : State
{
    public override void Enter()
    {
        StateMachine.AttachedToPlatform();

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

        Player.Animation.Play("Wall_Hold");
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(State_Move.Wall_Climb);
            return;
        }

        else if (InputManager.Instance.Horizon == 0)
        {
            StateMachine.TransState(State_Move.Wall_Slipper);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

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
