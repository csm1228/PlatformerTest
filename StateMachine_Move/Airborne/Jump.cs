using Godot;
using System;

public partial class Jump : SubState
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        MaxJumpTime.Start();
    }

    public override void Exit()
    {
        MaxJumpTime.Stop();
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

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Airborne, State_Move.Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Input.IsActionPressed(GamepadInput.Left))
        {
            velocity.X = -Player.WalkSpeed;
        }
        else if (Input.IsActionPressed(GamepadInput.Right))
        {
            velocity.X = Player.WalkSpeed;
        }
        else
        {
            velocity.X = 0;
        }

            velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;
    }
}
