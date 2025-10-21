using Godot;
using System;

public partial class FpsLabel : Label
{
    public override void _Process(double delta)
    {
        double fps = Performance.GetMonitor(Performance.Monitor.TimeFps);

        Text = $"FPS : {fps}";
    }
}
