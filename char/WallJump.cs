using Godot;
using System;

public partial class WallJump : State
{
    [Export] private Timer WallJumpTimer { get; set; }
    [Export] private Timer WallApexTimer { get; set; }

    private bool isRising;

    public override void Enter()
    {
        FSM.InputManager.ConsumeJumpBuffer();

        isRising = true;

        FSM.ActionDirection = FSM.Player.GetWallNormal().X;

        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.WallJumpSpeed * FSM.ActionDirection;
        FSM.velocity.Y = FSM.JumpSpeed;

        FSM.Player.Velocity = FSM.velocity;

        WallJumpTimer.Start();
    }

    public override void Exit()
    {
        isRising = false;

        WallJumpTimer.Stop();
        WallApexTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (FSM.Player.IsOnWall())
        {
            FSM.TryTransState(StateName.WallSlipper);
        }
    }

    private void _on_wall_jump_timer_timeout()
    {
        StopRising();
    }

    private void StopRising()
    {
        if (isRising)
        {
            isRising = false;

            WallJumpTimer.Stop();
            WallApexTimer.Start();
        }
    }

    private void _on_wall_jump_apex_timer_timeout()
    {
        FSM.TryTransState(StateName.Fall);
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        if (isRising)
        {
            FSM.velocity.Y = Mathf.MoveToward(FSM.velocity.Y, FSM.MaxFallSpeed, (float)(FSM.JumpAccel * delta));
            FSM.velocity.X = Mathf.MoveToward(FSM.velocity.X, 0, (float)(FSM.WallJumpAccel * delta));
        }
        else
        {
            FSM.velocity.Y = Mathf.MoveToward(FSM.velocity.Y, FSM.MaxFallSpeed, (float)(FSM.FallAccel * delta));

            if (FSM.InputManager.InputDir.X == -FSM.ActionDirection)
            {
                FSM.velocity.X = Mathf.MoveToward(FSM.velocity.X, 0, (float)(FSM.WallJumpAccel_Opposite * delta));
            }
            else
            {
                FSM.velocity.X = Mathf.MoveToward(FSM.velocity.X, 0, (float)(FSM.WallJumpAccel * delta));
            }
        }

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.DashInAir);
        }
        else if (@event.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.DoubleJump);
        }
    }
}
