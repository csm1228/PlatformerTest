using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export] Char Player { get; set; }

    private Vector2 offset;
    private Vector2 Base = new Vector2(0, -200);
    private double InputTime_Up = 0.5;
    private double InputTime_Down = 0.5;

    public override void _Ready()
    {
        offset = Player.GlobalPosition + Base;
        GlobalPosition = offset;
    }

    public override void _PhysicsProcess(double delta)
    {

    }

    public override void _Process(double delta)
    {
        offset = Player.GlobalPosition + Base;
        GlobalPosition = offset;
    }

    private void MoveCamera_Up()
    {
        offset = offset.Lerp(new Vector2(0, -300) + Base, 0.04f);
    }

    private void MoveCamera_Down()
    {
        offset = offset.Lerp(new Vector2(0, 1000) + Base, 0.04f);
    }

    private void InitializeCamera()
    {
        offset = offset.Lerp(Base, 0.12f);
    }
}
