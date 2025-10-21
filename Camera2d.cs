using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export] Char Player { get; set; }

    private Vector2 offset;
    private Vector2 Base = new Vector2(-960, -540);
    private double InputTime = 0.5;

    public override void _Ready()
    {
        offset = Base;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 newPosition = Player.GlobalPosition + offset;

        GlobalPosition = newPosition;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed(GamepadInput.Up) || Input.IsActionPressed(GamepadInput.Down))
        {
            if (Player.StateMachine_Move.CurrentMoveSubState.Name == "Idle")
            {
                InputTime -= delta;
            }

        }
        else
        {
            InputTime = 0.5;
            InitializeCamera();
        }

        if (InputTime < 0)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        if (Input.IsActionPressed(GamepadInput.Up))
        {
            GD.Print("!");
            offset = offset.Lerp(new Vector2(0, -500) + Base, 0.08f);
        }
        else if (Input.IsActionPressed(GamepadInput.Down))
        {
            GD.Print("!");
            offset = offset.Lerp(new Vector2(0, 500) + Base, 0.08f);
        }
    }

    private void InitializeCamera()
    {
        offset = offset.Lerp(Base, 0.12f);
    }

    private void _on_timer_timeout()
    {
        MoveCamera();
    }
}
