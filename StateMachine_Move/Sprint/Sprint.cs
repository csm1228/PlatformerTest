using Godot;
using System;

public partial class Sprint : SuperState
{
    public Char.LREnum SprintDirection;

    [Export] private SubState Sprint_Grounded { get; set; }
    [Export] private SubState Sprint_Jump { get; set; }
    [Export] private SubState Sprint_Fall { get; set; }
    [Export] private SubState Sprint_Decel { get; set; }
    [Export] private SubState Sprint_Bump { get; set; }

    [Export] private SubState Sprint_Apex { get; set; }

    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
        InputManager.Instance.ActionReleased += HandleReleasedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
        InputManager.Instance.ActionReleased -= HandleReleasedEvent;
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

    public override void HandleReleasedEvent(StringName action)
    {
        CurrentSubState.HandleReleasedEvent(action);
    }
}
