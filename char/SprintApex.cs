using Godot;
using System;

public partial class SprintApex : SubState
{
    public override void HandleTransState(double delta)
    {
        if (Input.IsActionJustPressed(GamepadInput.RT))
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
            return;
        }
        else if (Player.IsOnCeiling())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
            return;
        }
        else if (Player.Velocity.Y >= 0)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
            return;
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

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y += 200;

        Player.Velocity = velocity;
    }
}
