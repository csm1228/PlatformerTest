using Godot;
using System;

public partial class SprintJump : State
{
    // SuperState : Airborne - 공중에 떠 있는 상태.
    // HandleTranState, HandlePressEvent 호출
    // 땅에 닿으면 Sprint_Grounded/Walk/Idle로 전환, RT 입력 시 공중 대쉬
    // HandlePhysics 호출 X. 자체 물리 사용

    [Export] Timer MaxJumpTime { get; set; }

    public override void Enter()
    {
        MaxJumpTime.Start();

        Player.Animation.Play("Sprint_Jump");

        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            velocity.X = -Player.SprintSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Left;
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            velocity.X = Player.SprintSpeed;
            StateMachine.PlayerFacingDirection = Char.LREnum.Right;
        }

        velocity.Y = Player.JumpSpeed;

        Player.Velocity = velocity;
    }

    public override void Exit()
    {
        MaxJumpTime.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Player.IsOnCeiling())
        {
            StateMachine.TransState(State_Move.Fall);
            return;
        }
        else if (Player.IsOnFloor())
        {
            StateMachine.TransState(State_Move.Sprint_Grounded);
            return;
        }
        else if (StateMachine.IsOnLedge())
        {
            StateMachine.TransState(State_Move.Ledge_Climb);
            return;
        }
        else if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(State_Move.Wall_Hold);
            return;
        }
        else if (!Input.IsActionPressed(GamepadInput.Face_Down))
        {
            StateMachine.TransState(State_Move.Sprint_Apex);
            return;
        }

        SuperState.HandleTransState(delta);
    }

    private void _on_max_jump_time_timeout()
    {
        StateMachine.TransState(State_Move.Sprint_Apex);
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintJumpDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintJumpDelta);
            }
        }

        velocity.Y += (float)(Player.Gravity * delta * Player.GravityCoefficient_Jump);

        Player.Velocity = velocity;

        // SuperState(Airborne)의 물리를 반영하지 않음
    }

    public override void HandlePressedEvent(StringName action)
    {
        SuperState.HandlePressedEvent(action);
    }

    public override void HandleReleasedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(State_Move.Sprint_Apex);
            return;
        }
    }
}
