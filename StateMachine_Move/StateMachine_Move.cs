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

        Sprint = "Sprint",

        Mounting = "Mounting",
        Mounted_Grounded = "Mounted_Grounded",
        Mounted_Airborne = "Mounted_Airborne";
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
        Sprint_Bump = "Sprint_Bump",



        Ride_Call = "Ride_Call",
        Ride_GetOn = "Ride_GetOn",
        Ride_GetOff = "Ride_GetOff",



        Ride_Idle = "Ride_Idle",
        Ride_Walk = "Ride_Walk",
        Ride_Sprite = "Ride_Sprint",
        Ride_Decel = "Ride_Decel",



        Ride_Jump = "Ride_Jump",
        Ride_Apex = "Ride_Apex",
        Ride_Fall = "Ride_Fall";
}

public class GamepadInput
{
    // 사전 정의된 입력들을 visual studio가 찾게 해주고, 오타를 방지하기 위한 클래스.
    public const string
        Joypad_Down = "Joypad_Down", Joypad_Up = "Joypad_Up", Joypad_Left = "Joypad_Left", Joypad_Right = "Joypad_Right",
        LT = "LT", RT = "RT", LB = "LB", RB = "RB",
        Up = "Up", Down = "Down", Left = "Left", Right = "Right";
}


public partial class StateMachine_Move : Node
{
    [Export] Char Player { get; set; }



    private SuperState _currentSuperState;
    public SuperState CurrentMoveSuperState => _currentSuperState;
    public SubState CurrentMoveSubState => _currentSuperState.CurrentSubState;

    [Export] public CoolDownManager CooldownManager { get; set; }


    [Export] public InputManager inputManager { get; set; }


    [Export] SuperState Grounded { get; set; }
    [Export] SuperState Airborne { get; set; }
    [Export] SuperState Dash { get; set; }


    [Export] SuperState Wall { get; set; }
    [Export] SuperState Ledge { get; set; }

    [Export] SuperState Sprint { get; set; }


    [Export] SuperState Mounting { get; set; }
    [Export] SuperState Mounted_Grounded { get; set; }
    [Export] SuperState Mounted_Airborne { get; set; }





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
    }

    public void HandleTransState(double delta)
    {
        _currentSuperState?.HandleTransState(delta);
    }

    public void CheckWall()
    {
        // 어떤 방향의 벽에 붙어있는지 검사
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


    public bool IsInputLorR()
    {
        if (Input.IsActionPressed(GamepadInput.Left) || Input.IsActionPressed(GamepadInput.Right))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TransToWalkOrIdle()
    {
        if (IsInputLorR())
        {
            TransState(SuperState_Move.Grounded, State_Move.Walk);
        }
        else
        {
            TransState(SuperState_Move.Grounded, State_Move.Idle);
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

    public bool IsOppositeInput()
    {
        bool isOpposite = Player.ActionDirection == Char.LREnum.Left && Input.IsActionPressed(GamepadInput.Right) || Player.ActionDirection == Char.LREnum.Right && Input.IsActionPressed(GamepadInput.Left);

        return isOpposite;
    }
}
