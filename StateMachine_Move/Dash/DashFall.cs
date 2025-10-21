using Godot;
using System;

public partial class DashFall : SubState
{
    public override void HandleTransState(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
            }

            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }

        }
        else if (velocity.X <= Player.WalkSpeed && velocity.X >= -Player.WalkSpeed)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
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
