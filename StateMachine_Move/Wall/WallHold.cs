using Godot;
using System;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

public partial class WallHold : SubState
{
    public override void Enter()
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

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

    public override void Exit()
    {

    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Climb);
            return;
        }

        else if (InputManager.Instance.Horizon == 0)
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Slipper);
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

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

    public override void HandlePressedEvent(StringName action)
    {

    }

    public override void HandleReleasedEvent(StringName action)
    {

        
    }
}
