using Godot;
using System;

public partial class SprintJump : SubState
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        Player.ConsumeJumpBuffer();

        MaxJumpTime.Start();

        Player.Animation.Play("Sprint_Jump");

        Vector2 velocity = Player.Velocity;

        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
            Player.Animation.FlipH = true;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
            Player.Animation.FlipH = false;
        }

        velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;

        
    }

    public override void Exit()
    {
        MaxJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            if (!Input.IsActionPressed(GamepadInput.RT) || Player.ActionDirection != Player.LastInputDirection)
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
                return;
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
            return;
        }
        else if (Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
            return;
        }
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            if (StateMachine.CanDash && StateMachine.CooldownManager.IsDashReady)
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
                return;
            }
        }
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Down)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
            return;
        }
    }
}
