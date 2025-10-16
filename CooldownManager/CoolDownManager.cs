using Godot;
using System;

public partial class CoolDownManager : Node
{
    [Export] Timer DashCooldownTimer { get; set; }
    [Export] Timer AttackCooldownTimer { get; set; }
    [Export] Timer SkillCooldownTimer { get; set; }

    public bool IsDashReady = true;
    public bool IsAttackReady = true;
    public bool IsSkillReady = true;

    public void StartCooling_Dash()
    {
        IsDashReady = false;
        DashCooldownTimer.Start();
    }

    private void _on_dash_cooldown_timer_timeout()
    {
        IsDashReady = true;
    }
}
