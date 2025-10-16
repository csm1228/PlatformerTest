using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class WallHold : SubState
{
    private bool HoldingWall = true;

    [Export] Timer WallHoldTimer { get; set; }

    public override void Enter()
    {
        HoldingWall = true;

        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
        }

        Player.Velocity = velocity;

        WallHoldTimer.Start();
    }

    public override void Exit()
    {
        WallHoldTimer.Stop();
    }

    public override void HandleTransState(double delta)
    {
        if (Input.IsActionPressed(GamepadInput.Up))
        {
            StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Climb);
            return;
        }
        else if (!HoldingWall)
        {
            bool isHoldingInput = (Player.LastHoldingWallDirection == Char.LREnum.Left && Input.IsActionPressed(GamepadInput.Left) || Player.LastHoldingWallDirection == Char.LREnum.Right && Input.IsActionPressed(GamepadInput.Right));

            if(!isHoldingInput)
            {
                StateMachine.TransState(SuperState_Move.Wall, State_Move.Wall_Slipper);
                return;
            }
        }
    }

    public override void HandlePhysics(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.Y = 0;

        if (Player.LastHoldingWallDirection == Char.LREnum.Left)
        {
            velocity.X = -1;
        }
        else if (Player.LastHoldingWallDirection == Char.LREnum.Right)
        {
            velocity.X = 1;
        }

        Player.Velocity = velocity;
    }

    private void _on_wall_hold_timer_timeout()
    {
        HoldingWall = false;
    }
}
