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
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 newPosition;

        newPosition = Player.GlobalPosition + offset;

        GlobalPosition = newPosition;
    }

    public override void _Process(double delta)
    {
        if (Player.StateMachine_Move.CurrentState.Name == "Idle")
        {
            if (InputManager.Instance.Vertical < 0)
            {
                if (InputTime_Up > 0)
                {
                    InputTime_Up -= delta;
                    return;
                }
                else
                {
                    MoveCamera_Up();
                    return;
                }
            }
            else if (InputManager.Instance.Vertical > 0)
            {
                if (InputTime_Up > 0)
                {
                    InputTime_Up -= delta;
                    return;
                }
                else
                {
                    MoveCamera_Down();
                    return;
                }
            }
            else // 상하 입력 해제 시
            {
                InputTime_Up = 0.5;
                InputTime_Down = 0.5;
                InitializeCamera();
                return;
            }
        }
        else
        {
            InputTime_Up = 0.5;
            InputTime_Down = 0.5;
            InitializeCamera();
            return;
        }
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
