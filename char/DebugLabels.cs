using Godot;
using System;

public partial class DebugLabels : Control
{
    [Export] Char Player { get; set; }
    [Export] StateMachine_Move StateMachine_Move { get; set; }

    [Export] Label DebugLabel_State { get; set; }




    public override void _Process(double delta)
    {
        DebugLabel_State.Text = StateMachine_Move.CurrentState.Name.ToString();
    }
}
