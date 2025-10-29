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
        else if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
            return;
        }
        else if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
            return;
        }
        else if (InputManager.Instance.Horizon < 0 && Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (InputManager.Instance.Horizon > 0 && Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
            return;
        }
    }
}
