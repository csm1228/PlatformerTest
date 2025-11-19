using Godot;
using System;

public partial class SprintApex : State
{
    // SuperState : Airborne - 공중에 떠 있는 상태.
    // HandleTranState, HandlePressEvent 호출
    // 땅에 닿으면 Sprint_Grounded/Walk/Idle로 전환, RT 입력 시 공중 대쉬
    // HandlePhysics 호출 X. 자체 물리 사용

    [Export] Timer SprintApexTimer { get; set; }

    public override void Enter()
    {
        SprintApexTimer.Start();

        Player.Animation.Play("Sprint_Apex");
    }

    public override void Exit()
    {
        SprintApexTimer.Stop();
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
            StateMachine.TransState(State_Move.Ledge_Climb);
            return;
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(State_Move.Wall_Hold);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    private void _on_sprint_apex_timer_timeout()
    {
        StateMachine.TransState(State_Move.Fall);
        return;
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintApexDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintApexDelta);
            }
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Apex);

        Player.Velocity = velocity;

        // SuperState(Airborne)의 물리를 반영하지 않음
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
