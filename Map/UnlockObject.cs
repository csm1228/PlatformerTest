using Godot;
using System;

public partial class UnlockObject : Node2D
{
    [Export] Char Player { get; set; }

    private void _on_unlock_dash_body_entered(Node2D body)
    {
        Player.StateMachine_Move.IsDashUnlocked = true;
    }

    private void _on_unlock_wall_action_body_entered(Node2D body)
    {
        Player.StateMachine_Move.IsWallActionUnlocked = true;
    }

    private void _on_unlock_double_jump_body_entered(Node2D body)
    {
        Player.StateMachine_Move.IsDoubleJumpUnlocked = true;
    }
}
