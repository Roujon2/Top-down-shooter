using Godot;
using System;
using StateMachine;

namespace Player.States
{
    public class PlayerState : State
    {
        protected Player player;
        public override void _Ready()
        {
            base._Ready();

            player = Owner as Player;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            if(!player.hitCooldown.IsStopped()){
                player.chargingBar.MaxValue = player.hitCooldown.WaitTime;
                player.chargingBarSetValue(player.hitCooldown.TimeLeft);
            }
        }
    }
}
