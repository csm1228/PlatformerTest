using Godot;
using System;

public partial class DebugLabels : Control
{
    [Export] Char Player { get; set; }
    [Export] StateMachine_Move StateMachine_Move { get; set; }


    [Export] Label DebugLabel_LastInputDir { get; set; }
    [Export] Label DebugLabel_LastWallDir { get; set; }
    [Export] Label DebugLabel_State { get; set; }
    [Export] Label DebugLabel_IsOnFloor { get; set; }
    [Export] Label DebugLabel_IsOnWall { get; set; }


    [Export] Label DebugLabel_UpperRayCast_Right { get; set; }
    [Export] Label DebugLabel_UpperRayCast_Left { get; set; }
    [Export] Label DebugLabel_LowerRayCast_Right { get; set; }
    [Export] Label DebugLabel_LowerRayCast_Left { get; set; }

    [Export] Label DebugLabel_Velocity_X { get; set; }
    [Export] Label DebugLabel_Velocity_Y { get; set; }

    public override void _Process(double delta)
    {

        DebugLabel_State.Text = StateMachine_Move.CurrentMoveSuperState?.CurrentSubState?.Name;
        DebugLabel_IsOnFloor.Text = "IsOnFloor : " + Player.IsOnFloor().ToString();
        DebugLabel_IsOnWall.Text = "IsOnWall : " + Player.IsOnWall().ToString();

        DebugLabel_UpperRayCast_Right.Text = Player.RayCast_Ledge_Right.IsColliding().ToString();
        DebugLabel_UpperRayCast_Left.Text = Player.RayCast_Ledge_Left.IsColliding().ToString();
        DebugLabel_LowerRayCast_Right.Text = Player.RayCast_Wall_Right.IsColliding().ToString();
        DebugLabel_LowerRayCast_Left.Text = Player.RayCast_Wall_Left.IsColliding().ToString();

        DebugLabel_Velocity_X.Text = "X : " + Player.Velocity.X.ToString();
        DebugLabel_Velocity_Y.Text = "X : " + Player.Velocity.Y.ToString();
    }
}
