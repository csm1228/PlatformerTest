using Godot;
using System;

public partial class Wall : SuperState
{
    [Export] private SubState Wall_Hold { get; set; }
    [Export] private SubState Wall_Slipper { get; set; }

    [Export] private SubState Wall_Climb { get; set; }

    public override void Enter()
    {
        InputManager.Instance.ActionPressed += HandlePressedEvent;
        InputManager.Instance.ActionReleased += HandleReleasedEvent;
    }

    public override void Exit()
    {
        InputManager.Instance.ActionPressed -= HandlePressedEvent;
        InputManager.Instance.ActionReleased -= HandleReleasedEvent;
    }




    public override void HandleTransState(double delta)
    {
        if (Player.IsOnFloor())
        {
            if (InputManager.Instance.Horizon != 0)
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

        else if (Player.JumpBuffer > 0)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Wall_Jump);
            return;
        }

        else if (InputManager.Instance.Horizon > 0 && Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }

        else if (InputManager.Instance.Horizon < 0 && Player.LastHoldingWallDirection == Char.LREnum.Right)
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
        CurrentSubState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        CurrentSubState.HandleReleasedEvent(action);
    }
}
