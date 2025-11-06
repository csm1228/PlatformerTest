using Godot;
using System;

public partial class LedgeGrab : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Wall_Hold");

        Vector2 velocity = Player.Velocity;
        velocity = Vector2.Zero;
        Player.Velocity = velocity;

        StateMachine.CheckLedgeDirection();

        if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left)
        {
            Player.Animation.FlipH = true;
        }
        else if (StateMachine.HoldingLedgeDirection == Char.LREnum.Right)
        {
            Player.Animation.FlipH = false;
        }

    }

    public override void HandleTransState(double delta)
    {
        if (!StateMachine.IsOnLedge())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Jump);
            return;
        }
        else if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
            return;
        }
        else if (InputManager.Instance.Horizon < 0 && StateMachine.HoldingLedgeDirection == Char.LREnum.Right)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (InputManager.Instance.Horizon > 0 && StateMachine.HoldingLedgeDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Jump);
            return;
        }
    }
}
