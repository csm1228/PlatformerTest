using Godot;
using System;

public partial class Char : CharacterBody2D
{
    [Export] public StateMachine_Move StateMachine_Move { get; set; }


    [Export] public RayCast2D RayCast_Ledge_Left { get; set; }
    [Export] public RayCast2D RayCast_Ledge_Right { get; set; }

    [Export] public RayCast2D RayCast_Wall_Left { get; set; }
    [Export] public RayCast2D RayCast_Wall_Right { get; set; }


}
