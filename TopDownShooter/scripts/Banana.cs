using Godot;
using System;

public class Banana : RigidBody2D
{


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Timer killTimer = GetNode<Timer>("KillTimer");
        killTimer.Start();
    }

    public void readyAnimation(){
        // For generating random numbers
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        int[] numArr = {-1, 1};
        int reverse = numArr[rng.RandiRange(1, 2) % numArr.Length];

        // Animation Player reference and play animation
        AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.PlaybackSpeed = reverse * rng.RandfRange(1.0f, 3.0f);
        animationPlayer.Play("Spin");
    }

    // Destroys banana after a while of not hitting anything
    public void _on_KillTimer_timeout(){
        QueueFree();
    }


    // Called when banana collides with an object
    public void _on_Banana_body_entered(Node2D body){
        
        // If the body is an enemy
        if (body is Enemy enemy){
            // Calls enemy method
            enemy.handleHit();
            // Destroys itself
            QueueFree();
        }
    }



}
