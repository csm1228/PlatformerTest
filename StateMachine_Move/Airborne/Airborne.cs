using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Airborne : SuperState
{
    public override void HandleTransState(double delta)
    {
        // 착지 시점에 입력에 따라 상태 변경
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
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (InputManager.Instance.Horizon < 0)
        {
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
            velocity.X = -Player.WalkSpeed;
        }
        else if(InputManager.Instance.Horizon > 0)
        {
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
            velocity.X = Player.WalkSpeed;
        }
        else if (InputManager.Instance.Horizon == 0)
        {
            velocity.X = 0;
        }

        Player.Velocity = velocity;
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.RT)
        {
            // 공중 대쉬는, 대쉬 가능 여부만 검사
            if (StateMachine.CanDash)
            {
                StateMachine.FixActionDirection();
                StateMachine.TransState(State_Move.Dash_InAir);
                return;
            }
        }
    }
}
