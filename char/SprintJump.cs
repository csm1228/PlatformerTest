using Godot;
using System;

public partial class SprintJump : SubState
{
    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        MaxJumpTime.Start();

        Vector2 velocity = Player.Velocity;



        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
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
        if (!Input.IsActionPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
        }
        else if (Player.IsOnCeiling())
        {
            if (!Input.IsActionPressed(GamepadInput.RT) || Player.ActionDirection != Player.LastInputDirection)
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
        }
        else if (Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
        }
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y -= (float)(Player.JumpDelta * delta);

        Player.Velocity = velocity;
    }

}
