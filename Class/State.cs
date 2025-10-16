using Godot;
using System;

public partial class State : Node
{
    [Export] public StateMachine_Move StateMachine { get; set; }
    [Export] public Char Player { get; set; }
    [Export] public AnimatedSprite2D Sprite { get; set; }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleTransState(double delta) { }
    public virtual void HandlePhysics(double delta) { }
}
