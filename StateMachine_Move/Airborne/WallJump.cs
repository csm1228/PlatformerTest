using Godot;
using System;
using System.ComponentModel.Design;

public partial class WallJump : SubState
{
    [Export] Timer MaxWallJumpTime { get; set; }

    public override void Enter()
    {
        Player.ConsumeJumpBuffer();

        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.JumpSpeed;

        if (StateMachine.IsOnLedge())
        {
            if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left)
            {
                StateMachine.ActionDirection = Char.LREnum.Right;
                StateMachine.PlayerFacingDirection = Char.LREnum.Right;
                velocity.X = Player.WallJumpSpeed;
            }
            else if (StateMachine.HoldingLedgeDirection == Char.LREnum.Right)
            {
                StateMachine.ActionDirection = Char.LREnum.Left;
                StateMachine.PlayerFacingDirection = Char.LREnum.Left;
                velocity.X = -Player.WallJumpSpeed;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            if (StateMachine.HoldingWallDirection == Char.LREnum.Left)
            {
                StateMachine.ActionDirection = Char.LREnum.Right;
                StateMachine.PlayerFacingDirection = Char.LREnum.Right;
                velocity.X = Player.WallJumpSpeed;
            }
            else if (StateMachine.HoldingWallDirection == Char.LREnum.Right)
            {
                StateMachine.ActionDirection = Char.LREnum.Left;
                StateMachine.PlayerFacingDirection = Char.LREnum.Left;
                velocity.X = -Player.WallJumpSpeed;
            }
        }

        Player.Velocity = velocity;

        MaxWallJumpTime.Start();

        Player.Animation.Play("Jump");
    }

    public override void Exit()
    {
        MaxWallJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (!Input.IsActionPressed(GamepadInput.Face_Down))
        {
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Apex);
            return;
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.CheckLedgeDirection();

            if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 && StateMachine.ActionDirection == Char.LREnum.Left)
            {
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                return;
            }
            else if (StateMachine.HoldingLedgeDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0 && StateMachine.ActionDirection == Char.LREnum.Right)
            {
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                return;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.CheckWallDirection();

            if (StateMachine.HoldingWallDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 && StateMachine.ActionDirection == Char.LREnum.Left)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
            else if (StateMachine.HoldingWallDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0 && StateMachine.ActionDirection == Char.LREnum.Right)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
        }
    }

    private void _on_max_wall_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.WallJumpDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.WallJumpDelta);
            }
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Apex);
            return;
        }
    }
}
