using Godot;
using System;

public partial class DashFall : SubState
{
    [Export] Timer DashFallTimer { get; set; }

    public override void Enter()
    {
        Player.Animation.Play("Dash_Fall");

        StateMachine.ActionDirection = StateMachine.PlayerFacingDirection;

        DashFallTimer.Start();
    }

    public override void Exit()
    {
        DashFallTimer.Stop();
    }

    private void _on_dash_fall_timer_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        return;
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


    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.DashInAirDelta * 1.6);
            }

        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.DashInAirDelta * 1.6);
            }
        }

        velocity.Y += (float)(delta * Player.Gravity);

        Player.Velocity = velocity;
    }
}
