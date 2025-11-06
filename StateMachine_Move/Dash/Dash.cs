using Godot;
using System;

public partial class Dash : SuperState
{
    [Export] private SubState Dash_Grounded { get; set; }
    [Export] private SubState Dash_Fall { get; set; }
    [Export] private SubState Dash_InAir { get; set; }

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
