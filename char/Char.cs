using Godot;
using System;

public partial class Char : CharacterBody2D
{
    private float _walkSpeed = 800.0f;
    private float _jumpSpeed = -1800.0f;
    private float _dashSpeed = 4800.0f;
    private float _climbSpeed = -400.0f;
    private float _gravity = 1900.0f;
    private float _wallSlipperSpeed = 600.0f;

    public float WalkSpeed => _walkSpeed;
    public float JumpSpeed => _jumpSpeed;
    public float DashSpeed => _dashSpeed;
    public float ClimbSpeed => _climbSpeed;
    public float Gravity => _gravity;
    public float WallSlipperSpeed => _wallSlipperSpeed;

    public enum LREnum
    {
        Left, Right
    }

    // 마지막 입력 방향
    public LREnum LastInputDirection;

    public LREnum LastHoldingWallDirection;



    [Export] public StateMachine_Move StateMachine_Move { get; set; }


    [Export] public RayCast2D RayCast_Upper_Left { get; set; }
    [Export] public RayCast2D RayCast_Upper_Right { get; set; }

    [Export] public RayCast2D RayCast_Lower_Left { get; set; }
    [Export] public RayCast2D RayCast_Lower_Right { get; set; }

    public override void _Ready()
    {
        LastInputDirection = LREnum.Left;
        LastHoldingWallDirection = LREnum.Left;
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

    
}
