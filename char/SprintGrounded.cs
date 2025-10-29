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
            return;
        }
        else if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
            return;
        }
        else if (Player.ActionDirection != Player.LastInputDirection)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down && Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Jump);
            return;
        }
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.RT && Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
            return;
        }
    }
}
