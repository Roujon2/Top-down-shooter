using Godot;
using System;


namespace Player.States
{
    public class HitState : PlayerState
    {
        private Timer hitTimer;

        public override void _Ready()
        {
            base._Ready();

            hitTimer = GetNode<Timer>("HitTimer");

            OnEnter += () =>
            {
                player.hitting = true;
                player.canCharge = false;
                
                player.hitCollisionSetDisability(false);

                hitTimer.Start();
                player.hitCooldown.Start();
                player.Animation = "Hit";
            };
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

            player.Move(velocity);

            if (Input.IsActionJustPressed("shoot"))
            {
                player.shoot();
            }

            if (!player.hitting)
            {
                if (velocity == Vector2.Zero)
                {
                    StateManager?.ChangeState("Idle");
                }else{
                    StateManager?.ChangeState("Walk");
                }
            
            }

        }


        public void _on_HitTimer_timeout()
        {
            player.hitting = false;
            player.hitCollisionVisibility(false);
            player.hitCollisionSetScale(new Vector2(1, 1));
            player.hitCollisionSetDisability(true);

        }

        public void _on_MeleeHitArea_body_entered(Node2D body){
            if(body is Enemy enemy){
                enemy.handleSquash();
            }
        }



    }
}
