using Godot;
using System;

public partial class WallJump : SubState
{
    [Export] Timer MaxWallJumpTime { get; set; }

    public override void Enter()
    {
        Player.ConsumeJumpBuffer();

        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;

        MaxWallJumpTime.Start();

        Player.Animation.Play("Jump");

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            Player.Animation.FlipH = false;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            Player.Animation.FlipH = true;
        }
    }

    public override void Exit()
    {
        MaxWallJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }

    private void _on_max_wall_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = Player.WalkSpeed;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = -Player.WalkSpeed;
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
            return;
        }
    }
}
