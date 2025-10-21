using Godot;
using System;

public partial class SprintFall : SubState
{
    public override void HandleTransState(double delta)
    {
        if (StateMachine.inputManager.IsDashOnBuffer() && StateMachine.CooldownManager.IsDashReady)
        {
            StateMachine.TransState(SuperState_Move.Dash, State_Move.Dash_InAir);
            return;
        }
        else if (Player.IsOnFloor())
        {
            if (Input.IsActionPressed(GamepadInput.RT))
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Grounded);
                return;
            }
            else
            {
                StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Decel);
                return;
            }
        }
        else if (Player.IsOnWall())
        {
            StateMachine.TransState(SuperState_Move.Sprint, State_Move.Sprint_Bump);
            return;
        }
        else if (Math.Abs(Player.Velocity.X) <= Player.WalkSpeed)
        {
            StateMachine.TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
        
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        if (velocity.Y < Player.Gravity)
        {
            velocity.Y += (float)(7000 * delta);
        }
        else
        {
            velocity.Y = Player.Gravity;
        }

        if (StateMachine.IsOppositeInput())
        {
            if (Player.ActionDirection == Char.LREnum.Left)
            {
                velocity.X += (float)(2000 * delta);
            }
            else if (Player.ActionDirection == Char.LREnum.Right)
            {
                velocity.X -= (float)(2000 * delta);
            }
        }
        else
        {
            if (Player.ActionDirection == Char.LREnum.Left)
            {
                velocity.X += (float)(1000 * delta);
            }
            else if (Player.ActionDirection == Char.LREnum.Right)
            {
                velocity.X -= (float)(1000 * delta);
            }
        }

            Player.Velocity = velocity;
    }
}
