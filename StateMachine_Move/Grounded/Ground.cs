using Godot;
using System;

public partial class Ground : SuperState
{
    [Export] private SubState Idle { get; set; }
    [Export] private SubState Walk { get; set; }

    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
    }

    public override void HandleTransState(double delta)
    {
        // 점프 버퍼 검사
        if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
            return;
        }
        else if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            // 지상 대쉬는 쿨타임만 검사
            if (StateMachine.CooldownManager.IsDashReady)
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Grounded);
                return;
            }
        }
        // 점프 입력 감지
        else if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
            return;
        }

        CurrentSubState.HandlePressedEvent(action);
    }
}
