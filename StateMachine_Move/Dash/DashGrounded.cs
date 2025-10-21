using Godot;
using System;

public partial class DashGrounded : SubState
{
    private float startPositon;

    public override void Enter()
    {
        startPositon = Player.GlobalPosition.X;

        StateMachine.CooldownManager.StartCooling_Dash();
    }

    public override void HandleTransState(double delta)
    {
        float distanceMoved = Mathf.Abs(Player.GlobalPosition.X - startPositon);

        if (distanceMoved >= 400.0f)
        {
            GD.Print("지상 대쉬 이동거리만큼 이동했음");
            DashFinished();
        }
    }


    private void DashFinished()
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
            }

            else if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
            }
        }
        else if (Player.IsOnWallOnly())
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
        }
        else if (!Player.IsOnFloor() && !Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Fall);
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.DashSpeed;
        }
        else if (Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.DashSpeed;
        }

        velocity.Y = 0;

        Player.Velocity = velocity;
    }
}
