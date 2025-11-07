using Godot;
using System;

public partial class State : SuperState
{
    [Export] public SuperState SuperState { get; private set; } = null;
    [Export] public AnimatedSprite2D Sprite { get; set; }

    public void ConnectEventSignal()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
        InputManager.Instance.ActionReleased += HandleReleasedEvent;
    }

    public void DisconnectEventSignal()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
        InputManager.Instance.ActionReleased -= HandleReleasedEvent;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
}
