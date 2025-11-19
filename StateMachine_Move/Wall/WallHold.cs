using Godot;
using System;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

public partial class WallHold : State
{
    // SuperState : Wall - 벽에 붙어있는 상태.
    // HandleTranState, HandlePressEvent 호출
    // 땅에 닿았을 때 지상 상태로 전환, 벽에 사라지거나 벽 반대 방향으로 입력 시 Fall 상태로 전환, 점프 버퍼가 있거나 점프 입력 시 WallJump로 전환한다.

    public override void Enter()
    {
        StateMachine.AttachedToPlatform();

        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

        if (StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
        }
        else if (StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
        }

        Player.Velocity = velocity;

        Player.Animation.Play("Wall_Hold");
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Vertical < 0)
        {
            StateMachine.TransState(State_Move.Wall_Climb);
            return;
        }

        else if (InputManager.Instance.Horizon == 0)
        {
            StateMachine.TransState(State_Move.Wall_Slipper);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

        if (StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
        }
        else if (StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }
}
