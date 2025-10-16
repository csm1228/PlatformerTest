using Godot;
using System;

public partial class DashInAir : SubState
{
    Char.LREnum DashDirection;
    private float startPositon;

    public override void Enter()
    {
        DashDirection = Player.LastInputDirection;
        startPositon = Player.GlobalPosition.X;
        StateMachine.CooldownManager.StartCooling_Dash();
    }

    public override void HandleTransState(double delta)
    {
        float distanceMoved = Mathf.Abs(Player.GlobalPosition.X - startPositon);

        if (distanceMoved >= 200.0f)
        {
            GD.Print("지상 대쉬 이동거리만큼 이동했음");
            DashInAirFinished();
            return;
        }

    }

    private void DashInAirFinished()
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
                return;
            }
        }
        else if (!Player.IsOnFloor() && !Player.IsOnWall())
        {
            SuperState.TransSubState(State_Move.Dash_Fall);
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (DashDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.DashSpeed;
        }
        else if (DashDirection == Char.LREnum.Right)
        {
            velocity.X = Player.DashSpeed;
        }

        velocity.Y = 0;

        Player.Velocity = velocity;
    }
}
