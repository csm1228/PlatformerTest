using Godot;
using System;

public partial class SprintJump : State
{
    [Export] private Timer SprintJumpTimer { get; set; }

    public override void Enter()
    {
        FSM.FixActionDirection();

        // 최초 점프 속도로 변경
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = FSM.SprintSpeed * FSM.ActionDirection;
        FSM.velocity.Y = FSM.JumpSpeed;

        FSM.Player.Velocity = FSM.velocity;

        SprintJumpTimer.Start();
    }

    public override void Exit()
    {
        SprintJumpTimer.Stop();
    }

    private void _on_sprint_jump_timer_timeout()
    {
        FSM.TryTransState(StateName.Fall);
    }


    public override void HandleTransState(double delta)
    {
        if (FSM.Player.IsOnFloor() && FSM.Player.Velocity.Y >= 0)
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
        else if (FSM.Player.IsOnCeiling())
        {
            FSM.TryTransState(StateName.Fall);
        }
        else if (!Input.IsActionPressed(InputName.jump))
        {
            FSM.TryTransState(StateName.Fall);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.X = Mathf.MoveToward(FSM.velocity.X, 0.0f, (float)(FSM.SprintJumpAccel * delta));
        FSM.velocity.Y += (float)(FSM.JumpAccel * delta);

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
        else if (@event.IsActionReleased(InputName.jump))
        {
            FSM.TryTransState(StateName.Fall);
        }
    }
}
