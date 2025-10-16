using Godot;
using System;

public partial class DashFall : SubState
{
    public override void HandleTransState(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
            }
        }
        else if (velocity.X <= Player.WalkSpeed && velocity.X >= -Player.WalkSpeed)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        }
    }


    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;
        Vector2 Fall = new Vector2 ( 0, Player.Gravity );

        velocity = velocity.Lerp(Fall, 0.2f);

        Player.Velocity = velocity;
    }
}
