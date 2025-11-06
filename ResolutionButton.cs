using Godot;
using System;

public partial class ResolutionButton : OptionButton
{
    public override void _Ready()
    {
        int currentScreenIndex = GetWindow().CurrentScreen;

        Vector2 DisplaySize = DisplayServer.ScreenGetSize();

        AddItem("FullScreen", 0);

        if (DisplaySize.X > 3840 && DisplaySize.Y > 2160)
        {
            AddItem("UHD", 1);
        }
        if (DisplaySize.X > 2560 && DisplaySize.Y > 1440)
        {
            AddItem("QHD", 2);
        }
        if (DisplaySize.X > 1920 && DisplaySize.Y > 1080)
        {
            AddItem("FHD", 3);
        }
        if (DisplaySize.X > 1600 && DisplaySize.Y > 900)
        {
            AddItem("HD+", 4);
        }

        AddItem("HD", 5);

        Selected = 0;
    }

    private void _on_item_selected(int index)
    {
        int selectedId = GetItemId(index);

        switch(selectedId)
        {
            case 0:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                break;
            case 1:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetSize(Resolution.UHD);
                AlignWindow();
                break;
            case 2:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetSize(Resolution.QHD);
                AlignWindow();
                break;
            case 3:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetSize(Resolution.FHD);
                AlignWindow();
                break;
            case 4:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetSize(Resolution.HDP);
                AlignWindow();
                break;
            case 5:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetSize(Resolution.HD);
                AlignWindow();
                break;
        }
    }

    private void AlignWindow()
    {
        int currentScreenIndex = GetWindow().CurrentScreen;

        Vector2I DisplaySize = DisplayServer.ScreenGetSize(currentScreenIndex);
        Vector2I windowSize = DisplayServer.WindowGetSizeWithDecorations();
        Vector2I screenPosition = DisplayServer.ScreenGetPosition(currentScreenIndex);

        Vector2I newPosition = screenPosition + ((DisplaySize - windowSize) / 2);

        DisplayServer.WindowSetPosition(newPosition);
    }
}
