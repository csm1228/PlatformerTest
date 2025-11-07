using Godot;
using System;

public partial class Fall : State
{
    public override void Enter()
    {
        // 진입 시 수직 속도를 0으로 변경

        // 애니메이션
        Player.Animation.Play("Fall");

        if (InputManager.Instance.Horizon < 0)
        {
            Player.Animation.FlipH = true;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            Player.Animation.FlipH = false;
        }

    }

    public override void HandleTransState(double delta)
    {
        if (StateMachine.IsOnLedge())
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

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 최대 추락 속도까지 가속
        if (velocity.Y < Player.MaxFallSpeed)
        {
            velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Fall);
        }
        else
        {
            velocity.Y = Player.MaxFallSpeed;
        }

        Player.Velocity = velocity;

        SuperState.HandlePhysics(delta);
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
