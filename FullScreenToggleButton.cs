using Godot;
using System;

public partial class FullScreenToggleButton : Button
{
    public override void _Ready()
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }

    private void _on_pressed()
    {
        if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        }
        else if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
    }
}
