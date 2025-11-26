using Godot;
using System;

public partial class CoolDownManager : Node
{
    [Export] private Timer DashCoolDownTimer { get; set; }



    public bool IsDashReady { get; private set; }

    public override void _Ready()
    {
        IsDashReady = true;
    }

    public void StartCollingDash()
    {
        IsDashReady = false;
        DashCoolDownTimer.Start();
    }

    private void _on_dash_timeout()
    {
        IsDashReady = true;
    }
}
