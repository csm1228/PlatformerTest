using Godot;
using System;

public partial class Ui : Control
{
    [Export] Char Player { get; set; }

    [Export] MapUI mapUI { get; set; }

    private float FadeDuration = 0.2f;
    
    public override void _Ready()
    {
        Player.StateMachine_Move.OpenMap += OpenMap;
        Player.StateMachine_Move.CloseMap += CloseMap;
    }

    public override void _ExitTree()
    {
        Player.StateMachine_Move.OpenMap -= OpenMap;
        Player.StateMachine_Move.CloseMap -= CloseMap;
    }

    public void OpenMap()
    {
        mapUI.OpenMap();
        mapUI.UnlockLabel.Text = $"대쉬 해금 : {Player.StateMachine_Move.IsDashUnlocked}\n벽 액션 해금 : {Player.StateMachine_Move.IsWallActionUnlocked}\n이단 점프 해금 : {Player.StateMachine_Move.IsDoubleJumpUnlocked}";
    }

    public void CloseMap()
    {
        mapUI.CloseMap();
    }
}
