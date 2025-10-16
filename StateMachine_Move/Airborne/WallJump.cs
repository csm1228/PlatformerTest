using Godot;
using System;

public partial class WallJump : SubState
{
    [Export] Timer MaxWallJumpTime { get; set; }

    public override void Enter()
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;

        MaxWallJumpTime.Start();
    }

    public override void Exit()
    {
        MaxWallJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (!Input.IsActionPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
        }
        else if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
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

        velocity.Y -= (float)(Player.JumpDelta * delta);

        Player.Velocity = velocity;
    }
}
