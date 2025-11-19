using Godot;
using System;

public partial class Apex : State
{
    // SuperState : Airborne - 공중에 떠 있는 상태.
    // HandleTranState, HandlePressEvent 호출
    // 땅에 닿으면 Sprint_Grounded/Walk/Idle로 전환, RT 입력 시 공중 대쉬
    // HandlePhysics 호출 -> 좌우 입력으로 자유롭게 수평 이동 가능.

    [Export] Timer ApexTimer { get; set; }

    public override void Enter()
    {
        Player.Animation.Play("Apex");

        ApexTimer.Start();
    }

    public override void Exit()
    {
        ApexTimer.Stop();
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

    private void _on_apex_timer_timeout()
    {
        StateMachine.TransState(State_Move.Fall);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 수직 속도가 점차 감소. 점프보다 더 빠르게 감소함.
        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Apex);

        Player.Velocity = velocity;

        SuperState.HandlePhysics(delta);
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
