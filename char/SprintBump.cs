using Godot;
using System;

public partial class SprintBump : SubState
{
    public override void Enter()
    {
        Vector2 velocity = Player.Velocity;

        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = Player.WalkSpeed;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = -Player.WalkSpeed;
        }

            velocity.Y = -400;

        Player.Velocity = velocity;
    }

    public override void HandleTransState(double delta)
    {
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
        else if (Player.Velocity.X < (Player.WalkSpeed / 2) && Player.Velocity.X > (-Player.WalkSpeed / 2))
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        Vector2 Fall = new Vector2(0, Player.Gravity);

        GD.Print("!");
        velocity = velocity.Lerp(Fall, 0.05f);

        Player.Velocity = velocity;
    }
}
