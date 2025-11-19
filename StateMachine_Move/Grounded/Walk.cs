using Godot;
using System;

public partial class Walk : State
{
    // SuperState : Grounded - 지상에 붙어있는 상태.
    // SuperState의 HandleTransState, HandlePressedEvent 호출
    // 점프 버퍼 or 점프 입력 -> Jump로 전환, 땅에 붙어있지 않으면 Fall로 전환
    public override void Enter()
    {
        Player.Animation.Play("Walk");
        StateMachine.AttachedToPlatform();
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Horizon == 0)
        {
            StateMachine.TransState(State_Move.Idle);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        // 입력 방향에 따라 수평 이동
        if (InputManager.Instance.Horizon < 0)
        {
            velocity.X = -Player.WalkSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            velocity.X = Player.WalkSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
