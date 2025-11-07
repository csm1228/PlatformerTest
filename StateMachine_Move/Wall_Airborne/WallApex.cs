using Godot;
using System;

public partial class WallApex : State
{
    [Export] Timer WallApexTimer { get; set; }

    public override void Enter()
    {
        Player.Animation.Play("Wall_Apex");

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
            StateMachine.TransState(State_Move.Fall);
            return;
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.CheckLedgeDirection();

            if (StateMachine.HoldingLedgeDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 || StateMachine.HoldingLedgeDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0)
            {
                StateMachine.TransState(State_Move.Ledge_Climb);
                return;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.CheckWallDirection();

            if (StateMachine.HoldingWallDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 || StateMachine.HoldingWallDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0)
            {
                StateMachine.TransState(State_Move.Wall_Hold);
                return;
            }
        }

        SuperState.HandleTransState(delta);
    }

    private void _on_wall_apex_timer_timeout()
    {
        StateMachine.TransState(State_Move.Fall);
        return;
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 관성 기반 좌우 움직임
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

        // SuperState(Airborne)의 물리는 반영하지 않음.
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
