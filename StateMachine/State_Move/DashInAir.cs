using Godot;
using System;

public partial class DashInAir : State
{
    [Export] private Timer DashInAirTimer { get; set; }

    public override void Enter()
    {
        FSM.CanDashInAir = false;


        FSM.FixActionDirection();

        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.DashInAirSpeed_X * FSM.ActionDirection;
        FSM.velocity.Y = FSM.DashInAirSpeed_Y;

        FSM.Player.Velocity = FSM.velocity;

        DashInAirTimer.Start();
    }

    public override void Exit()
    {
        DashInAirTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {

    }

    private void _on_dash_in_air_timer_timeout()
    {
        FSM.TryTransState(StateName.Fall);
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = Mathf.MoveToward(FSM.velocity.X, 0, (float)(FSM.DashInAirDecel_X * delta));
        FSM.velocity.Y = Mathf.MoveToward(FSM.velocity.Y, FSM.MaxFallSpeed, (float)(FSM.DashInAirDecel_Y * delta));

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {

    }
}
