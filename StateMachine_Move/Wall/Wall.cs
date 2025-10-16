using Godot;
using System;

public partial class Wall : SuperState
{
    [Export] private SubState Wall_Hold { get; set; }
    [Export] private SubState Wall_Slipper { get; set; }

    [Export] private SubState Wall_Climb { get; set; }
    public override void HandleTransState(double delta)
    {
        // 다른 SuperState로 전환할 필요가 있는지 먼저 검사
        if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Walk);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
                return;
            }
        }
        else if (Input.IsActionJustPressed(GamepadInput.Joypad_Down))
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
            return;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            if (Input.IsActionPressed(GamepadInput.Right))
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
            else if (!Player.RayCast_Upper_Left.IsColliding())
            {
                if (Player.RayCast_Lower_Left.IsColliding())
                {
                    StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                    return;
                }
                else
                {
                    StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                    return;
                }

            }
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            if (Input.IsActionPressed(GamepadInput.Left))
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
            else if (!Player.RayCast_Upper_Right.IsColliding())
            {
                if (Player.RayCast_Lower_Right.IsColliding())
                {
                    StateMachine.TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
                    return;
                }
                else
                {
                    StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                    return;
                }
            }
        }

        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
