using Godot;
using System;


public class SuperState_Move
{
    public const string
        Grounded = "Grounded",
        Airborne = "Airborne",
        Dash = "Dash",
        Wall = "Wall",
        Ledge = "Ledge",
        Sprint = "Sprint";
}

public class State_Move
{
    public const string
        Idle = "Idle",
        Walk = "Walk",



        Jump = "Jump",
        Apex = "Apex",
        Fall = "Fall",



        Dash_Grounded = "Dash_Grounded",
        Dash_Fall = "Dash_Fall",
        Dash_InAir = "Dash_InAir",



        Wall_Hold = "Wall_Hold",
        Wall_Jump = "Wall_Jump",
        Wall_Slipper = "Wall_Slipper",
        Wall_Climb = "Wall_Climb",



        Ledge_Grab = "Ledge_Grab",
        Ledge_Climb = "Ledge_Climb",



        Sprint_Grounded = "Sprint_Grounded",
        Sprint_Jump = "Sprint_Jump",
        Sprint_Apex = "Sprint_Apex",
        Sprint_Fall = "Sprint_Fall",
        Sprint_Decel = "Sprint_Decel",
        Sprint_Bump = "Sprint_Bump";
}

public partial class StateMachine_Move : Node
{
    [Export] Char Player { get; set; }


    private SuperState _currentSuperState;
    public SuperState CurrentMoveSuperState => _currentSuperState;
    public SubState CurrentMoveSubState => _currentSuperState.CurrentSubState;

    [Export] public CoolDownManager CooldownManager { get; set; }

    public bool CanDash = true;
    public bool CanDoubleJump = true;
    public bool IsDoubleJumpUnlocked = false;

    [Export] SuperState Grounded { get; set; }
    [Export] SuperState Airborne { get; set; }
    [Export] SuperState Dash { get; set; }


    [Export] SuperState Wall { get; set; }
    [Export] SuperState Ledge { get; set; }
    [Export] SuperState Sprint { get; set; }



    public override void _Ready()
    {
        TransState(SuperState_Move.Grounded, State_Move.Idle);
    }

    public void TransState(string superStateName, string subStateName)
    {
        SuperState newState = GetNode<SuperState>(superStateName);

        _currentSuperState?.CurrentSubState?.Exit();
        _currentSuperState?.Exit();

        _currentSuperState = newState;

        _currentSuperState.Enter();
        _currentSuperState.TransSubState(subStateName);
        _currentSuperState.CurrentSubState.Enter();

    }

    public void HandlePhysics(double delta)
    {
        CheckWall();
        _currentSuperState?.HandlePhysics(delta);
        if (Player.IsOnFloor())
        {
            CanDash = true;
            CanDoubleJump = true;
        }
    }

    public void HandleTransState(double delta)
    {
        _currentSuperState?.HandleTransState(delta);
    }

    public void CheckWall()
    {
        // 어떤 방향의 벽에 붙어있는지 검사. 주로 벽에서 떨어질 때 호출
        if (Player.GetWallNormal().X > 0)
        {
            Player.LastHoldingWallDirection = Char.LREnum.Left;
            return;
        }
        else if (Player.GetWallNormal().X < 0)
        {
            Player.LastHoldingWallDirection = Char.LREnum.Right;
            return;
        }
    }

    public bool IsOnWall()
    {
        // 엔진 내 IsOnWall() 함수가 이상해서, 하나 만듦
        bool isCollidingLeftWall = Player.RayCast_Upper_Left.IsColliding() && Player.RayCast_Lower_Left.IsColliding();
        bool isCollidingRightWall = Player.RayCast_Upper_Right.IsColliding() && Player.RayCast_Lower_Right.IsColliding();

        bool isOnWall = isCollidingLeftWall || isCollidingRightWall;

        return isOnWall;
    }

    public bool IsOnLedge()
    {
        bool isCollidingLeftLedge = !Player.RayCast_Upper_Left.IsColliding() && Player.RayCast_Lower_Left.IsColliding();
        bool isCollidingRightLedge = !Player.RayCast_Upper_Right.IsColliding() && Player.RayCast_Lower_Right.IsColliding();

        bool isOnLedge = isCollidingLeftLedge || isCollidingRightLedge;

        return isOnLedge;
    }


    public void FixActionDirection()
    {
        Player.ActionDirection = Player.LastInputDirection;
    }

    public void TransToWalkOrIdle()
    {
        if (InputManager.Instance.Horizon != 0)
        {
            TransState(SuperState_Move.Grounded, State_Move.Walk);
            return;
        }
        else
        {
            TransState(SuperState_Move.Grounded, State_Move.Idle);
            return;
        }
    }

    public void TransWall()
    {
        CheckWall();

        if (IsOnWall())
        {
            TransState(SuperState_Move.Wall, State_Move.Wall_Hold);
            return;
        }
        else if (IsOnLedge())
        {
            TransState(SuperState_Move.Ledge, State_Move.Ledge_Grab);
            return;
        }
        else
        {
            TransState(SuperState_Move.Airborne, State_Move.Fall);
            return;
        }
    }
}
