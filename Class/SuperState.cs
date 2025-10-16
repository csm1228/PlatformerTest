using Godot;
using System;

public partial class SuperState : State
{
    public SubState CurrentSubState;

    public void TransSubState(string stateName)
    {
        SubState newState = GetNode<SubState>(stateName);
        if (newState == null || newState == CurrentSubState)
        {
            return;
        }

        CurrentSubState = newState;
    }
}
