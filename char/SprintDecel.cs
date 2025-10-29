using Godot;
using System;

public partial class SprintDecel : SubState
{
    public override void HandleTransState(double delta)
    {
        if (!Player.IsOnFloor())
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        else if (Math.Abs(Player.Velocity.X) <= Player.WalkSpeed)
        {
            StateMachine.TransToWalkOrIdle();
            return;
        }
        else if (Player.LastInputDirection == Player.ActionDirection)
        {
            if (Input.IsActionJustPressed(GamepadInput.Face_Down))
            {
                StateMachine.TransState(SuperState_Move.Airborne, State_Move.Jump);
                return;
            }
            else if (Input.IsActionPressed(GamepadInput.RT))
            {
                if (Input.IsActionJustPressed(GamepadInput.Face_Down))
                {
                    StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Jump);
                    return;
                }
                else
                {
                    StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Grounded);
                    return;
                }
            }
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity = velocity.Lerp(Vector2.Zero, 0.04f);

        Player.Velocity = velocity;
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
            StateMachine.FixActionDirection();
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_Grounded);
            return;
        }
    }

    public override void HandleReleasedEvent(StringName action)
    {
        
    }
}
