using Godot;
using System;

public partial class DashGrounded : SubState
{
    [Export] private Timer DashTimer { get; set; }

    public override void Enter()
    {
        DashTimer.Start();
        StateMachine.CooldownManager.StartCooling_Dash();
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
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }
        else if (!Player.IsOnFloor() && !Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Fall);
            return;
        }
    }

    private void _on_dash_grounded_timer_timeout()
    {
        DashFinished();
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.DashSpeed;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.DashSpeed;
        }

        velocity.Y = 0;

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Jump);
            return;
        }
    }
}
