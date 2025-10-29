using Godot;
using System;

public partial class Jump : SubState
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        // 점프 버퍼 소비
        Player.ConsumeJumpBuffer();

        // 최초 점프 속도
        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;

        // 점프 타이머 시작
        MaxJumpTime.Start();
    }

    public override void Exit()
    {
        MaxJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 조작 방향대로 수평 이동
        if (InputManager.Instance.Horizon < 0)
        {
            velocity.X = -Player.WalkSpeed;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            velocity.X = Player.WalkSpeed;
        }
        else
        {
            velocity.X = 0;
        }

        // 수직 상승 속도 점차 감소
        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
            return;
        }
    }
}
