using Godot;
using System;

public partial class SuperState : Node
{
    [Export] public StateMachine_Move StateMachine { get; private set; }
    [Export] public Char Player { get; private set; }

    public virtual void HandleTransState(double delta) { }
    public virtual void HandlePhysics(double delta) { }
    public virtual void HandlePressedEvent(StringName action) { }
    public virtual void HandleReleasedEvent(StringName action) { }
}
