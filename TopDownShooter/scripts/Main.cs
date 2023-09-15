using Godot;
using System;

public class Main : Node2D
{
    
    [Export]
    public PackedScene EnemyScene = (PackedScene)GD.Load("res://scenes/Enemy.tscn");
    private PathFollow2D enemySpawnLocation;
    private TextEdit waveTextBox;
    private TextEdit enemyKilledTextBox;

    private int initialEnemies;




    public int wave;

    public override void _Ready()
    {
        enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");

        waveTextBox = GetNode<TextEdit>("Player/Camera2D/CanvasLayer/Wave");
        enemyKilledTextBox = GetNode<TextEdit>("Player/Camera2D/CanvasLayer/EnemiesKilled");

        Global.wave = 0;
        Global.enemiesKilled = 0;
        Global.username = "";

        initialEnemies = 2;
        newWave();

    
    }

    public void newWave(){
        Global.wave ++;
        initialEnemies *= 2;
        waveTextBox.Text = $"Wave = {Global.wave}";
        enemyKilledTextBox.Text = $"Enemies Killed = {Global.enemiesKilled}";

        for (int i=0; i < initialEnemies; i++){
            spawnEnemy();
        }
        
    }

    public void spawnEnemy(){
        GD.Randomize();

        var enemy = (Enemy) EnemyScene.Instance();

        enemySpawnLocation.Offset = GD.Randi();

        enemy.Position = enemySpawnLocation.Position;

        AddChild(enemy);
    }

    public void enemyKilled(){
        Global.enemiesKilled += 1;

        enemyKilledTextBox.Text = $"Enemies Killed = {Global.enemiesKilled}";
    }

    public override void _Process(float delta)
    {
        
        if (GetTree().GetNodesInGroup("enemy").Count == 0){
            newWave();
        }

        
    }
}
