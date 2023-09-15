using Godot;
using System;

public class AI : Node2D
{
    
    [Signal] 
    public delegate void stateChanged(int newState);

    private Area2D engageZone;

    private Player.Player player = null;
    private Enemy enemy = null;

    // Enum referencing the different states of the enemy
    enum State{
        CHASE,
        ENGAGE,
        SQUASHED
    }

    // Default state: chasing the player
    private int currentState = (int)State.CHASE;


    public override void _Ready()
    {
        engageZone = GetNode<Area2D>("EngageDetection");
    }

    // Setter for the state
    public void setState(int newState){
        if (newState == currentState){
            return;
        }
        // Else, change the state and emit the signal
        currentState = newState;
        EmitSignal("stateChanged", currentState);
    }

    // Initializes the enemy (called from the enemy script)
    public void initialize(Enemy enemy){
        this.enemy = enemy;
    }

    // When a body enteres the engage area
    public void _on_EngageDetection_body_entered(Node2D body){
        // If it's a player, change the state and reference the player
        if (body.IsInGroup("player") && !enemy.squashed){
            setState((int)State.ENGAGE);
            player = (Player.Player)body;
        }

    }

    // When a body leaves the engage area
    public void _on_EngageDetection_body_exited(Node2D body){
        // If it's a player, changes the state
        if(body.IsInGroup("player") && !enemy.squashed){
            setState((int)State.CHASE);
            player = (Player.Player)body;
        }

    }


    // Catches state changed signal to change the animation of the enemy
    public void _on_AI_stateChanged(int state){
        switch(state){
            case (int)State.CHASE:
                enemy.playAnimation("Idle");
                break;
            
            case (int)State.ENGAGE:
                enemy.playAnimation("Attack");
                break;
            case (int)State.SQUASHED:
                enemy.playAnimation("Squash");
                enemy.squashed = true;
                break;

        }
    }

}
