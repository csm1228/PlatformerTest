using Godot;
using System;

public partial class SprintFall : SubState
{
    [Export] private float Decel { get; set; }
    [Export] private float Decel_OppositeInput { get; set; }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
                return;
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
            return;
        }
        else if (Math.Abs(Player.Velocity.X) <= Player.WalkSpeed)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (velocity.Y < Player.MaxFallSpeed)
        {
            velocity.Y += (float)(Player.MaxFallSpeed * delta * Player.GravityCoefficient_Fall);
        }
        else
        {
            velocity.Y = Player.MaxFallSpeed;
        }
        if (InputManager.Instance.Horizon > 0 && Player.ActionDirection == Char.LREnum.Left)
        {
            velocity.X += (float)(Decel_OppositeInput * delta);

        }
        if (InputManager.Instance.Horizon < 0 && Player.ActionDirection == Char.LREnum.Right)
        {
            velocity.X -= (float)(Decel_OppositeInput * delta);
        }
        else
        {
            if (Player.ActionDirection == Char.LREnum.Left)
            {
                velocity.X += (float)(Decel * delta);
            }
            else if (Player.ActionDirection == Char.LREnum.Right)
            {
                velocity.X -= (float)(Decel * delta);
            }
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            if (StateMachine.CanDash && StateMachine.CooldownManager.IsDashReady)
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
                return;
            }
        }
    }

    public override void HandleReleasedEvent(StringName action)
    {
        
    }
}
