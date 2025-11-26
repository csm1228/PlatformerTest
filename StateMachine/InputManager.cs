using Godot;
using System;

public class InputName
{
    public const string
        up = "up", down = "down", right = "right", left = "left",
        jump = "jump", attack = "attack", dash = "dash", parry = "parry",
        skill = "skill", heal = "heal", grab = "grab",
        map = "map", menu = "menu", pause = "pause";
}

public partial class InputManager : Node
{
    [Export] StateMachine_Move FSM { get; set; }

    private float rawInput_X;
    private float rawInput_Y;

    // Normalized Vector2
    public Vector2 InputDir_Normalized { get; private set; }
    public Vector2 InputDir { get; private set; }


    public float LastInput_Horizon { get; private set; }


    public const double BufferTime = 0.1;

    public double JumpBuffer = 0.0;
    public double AttackBuffer = 0.0;


    public float CutLine = 0.2f;

    public override void _Ready()
    {
        InputDir = Vector2I.Zero;
        LastInput_Horizon = 1.0f;
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessInputs();

        if (JumpBuffer > 0)
        {
            JumpBuffer -= delta;
        }   
        if (AttackBuffer > 0)
        {
            AttackBuffer -= delta;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed(InputName.jump))
        {
            JumpBuffer = BufferTime;
        }
        else if (@event.IsActionPressed(InputName.attack))
        {
            AttackBuffer = BufferTime;
        }

        FSM.CurrentState.HandleInputEvent(@event);
        GetViewport().SetInputAsHandled();
    }

    private void ProcessInputs()
    {
        // 상하좌우 입력을 가공하는 함수.
        // 패드 스틱, 패드 방향키, 키보드 방향키 모두에 대응할 수 있도록 함.
        rawInput_X = Math.Sign(Input.GetAxis(InputName.left, InputName.right));
        rawInput_Y = Math.Sign(Input.GetAxis(InputName.up, InputName.down));

        InputDir = new(rawInput_X, rawInput_Y);
        InputDir_Normalized = InputDir.Normalized();

        if (rawInput_X != 0)
        {
            LastInput_Horizon = InputDir.X;
        }
    }

    public void ConsumeJumpBuffer()
    {
        JumpBuffer = 0;
    }

    public void ConsumeAttackBuffer()
    {
        AttackBuffer = 0;
    }
}
