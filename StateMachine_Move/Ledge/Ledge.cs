using Godot;
using System;

public partial class Ledge : SuperState
{
    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
    }

    public override void HandleTransState(double delta)
    {

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }

    public override void HandlePressedEvent(StringName action)
    {
        CurrentSubState.HandlePressedEvent(action);
    }
}
