using Godot;
using System;

public partial class Ground : SuperState
{
    public override void HandleTransState(double delta)
    {
        // 점프 버퍼 검사
        if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(State_Move.Jump);
            return;
        }
        else if (!Player.IsOnFloor())
        {
            StateMachine.TransState(State_Move.Fall);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            // 지상 대쉬는 쿨타임만 검사
            if (StateMachine.CooldownManager.IsDashReady)
            {
                StateMachine.TransState(State_Move.Dash_Grounded);
                return;
            }
            // 아직 대쉬가 쿨타임이라면, 스프린트로 전환
            else
            {
                StateMachine.TransState(State_Move.Sprint_Grounded);
                return;
            }
        }
        // 점프 입력 감지
        else if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(State_Move.Jump);
            return;
        }
    }
}
