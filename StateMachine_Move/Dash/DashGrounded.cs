using Godot;
using System;

public partial class DashGrounded : State
{
    // SuperState 없음.

    [Export] private Timer DashTimer { get; set; }

    public override void Enter()
    {
        StateMachine.AttachedToPlatform();

        DashTimer.Start();
        StateMachine.CooldownManager.StartCooling_Dash();

        Player.Animation.Play("Dash_Grounded");

        if (InputManager.Instance.Horizon == 0)
        {
            // 좌우 입력이 없다면, 바라보고 있는 방향으로 시전됨
            StateMachine.ActionDirection = StateMachine.PlayerFacingDirection;
        }
        else
        {
            if (InputManager.Instance.Horizon > 0)
            {
                StateMachine.PlayerFacingDirection = Char.LREnum.Right;
                StateMachine.ActionDirection = Char.LREnum.Right;
            }
            else if (InputManager.Instance.Horizon < 0)
            {
                StateMachine.PlayerFacingDirection = Char.LREnum.Left;
                StateMachine.ActionDirection = Char.LREnum.Left;
            }
        }

    }

    public override void Exit()
    {
        DashTimer.Stop();
    }

    private void DashFinished() // 대쉬가 끝난 시점의 상태 전환
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                if (InputManager.Instance.Horizon > 0 && StateMachine.ActionDirection == Char.LREnum.Left || InputManager.Instance.Horizon < 0 && StateMachine.ActionDirection == Char.LREnum.Right)
                {
                    StateMachine.TransState(State_Move.Sprint_Decel);
                    return;
                }
                else
                {
                    StateMachine.TransState(State_Move.Sprint_Grounded);
                    return;
                }

            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.CheckLedgeDirection();

            if (StateMachine.HoldingLedgeDirection == StateMachine.ActionDirection)
            {
                StateMachine.TransState(State_Move.Ledge_Climb);
                return;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.CheckWallDirection();

            if (StateMachine.HoldingWallDirection == StateMachine.ActionDirection)
            {
                StateMachine.TransState(State_Move.Wall_Hold);
                return;
            }
        }
        else
        {
            StateMachine.TransState(State_Move.Dash_Fall);
            return;
        }
    }

    private void _on_dash_grounded_timer_timeout()
    {
        DashFinished();
    }

    public override void HandlePhysics(double delta)
    {
        // ActionDirection을 근거로 이동 방향 결정
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.DashSpeed;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.DashSpeed;
        }

        velocity.Y = 0;

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        // 지상 대쉬 중 점프 시, 달리기 점프로 전환 -> 짧은 플랫폼에서 발생하는 문제 해결
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(State_Move.Sprint_Jump);
            return;
        }
    }
}
