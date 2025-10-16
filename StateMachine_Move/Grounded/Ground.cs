using Godot;
using System;

public partial class Ground : SuperState
{
    [Export] private SubState Idle { get; set; }
    [Export] private SubState Walk { get; set; }

    public override void HandleTransState(double delta)
    {
        // 다른 SuperState로 전환할 필요가 있는지 먼저 검사
        if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Input.IsActionJustPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
            return;
        }
        else if (Input.IsActionJustPressed(GamepadInput.RT) && StateMachine.CooldownManager.IsDashReady)
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Grounded);
            return;
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
