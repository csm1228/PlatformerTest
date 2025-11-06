using Godot;
using System;

public partial class SprintDecel : SubState
{
    [Export] Timer SprintDecelTimer { get; set; }


    public override void Enter()
    {
        Player.Animation.Play("Sprint_Decel");

        SprintDecelTimer.Start();
    }

    public override void Exit()
    {
        SprintDecelTimer.Stop();
    }


    public override void HandleTransState(double delta)
    {
        if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        if (StateMachine.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Grounded, State_Move.Idle);
            return;
        }
    }

    private void _on_sprint_decel_timer_timeout()
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

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (StateMachine.ActionDirection == Char.LREnum.Left)
        {
            if (velocity.X < 0)
            {
                velocity.X += (float)(delta * Player.SprintDecelDelta);
            }
        }
        else if (StateMachine.ActionDirection == Char.LREnum.Right)
        {
            if (velocity.X > 0)
            {
                velocity.X -= (float)(delta * Player.SprintDecelDelta);
            }
        }
        Player.Velocity = velocity;

        if (InputManager.Instance.Horizon < 0)
        {
            StateMachine.ActionDirection = Char.LREnum.Right;
        }
        else if (InputManager.Instance.Horizon > 0)
        {
            StateMachine.ActionDirection = Char.LREnum.Left;
        }
    }

    public override void HandlePressedEvent(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
            return;
        }
        else if (action == GamepadInput.RT)
        {
            if (StateMachine.CooldownManager.IsDashReady)
            {
                StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Grounded);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
        }
    }
}
