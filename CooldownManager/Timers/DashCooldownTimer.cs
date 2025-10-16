using Godot;
using System;

public partial class DashCooldownTimer : CooldownTimer
{
    public override void SetCooldownTime()
    {
        // 여러 조건에 따라 쿨타임을 재설정하는 로직을 여기에 넣으면 됨.

        // 일단 0.5초로 초기화.
        WaitTime = 0.5;
    }
}
