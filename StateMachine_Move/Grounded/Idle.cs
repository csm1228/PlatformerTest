using Godot;
using System;

public partial class Idle : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Idle");
    }

    public override void HandleTransState(double delta)
    {
        if (InputManager.Instance.Horizon != 0)
        {
            StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
            return;
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.X = 0;

        Player.Velocity = velocity;

        Player.Animation.FlipH = (Player.LastInputDirection == Char.LREnum.Left);
    }
}
