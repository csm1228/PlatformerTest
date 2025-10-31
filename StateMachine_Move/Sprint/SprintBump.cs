using Godot;
using System;

public partial class SprintBump : SubState
{
    [Export] private Vector2I StartVector { get; set; }
    [Export] private Vector2I TargetVector { get; set; }
    [Export] private float LerfCoefficient { get; set; }

    [Export] private Timer BumpTimer { get; set; }

    public override void Enter()
    {
        Player.Animation.Play("Sprint_Bump");

        Vector2 velocity = StartVector;

        if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = - velocity.X;
        }

        Player.Velocity = velocity;

        BumpTimer.Start();
    }

    public override void HandleTransState(double delta)
    {

    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity = velocity.Lerp(TargetVector, LerfCoefficient);

        Player.Velocity = velocity;
    }

    private void _on_bump_timer_timeout()
    {
        if (Player.IsOnFloor())
        {
            StateMachine.TransToWalkOrIdle();
            return;
        }
        else
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }


    }
}
