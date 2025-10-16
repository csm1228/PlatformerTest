using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CooldownTimer : Timer
{
    public override void _Ready()
    {
        SetCooldownTime();
    }
    public virtual void SetCooldownTime() { }
}
