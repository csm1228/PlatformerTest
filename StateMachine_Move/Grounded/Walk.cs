using Godot;
using System;

public partial class Walk : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Walk");
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Horizon == 0)
        {
            StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
        }
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
}
