using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class InputManager : Node
{
    private int _inputFrame_Jump = 0;
    private int _inputFrame_Dash = 0;

    public int InputFrame_Jump => _inputFrame_Jump;
    public int InputFrame_Dash => _inputFrame_Dash;

    private int Buffer = 10;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(GamepadInput.Joypad_Down))
        {
            _inputFrame_Jump = Buffer;
        }
        else if (@event.IsActionPressed(GamepadInput.RT))
        {
            _inputFrame_Dash = Buffer;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_inputFrame_Jump > 0)
        {
            _inputFrame_Jump--;
        }

        if (_inputFrame_Dash > 0)
        {
            _inputFrame_Dash--;
        }
    }

    public bool IsJumpOnBuffer()
    {
        bool isJumpOnBuffer = (_inputFrame_Jump > 0);

        return isJumpOnBuffer;
    }

    public bool IsDashOnBuffer()
    {
        bool isDashOnBuffer = (_inputFrame_Dash > 0);

        return isDashOnBuffer;
    }
}
