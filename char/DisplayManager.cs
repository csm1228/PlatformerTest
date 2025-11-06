using Godot;
using System;

public static class Resolution
{
    public static Vector2I UHD = new Vector2I(3840, 2160);
    public static Vector2I QHD = new Vector2I(2560, 1440);
    public static Vector2I FHD = new Vector2I(1920, 1080);
    public static Vector2I HDP = new Vector2I(1600, 900);
    public static Vector2I HD = new Vector2I(1280, 720);
}
public partial class DisplayManager : Node
{


    public override void _Ready()
    {
        int currentScreenIndex = GetWindow().CurrentScreen;

        Vector2 DisplaySize = DisplayServer.ScreenGetSize();

        if (DisplaySize.X >= 3840 && DisplaySize.Y >= 2160)
        {
            DisplayServer.WindowSetSize(Resolution.UHD);
        }
        else if (DisplaySize.X >= 2560 && DisplaySize.Y >= 1440)
        {
            DisplayServer.WindowSetSize(Resolution.QHD);
        }
        else if (DisplaySize.X >= 1920 && DisplaySize.Y >= 1080)
        {
            DisplayServer.WindowSetSize(Resolution.FHD);
        }
        else if (DisplaySize.X >= 1600 && DisplaySize.Y >= 900)
        {
            DisplayServer.WindowSetSize(Resolution.FHD);
        }
        else
        {
            DisplayServer.WindowSetSize(Resolution.HD);
        }
    }
}
