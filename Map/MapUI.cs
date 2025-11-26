using Godot;
using System;
using System.Diagnostics.Contracts;

public partial class MapUI : Control
{
    [Export] AnimationPlayer animationPlayer { get; set; }
    [Export] Label label { get; set; }
    [Export] Panel Panel { get; set; }

    [Export] public Label UnlockLabel { get; set; }

    private Tween tween;

    public override void _Ready()
    {
        label.Visible = false;
        Panel.Visible = false;
    }

    public void OpenMap()
    {
        animationPlayer.Stop();
        animationPlayer.Play("FadeIn");
    }

    public void CloseMap()
    {
        animationPlayer.Stop();
        animationPlayer.Play("FadeOut");
    }
}
