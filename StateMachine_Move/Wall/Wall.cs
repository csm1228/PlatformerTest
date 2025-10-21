using Godot;
using System;

public partial class Wall : SuperState
{
    [Export] private SubState Wall_Hold { get; set; }
    [Export] private SubState Wall_Slipper { get; set; }

    [Export] private SubState Wall_Climb { get; set; }
    public override void HandleTransState(double delta)
    {
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

        else if (!StateMachine.IsOnWall())
        {
            if (StateMachine.IsOnLedge())
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

        // 다른 SuperState로 전환할 필요가 있는지 먼저 검사
        else if (StateMachine.inputManager.IsJumpOnBuffer())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
            return;
        }
        else if (Input.IsActionPressed(GamepadInput.Right))
        {
            if (Player.LastHoldingWallDirection == Char.LREnum.Left)
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
        }
        else if (Input.IsActionPressed(GamepadInput.Left))
        {
            if (Player.LastHoldingWallDirection == Char.LREnum.Right)
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
                return;
            }
        }


        CurrentSubState.HandleTransState(delta);
    }

    public override void HandlePhysics(double delta)
    {
        CurrentSubState.HandlePhysics(delta);
    }
}
