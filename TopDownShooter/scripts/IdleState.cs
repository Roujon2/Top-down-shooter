using Godot;
using System;
using StateMachine;

namespace Player.States
{
    public class IdleState : PlayerState
    {
        public override void _Ready()
        {

            base._Ready();

            
            OnEnter += () => { player.Animation = "Idle"; };
            OnPhysicsProcess += PhysicsProcess;
            
        }

        private void PhysicsProcess(float delta)
        {
            var velocity = player.Velocity;
            velocity.x = 0;
            velocity.y = 0;

            player.Move(velocity);

            var right = Input.IsActionPressed("right");
            var left = Input.IsActionPressed("left");
            var up = Input.IsActionPressed("up");
            var down = Input.IsActionPressed("down");

            if(right || left || up || down){
              StateManager?.ChangeState("Walk");
            }
            if(Input.IsActionJustPressed("charge") && player.canCharge){
                StateManager?.ChangeState("Charge");
            }
            if(Input.IsActionJustPressed("shoot")){
                player.shoot();
            }


        }
    }
}
