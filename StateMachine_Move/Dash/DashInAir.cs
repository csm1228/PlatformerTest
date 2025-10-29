using Godot;
using System;

public partial class DashInAir : SubState
{
    [Export] private Timer DashTimer { get; set; }

    public override void Enter()
    {
        // 공중 대쉬는 대쉬 가능 여부만 검토
        StateMachine.CanDash = false;
        DashTimer.Start();
    }

    public override void Exit()
    {
        DashTimer.Stop();
    }

    private void DashInAirFinished()
    {
        if (!Player.IsOnFloor() && !Player.IsOnWall())
        {
            SuperState.TransSubState(State_Move.Dash_Fall);
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
}
