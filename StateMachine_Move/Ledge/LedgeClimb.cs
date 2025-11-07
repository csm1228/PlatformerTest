using Godot;
using System;

public partial class LedgeClimb : State
{
    // SuperState 없음

    public override void Enter()
    {
        StateMachine.AttachedToPlatform();

        Player.Animation.Play("Ledge_Climb");

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
        if (!Player.IsOnWall())
        {
            if (Player.IsOnFloor())
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }
        else if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(State_Move.Jump);
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.IsOnWall())
        {
            velocity.Y = Player.ClimbSpeed * 2;
        }
        else
        {
            velocity.Y = -Player.ClimbSpeed * 2;
        }

        if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.WalkSpeed * 2;
        }
        else if (StateMachine.HoldingLedgeDirection == Char.LREnum.Right)
        {
            velocity.X = Player.WalkSpeed * 2;
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(State_Move.Jump);
            return;
        }
    }
}
