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

    [Export] public float WallJumpSpeed { get; private set; }
    [Export] public float WallJumpDelta { get; private set; }

    [Export] public float WallApexDelta { get; private set; }

    [Export] public float SprintJumpDelta { get; private set; }
    [Export] public float SprintApexDelta { get; private set; }

    [Export] public float SprintDecelDelta { get; private set; }

    [Export] public double InputBuffer { get; private set; }

    [Export] public float HorizonInertia { get; private set; }
    public double JumpBuffer { get; private set; } = 0.0;

    [Export] public float DashInAirDelta { get; private set; }
    [Export] public float DashInAirRise { get; private set; }

    public enum LREnum
    {
        Left, Right
    }


    [Export] public StateMachine_Move StateMachine_Move { get; set; }


    [Export] public RayCast2D RayCast_Ledge_Left { get; set; }
    [Export] public RayCast2D RayCast_Ledge_Right { get; set; }

    [Export] public RayCast2D RayCast_Wall_Left { get; set; }
    [Export] public RayCast2D RayCast_Wall_Right { get; set; }

    [Export] public AnimatedSprite2D Animation { get; set; }


    public override void _Ready()
    {
        InputManager.Instance.ActionPressed += ApplyJumpBuffer;
    }

    public override void _PhysicsProcess(double delta)
    {
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
