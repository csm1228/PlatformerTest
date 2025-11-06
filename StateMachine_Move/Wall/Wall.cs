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

        StateMachine.CheckWallDirection();
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
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
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
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Jump);
            return;
        }

        else if (InputManager.Instance.Horizon > 0 && StateMachine.HoldingWallDirection == Char.LREnum.Left)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }

        else if (InputManager.Instance.Horizon < 0 && StateMachine.HoldingWallDirection == Char.LREnum.Right)
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
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Wall_Airborne, State_Move.Wall_Jump);
            return;
        }

        CurrentSubState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        CurrentSubState.HandleReleasedEvent(action);
    }
}
