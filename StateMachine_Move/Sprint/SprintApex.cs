using Godot;
using System;

public partial class SprintApex : SubState
{
    [Export] Timer SprintApexTimer { get; set; }

    public override void Enter()
    {
        SprintApexTimer.Start();
    }

    public override void Exit()
    {
        SprintApexTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
            return;
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Climb);
            return;
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
            return;
        }
    }

    private void _on_sprint_apex_timer_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        return;
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintApexDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintApexDelta);
            }
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Apex);

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            if (StateMachine.CanDash)
            {
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
                return;
            }
        }
    }
}
