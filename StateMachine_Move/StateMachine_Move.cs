using Godot;
using System;
using static Char;


public class SuperState_Move
{
    public const string
        Grounded = "Grounded",
        Airborne = "Airborne",
        Wall = "Wall",

        Action = "Action";
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

        Wall_Jump = "Wall_Jump",
        Wall_Apex = "Wall_Apex",

        Wall_Hold = "Wall_Hold",
        Wall_Slipper = "Wall_Slipper",
        Wall_Climb = "Wall_Climb",

        Ledge_Climb = "Ledge_Climb",

        Sprint_Grounded = "Sprint_Grounded",
        Sprint_Jump = "Sprint_Jump",
        Sprint_Apex = "Sprint_Apex",
        Sprint_Fall = "Sprint_Fall",
        Sprint_Decel = "Sprint_Decel",
        Sprint_Bump = "Sprint_Bump",

        Parry = "Parry";
}

public partial class StateMachine_Move : Node
{
    [Export] Char Player { get; set; }
    public State CurrentState { get; private set; }

    [Export] public CoolDownManager CooldownManager { get; set; }

    public bool CanDash = true;
    public bool CanDoubleJump = true;
    public bool IsDoubleJumpUnlocked = false;

    // 애니메이션 방향, 다음 액션 방향 결정하는 변수
    public LREnum PlayerFacingDirection;

    public LREnum HoldingWallDirection;
    public LREnum HoldingLedgeDirection;

    public LREnum ActionDirection;



    public override void _Ready()
    {
        TransState(State_Move.Idle);

        PlayerFacingDirection = LREnum.Right;
    }

    public void TransState(string subStateName)
    {
        CurrentState?.DisconnectEventSignal();
        CurrentState?.Exit();

        CurrentState = GetNode<State>(subStateName);

        CurrentState.Enter();
        CurrentState.ConnectEventSignal();
    }

    public void HandlePhysics(double delta)
    {
        // 왼쪽 바라보고 있으면 애니메이션 반전. 모든 건 오른쪽 바라보는 거 기준으로 만들기.
        Player.Animation.FlipH = (PlayerFacingDirection == LREnum.Left);

        CurrentState?.HandlePhysics(delta);
    }

    public void HandleTransState(double delta)
    {
        CurrentState?.HandleTransState(delta);
    }

    public void CheckWallDirection()
    {
        if (Player.RayCast_Wall_Left.IsColliding())
        {
            HoldingWallDirection = Char.LREnum.Left;
            return;
        }
        else if (Player.RayCast_Wall_Right.IsColliding())
        {
            HoldingWallDirection = Char.LREnum.Right;
            return;
        }
    }

    public void CheckLedgeDirection()
    {
        if (Player.RayCast_Ledge_Left.IsColliding())
        {
            HoldingLedgeDirection = Char.LREnum.Left;
            return;
        }
        else if (Player.RayCast_Ledge_Right.IsColliding())
        {
            HoldingLedgeDirection = Char.LREnum.Right;
            return;
        }
    }

    public bool IsOnWall()
    {
        // 엔진 내 IsOnWall() 함수가 이상해서, 하나 만듦
        bool isCollidingLeftWall = Player.RayCast_Wall_Left.IsColliding();
        bool isCollidingRightWall = Player.RayCast_Wall_Right.IsColliding();

        bool isOnWall = isCollidingLeftWall || isCollidingRightWall;

        return isOnWall;
    }

    public bool IsOnLedge()
    {
        bool isCollidingLeftLedge = Player.RayCast_Ledge_Left.IsColliding();
        bool isCollidingRightLedge = Player.RayCast_Ledge_Right.IsColliding();

        bool isOnLedge = isCollidingLeftLedge || isCollidingRightLedge;

        return isOnLedge;
    }


    public void FixActionDirection()
    {
        // Sprint, Dash 등 방향 정해진 액션 시작 시 액션의 방향을 고정시키는 용도
        // Enter 함수에서 호출하지 않고, 다른 SubState에서 호출 후 상태 변환
        ActionDirection = PlayerFacingDirection;
    }





    /* 
     
    이하 자주 쓰는 상태 변환 함수 
     
    */
    public void TransToWalkOrIdle()
    {
        if (InputManager.Instance.Horizon != 0)
        {
            TransState(State_Move.Walk);
            return;
        }
        else
        {
            TransState(State_Move.Idle);
            return;
        }
    }

    public void AttachedToPlatform()
    {
        CanDoubleJump = true;
        CanDash = true;
    }
}
