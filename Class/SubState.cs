using Godot;
using System;

public partial class SubState : State
{
    [Export] public SuperState SuperState { get; set; }
}
