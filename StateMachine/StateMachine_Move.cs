using Godot;
using Godot.Collections;
using System;
using System.Text.RegularExpressions;

public class AnimationName
{

}

public static class StateName
{
    public static StringName
        Grounded = "Grounded", Map = "Map", Crouch = "Crouch",
        Jump = "Jump", Fall = "Fall",
        Dash = "Dash", DashInAir = "DashInAir",
        Sprint = "Sprint", SprintJump = "SprintJump", Decel = "Decel",
        WallSlipper = "WallSlipper", WallJump = "WallJump",
        WallHold = "WallHold", WallClimb = "WallClimb",
        LedgeClimb = "LedgeClimb",
        DoubleJump = "DoubleJump";
}

public partial class StateMachine_Move : Node
{
    [Signal] public delegate void OpenMapEventHandler();
    [Signal] public delegate void CloseMapEventHandler();

    [Export] public Char Player { get; set; }
    [Export] public AnimationTree Animation_Player { get; private set; }
    [Export] public AnimationTree Animation_Attack { get; private set; }
    [Export] public InputManager InputManager { get; private set; }

    [Export] public CollisionShape2D Hitbox { get; private set; }
    [Export] public CollisionShape2D Hitbox_Crouch { get; private set; }

    // 디버그용임
    [Export] public State InitialState { get; private set; }

    [Export] public TerrainDetector TerrainDetector { get; set; }




    // 현재 상태
    public State CurrentState { get; private set; }
    public StringName LastStateName { get; private set; }

    // State 이름, State 노드 딕셔너리
    private Dictionary<StringName, State> _stateDictionary = new();

    // State에서 가져다 쓸 변수 미리 선언
    public Vector2 velocity;

    public float LastInput_Horizon;
    public float LastInput_Vertical;

    public float ActionDirection;

    // 이하 수치들
    [Export] public float WalkSpeed { get; private set; }
    [Export] public float MapWalkSpeed { get; private set; }
    [Export] public float CrouchSpeed { get; private set; }


    [Export] public float JumpSpeed { get; private set; }
    [Export] public float JumpAccel { get; private set; }
    [Export] public float FallAccel { get; private set; }
    [Export] public float MaxFallSpeed { get; private set; }



    [Export] public float DashSpeed { get; private set; }
    [Export] public float SprintSpeed { get; private set; }
    [Export] public float SprintJumpAccel { get; private set; }
    [Export] public float DecelSpeed { get; private set; }

    [Export] public float SwimSpeed { get; private set; }


    [Export] public float DashInAirSpeed_X { get; private set; }
    [Export] public float DashInAirSpeed_Y { get; private set; }
    [Export] public float DashInAirDecel_X { get; private set; }
    [Export] public float DashInAirDecel_Y { get; private set; }


    [Export] public float MaxSlipperSpeed { get; private set; }
    [Export] public float SlipperAccel { get; private set; }


    [Export] public float WallJumpSpeed { get; private set; }
    [Export] public float WallJumpAccel { get; private set; }
    [Export] public float WallJumpAccel_Opposite { get; private set; }


    [Export] public float WallClimbSpeed { get; private set; }
    [Export] public float WallClimbSpeed_Fast { get; private set; }

    // 해금 여부
    public bool IsDoubleJumpUnlocked = false;
    public bool IsDashUnlocked = false;
    public bool IsWallActionUnlocked = false;



    // 사용 가능 여부
    public bool CanDashInAir = true;
    public bool CanDoubleJump = true;

    [Export] public CoolDownManager CooltimeManager { get; private set; }


    public override void _Ready()
    {
        AddStateToDictionary();

        CurrentState = InitialState;

        LastInput_Horizon = 1.0f;
        LastInput_Vertical = 0.0f;

        ActionDirection = 0.0f;
    }

    public void AddStateToDictionary()
    {
        // State 노드들을 딕셔너리에 등록
        _stateDictionary.Clear();

        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _stateDictionary[child.Name] = state;
            }
        }
    }




    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsProcess(delta);

        Player.MoveAndSlide();

        CurrentState.HandleTransState(delta);
    }

    public void TryTransState(StringName stateName)
    {
        // 해금 여부, 쿨다운 여부, 사용 가능 횟수가 남아있는지 등을 확인
        // 결과적으로 전환 가능하다면 전환함.
        // State에서는 변환 시도만 하면 됨

        switch (stateName)
        {
            case var name when stateName == StateName.DashInAir:
                if (!CanDashInAir || !IsDashUnlocked) return;
                break;

            case var name when stateName == StateName.Dash:
                if (!IsDashUnlocked || !CooltimeManager.IsDashReady) return;
                else CooltimeManager.StartCollingDash();
                break;

            case var name when stateName == StateName.Sprint:
                if (!IsDashUnlocked) return;
                break;

            case var name when stateName == StateName.DoubleJump:
                if (!CanDoubleJump || !IsDoubleJumpUnlocked) return;
                break;

            case var name when stateName == StateName.WallHold || stateName == StateName.WallClimb:
                if (!IsWallActionUnlocked) stateName = StateName.WallSlipper;
                break;
        }
        
        TransState(stateName);
    }

    public void TransState(StringName stateName)
    {
        if (!_stateDictionary.ContainsKey(stateName))
        {
            GD.PrintErr($"유효하지 않은 stateName : {stateName}");
            return;
        }

        CurrentState?.Exit();

        CurrentState = _stateDictionary[stateName];

        CurrentState.Enter();
    }






    public void FixActionDirection()
    {
        if (InputManager.InputDir.X == 0.0f)
        {
            ActionDirection = InputManager.LastInput_Horizon;
        }
        else
        {
            ActionDirection = InputManager.InputDir.X;
        }
    }

    public void RestoreAirAction()
    {
        CanDashInAir = true;
        CanDoubleJump = true;
    }
}
