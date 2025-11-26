using Godot;
using System;

public partial class Decel : State
{
    [Export] Timer DecelTimer { get; set; }

    public override void Enter()
    {
        FSM.RestoreAirAction();

        FSM.FixActionDirection();

        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = -FSM.DecelSpeed * FSM.ActionDirection;

        FSM.Player.Velocity = FSM.velocity;

        DecelTimer.Start();
    }

    public override void Exit()
    {
        DecelTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {

    }

    private void _on_decel_timer_timeout()
    {
        if (Input.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.Sprint);
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

    }
}
