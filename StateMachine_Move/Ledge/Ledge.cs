using Godot;
using System;

public partial class Ledge : SuperState
{
    public override void HandleTransState(double delta)
    {

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
