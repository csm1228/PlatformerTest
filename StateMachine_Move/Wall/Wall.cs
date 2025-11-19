using Godot;
using System;

public partial class Wall : SuperState
{
    // SuperState : Wall - 벽에 붙어있는 상태.
    // 땅에 닿았을 때 지상 상태로 전환, 벽에 사라지거나 벽 반대 방향으로 입력 시 Fall 상태로 전환, 점프 버퍼가 있거나 점프 입력 시 WallJump로 전환한다.


    public override void HandleTransState(double delta)
    {
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransToWalkOrIdle();
                return;
            }
        }

        else if (!StateMachine.IsOnWall())
        {
            if (StateMachine.IsOnLedge())
            {
                StateMachine.TransState(State_Move.Ledge_Climb);
                return;
            }
            else
            {
                StateMachine.TransState(State_Move.Fall);
                return;
            }
        }

        else if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(State_Move.Wall_Jump);
            return;
        }

        else if (InputManager.Instance.Horizon > 0 && StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(State_Move.Fall);
            return;
        }

        else if (InputManager.Instance.Horizon < 0 && StateMachine.HoldingWallDirection == Char.LREnum.Right)
        {
            StateMachine.TransState(State_Move.Fall);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(State_Move.Wall_Jump);
            return;
        }
    }
}
