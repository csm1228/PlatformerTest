using Godot;
using System;

public partial class SprintGrounded : SubState
{
    public override void Enter()
    {
        Vector2 velocity = Player.Velocity;

        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
        }

        Player.Velocity = velocity;
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
        }
        else if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
        }
        else if (!Input.IsActionPressed(GamepadInput.RT) || Player.ActionDirection != Player.LastInputDirection)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
        }
        else if (Input.IsActionJustPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Jump);
        }
    }


    public override void HandlePhysics(double delta)
    {

    }
}
