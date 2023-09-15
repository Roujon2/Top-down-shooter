using Godot;
using System;

public class Enemy : KinematicBody2D
{

    public int speed;
    public int health;

    private Stats stats;

    private Sprite enemySprite;
    private AnimationPlayer animationPlayer;
    private Timer hitTimer;
    private Timer squashTimer;
    private Timer spawnTimer;
    
    private Area2D ContactDetection;
    private Area2D EngageDetection;
    private CollisionShape2D bodyShape;

    private AI ai;

    public bool squashed;


    public override void _Ready()
    {
        // Sprite reference
        enemySprite = GetNode<Sprite>("VisualNode/Sprite");

        enemySprite.Modulate = Color.Color8(255, 255, 255, 255);

        squashed = false;

        // Referencing collisions
        ContactDetection = GetNode<Area2D>("ContactDetection");
        EngageDetection = GetNode<Area2D>("AI/EngageDetection");
        bodyShape = GetNode<CollisionShape2D>("CollisionShape2D");


        // Stats reference
        stats = GetNode<Stats>("Stats");
        speed = 0;

        // Stat setup
        health = stats.getHealth();

        // AI node reference
        ai = GetNode<AI>("AI");

        ai.initialize(this);

        // Animation player reference
        animationPlayer = GetNode<AnimationPlayer>("VisualNode/AnimationPlayer");

        animationPlayer.Play("Idle");

        // Timer references
        hitTimer = GetNode<Timer>("HitTimer");
        squashTimer = GetNode<Timer>("SquashTimer");   
        spawnTimer = GetNode<Timer>("SpawnTimer");

        spawnTimer.Start();

        ContactDetection.SetDeferred("monitoring", false);
        EngageDetection.SetDeferred("monitoring", false);
        bodyShape.SetDeferred("disabled", true);

    }

    public override void _PhysicsProcess(float delta)
    {
        // Player reference
        var player = GetParent().GetNode<Player.Player>("Player");

        // Movement if not squashed
        if (!squashed)
        {
            float moveAmount = speed * delta;
            Vector2 direction = (player.Position - this.Position).Normalized();
            MoveAndCollide(direction * moveAmount);
        }
    }


    // Handles contact with player
    public void _on_ContactDetection_body_entered(Node2D body)
    {
        if (body is Player.Player player)
        {
            // Calls the player method
            player.handleHit();
        }

    }


    // Handles hit by banana
    public void handleHit()
    {

        // Changes color briefly (set with timer)
        enemySprite.Modulate = Color.Color8(255, 67, 67, 255);
        hitTimer.Start();


        // Reduces health and checks if it should die
        health -= 1;
        if (health <= 0)
        {
            death();
        }
    }

    public void handleSquash()
    {
        // Setting the state
        ai.setState(2);
        ContactDetection.SetDeferred("monitoring", false);
        EngageDetection.SetDeferred("monitoring", false);
        bodyShape.SetDeferred("disabled", true);

        squashTimer.Start();


        
    }

    // Enemy back to normal color
    public void _on_HitTimer_timeout()
    {
        enemySprite.Modulate = Color.Color8(255, 255, 255, 255);
    }

    public void _on_SpawnTimer_timeout(){
        ContactDetection.SetDeferred("monitoring", true);
        EngageDetection.SetDeferred("monitoring", true);
        bodyShape.SetDeferred("disabled", false);

        speed = stats.getSpeed();
    }

    public void _on_SquashTimer_timeout(){
        death();
    }

    public void playAnimation(string animation)
    {
        animationPlayer.Play(animation);
    }


    // Kills the node
    public void death()
    {
        QueueFree();

        // Player reference
        var player = GetParent().GetNode<Player.Player>("Player");
        player.addKill();

    }
}
