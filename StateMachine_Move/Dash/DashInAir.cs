using Godot;
using System;

public partial class DashInAir : SubState
{
    [Export] private Timer DashTimer { get; set; }

    public override void Enter()
    {
        // 공중 대쉬는 대쉬 가능 여부만 검토
        StateMachine.CanDash = false;

        Player.Animation.Play("Dash_InAir");

        Vector2 velocity = Player.Velocity;

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

        // Enter에서 ActionDirection을 결정하지 않음.
        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.DashSpeed;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.DashSpeed;
        }

        velocity.Y = Player.DashInAirRise;

        Player.Velocity = velocity;

        DashTimer.Start();
    }

    public override void Exit()
    {
        DashTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
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
                StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                return;
            }
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.CheckWallDirection();

            if (StateMachine.HoldingWallDirection == StateMachine.ActionDirection)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
                return;
            }
        }
    }

    private void DashInAirFinished()
    {
        if (!Player.IsOnFloor() && !Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Fall);
            return;
        }
        else if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }
    }

    private void _on_dash_in_air_timer_timeout()
    {
        DashInAirFinished();
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.DashInAirDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.DashInAirDelta);
            }
        }

        velocity.Y += (float)(delta * Player.Gravity);

        Player.Velocity = velocity;
    }
}
