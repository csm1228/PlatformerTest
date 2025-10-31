using Godot;
using System;

public partial class Char : CharacterBody2D
{
    [Export] public float WalkSpeed { get; private set; }
    [Export] public float DashSpeed { get; private set; }
    [Export] public float SprintSpeed { get; private set; }


    [Export] public float Gravity { get; private set; }
    [Export] public float JumpSpeed { get; private set; }
    [Export] public float MaxFallSpeed { get; private set; }
    [Export] public float ClimbSpeed { get; private set; }



    [Export] public float GravityCoefficient_Jump { get; set; }
    [Export] public float GravityCoefficient_Apex { get; set; }
    [Export] public float GravityCoefficient_Fall { get; set; }

    [Export] public float WallSlipperSpeed { get; private set; }


    [Export] public double InputBuffer { get; private set; }
    public double JumpBuffer { get; private set; } = 0.0;


    public enum LREnum
    {
        Left, Right
    }

    // 마지막 입력 방향
    public LREnum LastInputDirection;

    public LREnum LastHoldingWallDirection;

    public LREnum ActionDirection;



    [Export] public StateMachine_Move StateMachine_Move { get; set; }


    [Export] public RayCast2D RayCast_Upper_Left { get; set; }
    [Export] public RayCast2D RayCast_Upper_Right { get; set; }

    [Export] public RayCast2D RayCast_Lower_Left { get; set; }
    [Export] public RayCast2D RayCast_Lower_Right { get; set; }

    [Export] public AnimatedSprite2D Animation { get; set; }


    public override void _Ready()
    {
        LastInputDirection = LREnum.Left;
        LastHoldingWallDirection = LREnum.Left;

        InputManager.Instance.ActionPressed += ApplyJumpBuffer;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed(GamepadInput.Left))
        {
            LastInputDirection = LREnum.Left;
        }
        else if (Input.IsActionPressed(GamepadInput.Right))
        {
            LastInputDirection = LREnum.Right;
        }

        StateMachine_Move.HandlePhysics(delta);

        MoveAndSlide();

        StateMachine_Move.HandleTransState(delta);
    }

    public override void _Process(double delta)
    {
        if (JumpBuffer > 0)
        {
            JumpBuffer -= delta;
        }
    }

    private void ApplyJumpBuffer(StringName action)
    {
        if (action == GamepadInput.Face_Down)
        {
            JumpBuffer = InputBuffer;
        }
    }

    public void ConsumeJumpBuffer()
    {
        JumpBuffer = 0;
    }
    
}
