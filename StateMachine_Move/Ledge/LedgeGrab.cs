using Godot;
using System;

public partial class LedgeGrab : SubState
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

    public override void HandleTransState(double delta)
    {
        if (!Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Input.IsActionPressed(GamepadInput.Up))
        {
            StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
            return;
        }
        else if (Input.IsActionPressed(GamepadInput.Left))
        {
            if (Player.LastHoldingWallDirection == Char.LREnum.Left)
            {
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
        }
        else if (Input.IsActionPressed(GamepadInput.Right))
        {
            if (Player.LastHoldingWallDirection == Char.LREnum.Right)
            {
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
        }
    }
}
