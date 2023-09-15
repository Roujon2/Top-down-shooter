using Godot;
using System.Collections.Generic;

public class GameOver : Control
{

    private int wave;
    private int enemiesKilled;

    private TextEdit textEdit;

    private LineEdit lineEdit;

    private Button saveUsername;
    private Button leaderboard;




    public override void _EnterTree()
    {
        base._EnterTree();

        lineEdit = GetNode<LineEdit>("HBoxContainer/LineEdit");

        saveUsername = GetNode<Button>("HBoxContainer/SaveUsername");

        saveUsername.Disabled = true;

        leaderboard = GetNode<Button>("VBoxContainer/Leaderboard");
        leaderboard.Disabled = true;

        if(!(Global.username.Length > 0)){
            lineEdit.Editable = true;
        }

        textEdit = GetNode<TextEdit>("CanvasLayer/TextEdit");

        wave = Global.wave;
        enemiesKilled = Global.enemiesKilled;

        textEdit.Text = $"Wave = {wave}\nEnemies Killed = {enemiesKilled}";


        Dictionary<string, int> leaderboardData = new Dictionary<string, int>{{"asdf", 43}};

    }

    public void _on_SaveUsername_pressed()
    {
        Global.username = lineEdit.Text;

        leaderboard.Disabled = false;

        saveUsername.Disabled = true;

        lineEdit.Editable = false;

    }

    public void _on_LineEdit_text_changed(string text)
    {
        if (lineEdit.Text.Length > 0 && Global.username.Length == 0)
        {
            saveUsername.Disabled = false;
        }
        else
        {
            saveUsername.Disabled = true;
        }
    }

    public void _on_Leaderboard_pressed()
    {
        GetTree().ChangeScene("res://scenes/Leaderboard.tscn");
    }

    public void _on_PlayAgain_pressed()
    {
        GetTree().ChangeScene("res://scenes/Main.tscn");
    }

    public void _on_Quit_pressed()
    {
        GetTree().Quit();
    }
}
