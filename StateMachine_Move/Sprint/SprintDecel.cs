using Godot;
using System;

public partial class SprintDecel : State
{
    // SuperState : Airborne - 공중에 떠 있는 상태.
    // HandleTranState, HandlePressEvent 호출
    // 땅에 닿으면 Sprint_Grounded/Walk/Idle로 전환, RT 입력 시 공중 대쉬
    // HandlePhysics 호출 X. 자체 물리 사용

    [Export] Timer SprintDecelTimer { get; set; }

    public override void Enter()
    {

        StateMachine.AttachedToPlatform();

        Player.Animation.Play("Sprint_Decel");

        SprintDecelTimer.Start();
    }

    public override void Exit()
    {
        SprintDecelTimer.Stop();
    }


    public override void HandleTransState(double delta)
    {
        if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(State_Move.Idle);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    private void _on_sprint_decel_timer_timeout()
    {
        if (Input.IsActionPressed(GamepadInput.RT))
        {
            StateMachine.TransState(State_Move.Sprint_Grounded);
            return;
        }
        else
        {
            StateMachine.TransToWalkOrIdle();
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintDecelDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintDecelDelta);
            }
        }
        Player.Velocity = velocity;

        if (InputManager.Instance.Horizon < 0)
        {
            StateMachine.ActionDirection = Char.LREnum.Right;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            StateMachine.ActionDirection = Char.LREnum.Left;
        }

        //SuperState(Grounded)의 물리를 반영하지 않음
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
