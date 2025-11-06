using Godot;
using System;

public partial class WallApex : SubState
{
    [Export] Timer WallApexTimer { get; set; }

    public override void Enter()
    {
        Player.Animation.Play("Apex");

        Vector2 velocity = Player.Velocity;

        if (velocity.X < -Player.WalkSpeed)
        {
            velocity.X = -Player.WalkSpeed;
        }
        else if (velocity.X > Player.WalkSpeed)
        {
            velocity.X = Player.WalkSpeed;
        }

        Player.Velocity = velocity;

        WallApexTimer.Start();
    }

    public override void Exit()
    {
        WallApexTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.CheckLedgeDirection();

            if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 || StateMachine.HoldingLedgeDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0)
            {
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                return;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.CheckWallDirection();

            if (StateMachine.HoldingWallDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 || StateMachine.HoldingWallDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
        }
    }

    private void _on_wall_apex_timer_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        return;
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 조작 방향대로 수평 이동
        if (InputManager.Instance.Horizon < 0)
        {
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;

            if (velocity.X < -Player.WalkSpeed)
            {
                velocity.X = -Player.WalkSpeed;
            }
            else
            {
                velocity.X -= (float)(delta * Player.WallApexDelta);
            }
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;

            if (velocity.X > Player.WalkSpeed)
            {
                velocity.X = Player.WalkSpeed;
            }
            else
            {
                velocity.X += (float)(delta * Player.WallApexDelta);
            }
        }
        // 수직 속도가 점차 감소. 점프보다 더 빠르게 감소함.
        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Apex);

        Player.Velocity = velocity;
    }
}
