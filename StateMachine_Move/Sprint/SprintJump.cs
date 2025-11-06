using Godot;
using System;

public partial class SprintJump : SubState
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        MaxJumpTime.Start();

        Player.Animation.Play("Sprint_Jump");

        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
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
        else if (!Input.IsActionPressed(GamepadInput.Face_Down))
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
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

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintJumpDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintJumpDelta);
            }
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

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

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
            return;
        }
    }
}
