using Godot;
using System;
using StateMachine;

namespace Player.States
{
    public class WalkState : PlayerState
    {
        public override void _Ready()
        {

            base._Ready();

            OnEnter += () => { player.Animation = "Walk"; };
            OnPhysicsProcess += PhysicsProcess;
        }

        private void PhysicsProcess(float delta)
        {   
            var velocity = new Vector2();

            // Movement keys adding to velocity vector
            if (Input.IsActionPressed("up"))
            {
                velocity.y -= 1;
            }
            if (Input.IsActionPressed("down"))
            {
                velocity.y += 1;
            }
            if (Input.IsActionPressed("left"))
            {
                velocity.x -= 1;
            }
            if (Input.IsActionPressed("right"))
            {
                velocity.x += 1;
            }

            if (velocity == Vector2.Zero){
                StateManager?.ChangeState("Idle");
            }
            if(Input.IsActionJustPressed("charge") && player.canCharge){
                StateManager?.ChangeState("Charge");
            }
            if(Input.IsActionJustPressed("shoot")){
                player.shoot();
            }

            player.Move(velocity);
        }
    }
}
