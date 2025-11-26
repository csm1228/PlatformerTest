using Godot;
using System;

public partial class DoubleJump : State
{
    [Export] private Timer DoubleJumpTimer { get; set; }

    public override void Enter()
    {
        FSM.CanDoubleJump = false;

        FSM.InputManager.ConsumeJumpBuffer();

        // 최초 점프 속도로 변경
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.Y = FSM.JumpSpeed;

        FSM.Player.Velocity = FSM.velocity;

        DoubleJumpTimer.Start();
    }

    public override void Exit()
    {
        DoubleJumpTimer.Stop();
    }

    private void _on_double_jump_timer_timeout()
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

        FSM.velocity.X = FSM.WalkSpeed * FSM.InputManager.InputDir.X;
        FSM.velocity.Y += (float)(FSM.JumpAccel * delta);

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void HandleInputEvent(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.dash))
        {
            FSM.TryTransState(StateName.DashInAir);
        }
        else if (@event.IsActionReleased(InputName.jump))
        {
            FSM.TryTransState(StateName.Fall);
        }
    }
}
