using Godot;
using System;

public partial class LedgeClimb : SubState
{
    public override void Enter()
    {
        StateMachine.CheckWall();
    }

    public override void HandleTransState(double delta)
    {
        if (!Player.IsOnWall())
        {
            if (Player.IsOnFloor())
            {
                if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
                {
                    StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
                    return;
                }
                else
                {
                     StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
                    return;
                }
            }
        }
        else if (StateMachine.inputManager.IsJumpOnBuffer())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
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

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.WalkSpeed * 2;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = Player.WalkSpeed * 2;
        }

        Player.Velocity = velocity;
    }
}
