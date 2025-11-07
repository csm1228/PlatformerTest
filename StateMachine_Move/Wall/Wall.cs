using Godot;
using System;

public partial class Wall : SuperState
{
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
