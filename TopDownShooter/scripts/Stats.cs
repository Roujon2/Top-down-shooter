using Godot;
using System;

public class Stats : Node2D
{
    [Export]
    private int health;
    [Export]
    private int speed;

    public int getHealth(){
        return health;
    }

    public int getSpeed(){
        return speed;
    }

    public void setHealth(int newHealth){
        health = newHealth;
    }

    public void setSpeed(int newSpeed){
        speed = newSpeed;
    }
}
