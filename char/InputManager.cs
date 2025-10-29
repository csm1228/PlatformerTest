using Godot;
using System;
using System.Collections.Generic;

public class GamepadInput
{
    // 사전 정의된 입력들을 visual studio가 찾게 해주고, 오타를 방지하기 위한 클래스.
    public const string
        Face_Up = "Face_Up", Face_Down = "Face_Down", Face_Left = "Face_Left", Face_Right = "Face_Right",
        LT = "LT", RT = "RT", LB = "LB", RB = "RB",
        Up = "Up", Down = "Down", Left = "Left", Right = "Right",
        Start = "Start", Select = "Select";
}

public partial class InputManager : Node
{
    public static InputManager Instance { get; private set; }

    public float Horizon { get; private set; }
    public float Vertical { get; private set; }


    public event Action<StringName> ActionPressed;
    public event Action<StringName> ActionReleased;


    public override void _Ready()
    {
        Instance = this;
    }

    public List<StringName> _inputNames = new List<StringName>
    {
        GamepadInput.LT, GamepadInput.RT, GamepadInput.LB, GamepadInput.RB,
        GamepadInput.Face_Up, GamepadInput.Face_Down, GamepadInput.Face_Left, GamepadInput.Face_Right,
    };


    public override void _Input(InputEvent @event)
    {
        foreach (StringName action in _inputNames)
        {
            if (@event.IsActionPressed(action))
            {
                ActionPressed?.Invoke(action);
            }
            else if (@event.IsActionReleased(action))
            {
                ActionReleased?.Invoke(action);
            }
        }
    }

    public override void _Process(double delta)
    {
        Horizon = Input.GetAxis(GamepadInput.Left, GamepadInput.Right);
        Vertical = Input.GetAxis(GamepadInput.Up, GamepadInput.Down);
    }
}
