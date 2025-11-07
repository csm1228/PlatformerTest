using Godot;
using System;

public partial class Jump : State
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        // 점프 버퍼 소비
        Player.ConsumeJumpBuffer();

        // 최초 점프 속도로 설정
        Vector2 velocity = Player.Velocity;
        velocity.Y = Player.JumpSpeed;
        Player.Velocity = velocity;

        Player.Animation.Play("Jump");

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
            StateMachine.TransState(State_Move.Fall);
            return;
        }
        else if (!Input.IsActionPressed(GamepadInput.Face_Down))
        {
            StateMachine.TransState(State_Move.Apex);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(State_Move.Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 수직 상승 속도 점차 감소
        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;

        SuperState.HandlePhysics(delta);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down) // 점프 키 떼면 점프 중단. 가변 점프
        {
            StateMachine.TransState(State_Move.Apex);
            return;
        }
    } 

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
