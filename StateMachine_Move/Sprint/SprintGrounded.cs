using Godot;
using System;

public partial class SprintGrounded : SubState
{
    public override void Enter()
    {
        Player.Animation.Play("Sprint_Grounded");

        Vector2 velocity = Player.Velocity;

        if (InputManager.Instance.Horizon == 0)
        {
            // 좌우 입력이 없다면, 바라보고 있는 방향으로 시전됨
            StateMachine.ActionDirection = StateMachine.PlayerFacingDirection;
        }
        else
        {
            if (InputManager.Instance.Horizon > 0)
            {
                StateMachine.PlayerFacingDirection = Char.LREnum.Right;
                StateMachine.ActionDirection = Char.LREnum.Right;
            }
            else if (InputManager.Instance.Horizon < 0)
            {
                StateMachine.PlayerFacingDirection = Char.LREnum.Left;
                StateMachine.ActionDirection = Char.LREnum.Left;
            }
        }

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
        }

        Player.Velocity = velocity;
    }

    public override void HandleTransState(double delta)
    {
        if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
            return;
        }
        else if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Fall);
            return;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Left && InputManager.Instance.Horizon > 0)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
            return;

        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right && InputManager.Instance.Horizon < 0)
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
            return;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down && Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Jump);
            return;
        }
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.RT && Player.IsOnFloor())
        {
            if (InputManager.Instance.Horizon == 0)
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
                return;
            }

            else if (StateMachine.ActionDirection == Char.LREnum.Left && InputManager.Instance.Horizon < 0 || StateMachine.ActionDirection == Char.LREnum.Right && InputManager.Instance.Horizon > 0)
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
                return;

            }
        }
    }
}
