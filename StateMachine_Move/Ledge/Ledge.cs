using Godot;
using System;

public partial class Ledge : SuperState
{
    public override void HandleTransState(double delta)
    {
        if (Input.IsActionJustPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
            return;
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
