using Godot;
using System;

namespace Player.States
{
    public class ChargeState : PlayerState
    {

        private Timer chargeTimer;

        public override void _Ready()
        {
            base._Ready();

            chargeTimer = GetNode<Timer>("ChargeTimer");

            OnEnter += () =>
            {
                player.charging = true;
                chargeTimer.Start();

                player.hitCollisionVisibility(true);
                player.Animation = "Charge";
            };
            OnPhysicsProcess += PhysicsProcess;
            OnProcess += Process;
        }

        private void Process(float delta)
        {
            player.hitCollisionSetScale(new Vector2(-20*chargeTimer.TimeLeft+15, -20*chargeTimer.TimeLeft+15));
        }

        private void PhysicsProcess(float delta)
        {
            // Cancel movement
            var velocity = player.Velocity;
            velocity.x = 0;
            velocity.y = 0;

            player.Move(velocity);

            if (Input.IsActionJustReleased("charge"))
            {
                player.chargingBarSetValue(0.0);


                chargeTimer.Stop();

                player.charging = false;
                player.canHit = false;
                StateManager?.ChangeState("Hit");
            }

        }
    }
}
