using Godot;
using System;

public partial class Swim : State
{
    public override void Enter()
    {
        FSM.RestoreAirAction();


        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity.Y = FSM.JumpSpeed;

        FSM.Player.Velocity = FSM.velocity;
    }

    public override void Exit()
    {

    }

    public override void HandleTransState(double delta)
    {

    }

    public override void PhysicsProcess(double delta)
    {
        FSM.velocity = FSM.Player.Velocity;

        FSM.velocity = FSM.InputManager.InputDir_Normalized * FSM.SwimSpeed;

        FSM.Player.Velocity = FSM.velocity;
    }
}
