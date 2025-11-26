using Godot;
using System;

public partial class TerrainDetector : Node2D
{
    [Export] private RayCast2D Wall_Right { get; set; }
    [Export] private RayCast2D Wall_Left { get; set; }

    [Export] private RayCast2D Ledge_Right { get; set; }
    [Export] private RayCast2D Ledge_Left { get; set; }

    public float GetWallDirection()
    {
        if (!Wall_Right.IsColliding() && Wall_Left.IsColliding())
        {
            return -1;
        }
        else if (Wall_Right.IsColliding() && !Wall_Left.IsColliding())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public float GetLedgeDirection()
    {
        if (!Ledge_Right.IsColliding() && Ledge_Left.IsColliding())
        {
            return -1;
        }
        else if (Ledge_Right.IsColliding() && !Ledge_Left.IsColliding())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
