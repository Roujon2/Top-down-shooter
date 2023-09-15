using Godot;
using System;

public class MainMenu : Control
{
    public void _on_Start_pressed()
    {
        GetTree().ChangeScene("res://scenes/Main.tscn");
    }

    public void _on_Quit_pressed()
    {
        GetTree().Quit();
    }
}
