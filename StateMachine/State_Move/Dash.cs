using Godot;
using System;

public partial class Dash : State
{
    [Export] private Timer DashTimer { get; set; }

    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.FixActionDirection();

        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.ActionDirection * FSM.DashSpeed;
        FSM.velocity.Y = 0.0f;

        FSM.Player.Velocity = FSM.velocity;

        DashTimer.Start();
    }

    public override void Exit()
    {
        DashTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (!FSM.Player.IsOnFloor())
        {
            FSM.TryTransState(StateName.Fall);
        }
    }

    private void _on_dash_timer_timeout()
    {
        if (Input.IsActionPressed(InputName.dash))
        {
            // 대쉬가 끝났는데 반대 방향으로 달리기 시도 시
            if (FSM.ActionDirection == -FSM.InputManager.InputDir.X)
            {
                FSM.TryTransState(StateName.Decel);
            }
            else
            {
                FSM.TryTransState(StateName.Sprint);
            }
        }
        else
        {
            FSM.TryTransState(StateName.Grounded);
        }
    }

    public override void PhysicsProcess(double delta)
    {

    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.SprintJump);
        }
    }
}
