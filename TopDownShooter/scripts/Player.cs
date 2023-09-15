using Godot;
using System;


namespace Player
{
    public class Player : KinematicBody2D
    {

        // Packed Scenes
        [Export] PackedScene Banana = (PackedScene)GD.Load("res://scenes/Banana.tscn");
        [Export] private int speed;
        [Export] private int bananaSpeed = 1100;
        [Export] private float hitCooldownDuration = 1.5f;


        public Vector2 Velocity { get; private set; }


        // Animation booleans
        public bool charging;
        public bool hitting;
        public bool canHit;
        public bool canShoot;
        public bool canCharge;

        private Stats stats;


        public int killCount {get; private set;}


        private Vector2 velocity = new Vector2();

        private Node2D visualNode;
        private CollisionShape2D collisionShape;

        private CollisionShape2D hitCollisionShape;

        private AnimationPlayer animationPlayer;
        public Timer hitCooldown;
        private Timer shootCooldown;
        public ProgressBar chargingBar;


        public override void _Ready()
        {
            animationPlayer = GetNode<AnimationPlayer>("VisualNode/AnimationPlayer");
            visualNode = GetNode<Node2D>("VisualNode");
            chargingBar = GetNode<ProgressBar>("VisualNode/ChargingBar");
            chargingBarSetValue(0.0);
            hitCooldown = GetNode<Timer>("HitCooldown");

            hitCollisionShape = GetNode<CollisionShape2D>("MeleeHitArea/HitCollisionShape");

            hitCollisionShape.Visible = false;
            hitCollisionShape.Disabled = true;
            hitCollisionSetScale(new Vector2(1, 1));

            stats = GetNode<Stats>("Stats");
            speed = stats.getSpeed();

            charging = false;
            hitting = false;
            canCharge = true;
            canShoot = true;

            killCount = 0;


        }

        public void addKill(){
            killCount += 1;

            GetTree().CallGroup("HUD", "enemyKilled");
        }

        public void hitCollisionSetScale(Vector2 scale){
            hitCollisionShape.Scale = scale;
        }

        public void hitCollisionVisibility(bool visible){
            hitCollisionShape.Visible = visible;
        }

        public void hitCollisionSetDisability(bool disabled){
            hitCollisionShape.Disabled = disabled;
        }


        private string animation;

        public string Animation
        {
            get => animation;
            set
            {
                if (animation == value) return;

                animation = value;
                animationPlayer.Play(animation);
            }
        }

        // Sets the value and colours of the charging bar
        public void chargingBarSetValue(double value){

            // If can hit, changes to red
            if(canHit){
                chargingBar.Modulate = Color.ColorN("RED", 0.5f);
            }else if(charging){
                chargingBar.Modulate = Color.Color8(71, 255, 72, 255);
            }
            else{
                chargingBar.Modulate = Color.Color8(255, 255, 255, 100);
            }

            // If not charging
            if (value == 0.0){
                chargingBar.Visible = false;
            }else{
                chargingBar.Visible = true;
                chargingBar.Value = value;
            }

        }

      

        /*
            // Physics processes
            public override void _PhysicsProcess(float delta)
            {
                // Detecting input and applying the velocity to player
                getInput();
                MoveAndSlide(velocity);
            }

            // Called when the node enters the scene tree for the first time.
            public override void _Ready()
            {

                // Stats reference
                stats = GetNode<Stats>("Stats");

                speed = stats.getSpeed();


                // Idle animation
                animationPlayer = GetNode<AnimationPlayer>("VisualNode/AnimationPlayer");
                playAnimation("Idle");

                // Collision reference
                collisionShape = GetNode<CollisionShape2D>("CollisionShape");

                // Setting up boolean states
                canShoot = true;
                charging = false;
                hitting = false;
                canHit = false;
                canCharge = true;

                // Charging/cooldown bar reference
                chargingBar = GetNode<ProgressBar>("VisualNode/ChargingBar");

                // Timer references
                hitCooldown = GetNode<Timer>("HitCooldown");
                shootCooldown = GetNode<Timer>("ShootCooldown");


                hitCooldown.WaitTime = hitCooldownDuration;


                // Resetting charging bar
                chargingBarSetValue(0.0);

                // Connecting signal for knowing when animations finish
                animationPlayer.Connect("animation_finished", this, "animationFinished");

            }

        */
        // Moves the kinematic body
        public void Move(Vector2 velocity)
        {
            velocity = velocity.Normalized() * speed;
            // If going left
            if (velocity.x < 0)
            {
                // Switch direction
                visualNode.Scale = new Vector2(-1, 1);
            }

            // If going right
            if (velocity.x > 0)
            {
                // Switch direction
                visualNode.Scale = new Vector2(1, 1);
            }

            Velocity = MoveAndSlide(velocity);
        }


        /*
            public void getInput()
            {

                // Resetting velocity vector
                velocity = Vector2.Zero;

                // Getting visual node reference
                visualNode = GetNode<Node2D>("VisualNode");


                // Cannot move if charging
                if (!charging)
                {
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

                        // Flipping visuals
                        visualNode.Scale = new Vector2(-1, 1);

                    }
                    if (Input.IsActionPressed("right"))
                    {
                        velocity.x += 1;

                        // Flipping visuals
                        visualNode.Scale = new Vector2(1, 1);

                    }

                    if(!hitting && velocity > Vector2.Zero){
                        playAnimation("Walk");
                    }
                }


                // When staying still
                if (velocity == Vector2.Zero && !charging && !hitting)
                {
                    playAnimation("Idle");
                }


                // If space is pressed
                if (Input.IsActionJustPressed("charge"))
                {
                    if (canCharge){
                        // Play charge animation and activate bool
                        playAnimation("Charge");
                        charging = true;
                        canShoot = false;

                        chargingBar.MaxValue = chargingDuration;
                        chargingBarSetValue(animationPlayer.CurrentAnimationPosition);
                    }
                }

                // If space is released
                if (Input.IsActionJustReleased("charge"))
                {
                    canShoot = true;
                    charging = false;

                    // Reset charging bar
                    chargingBarSetValue(0.0);

                    // If player charged for long enough
                    if (canHit)
                    {
                        // Hits
                        hitting = true;
                        playAnimation("Hit");

                        // Setting up hit cooldown timer and bar
                        chargingBar.MaxValue = hitCooldownDuration;
                        hitCooldown.Start();

                        canHit = false;
                        canCharge = false;
                    }
                    // Reset everything
                    else
                    {
                        hitting = false;
                        canHit = false;
                    }
                }

                // Normalizing vector for (diagonal) magnitude issues
                velocity = velocity.Normalized() * speed;

            }

            public void shoot()
            {
                // If shooting
                if(shootCooldown.IsStopped()){
                    // Create banana instance
                    var banana_instance = (Banana)Banana.Instance();

                    // Call method in banana class to ready animation
                    banana_instance.readyAnimation();

                    // Ready position and rotation depending on mouse position, adding it and applying impulse
                    banana_instance.Position = this.Position;
                    GetParent().AddChild(banana_instance);
                    banana_instance.Rotation = banana_instance.GlobalPosition.DirectionTo(GetGlobalMousePosition()).Angle();
                    banana_instance.ApplyImpulse(new Vector2(), new Vector2(bananaSpeed, 0).Rotated(banana_instance.Rotation));

                    // Starting cooldown timer
                    shootCooldown.Start();
                }
            }
        */

        public void shoot()
        {
        
            // Create banana instance
            var banana_instance = (Banana)Banana.Instance();

            // Call method in banana class to ready animation
            banana_instance.readyAnimation();

            // Ready position and rotation depending on mouse position, adding it and applying impulse
            banana_instance.Position = this.Position;
            GetParent().AddChild(banana_instance);
            banana_instance.Rotation = banana_instance.GlobalPosition.DirectionTo(GetGlobalMousePosition()).Angle();
            banana_instance.ApplyImpulse(new Vector2(), new Vector2(bananaSpeed, 0).Rotated(banana_instance.Rotation));

            
        }

        // When hit cooldown ends
        public void _on_HitCooldown_timeout(){
            chargingBar.Visible = false;
            canCharge = true;
        }

        public void playAnimation(String animationName)
        {
            animationPlayer.Play(animationName);
        }

        /*
            public void animationFinished(String animationName)
            {
                // Changes bool states depending on the animation that finished
                switch (animationName)
                {
                    case "Charge":
                        canHit = true;
                        break;
                    case "Hit":
                        hitting = false;

                        break;
                }
            }

            public override void _Process(float delta)
            {
                if(charging){
                    chargingBarSetValue(animationPlayer.CurrentAnimationPosition);
                }else if(!hitCooldown.IsStopped()){
                    chargingBarSetValue(hitCooldown.TimeLeft);
                }
            }

            // Sets the value and colours of the charging bar
            public void chargingBarSetValue(double value){

                // If can hit, changes to red
                if(canHit){
                    chargingBar.Modulate = Color.ColorN("RED", 0.5f);
                }else if(charging){
                    chargingBar.Modulate = Color.Color8(71, 255, 72, 255);
                }
                else{
                    chargingBar.Modulate = Color.Color8(255, 255, 255, 100);
                }

                // If not charging
                if (value == 0.0){
                    chargingBar.Visible = false;
                }else{
                    chargingBar.Visible = true;
                    chargingBar.Value = value;
                }

            }

            // When hit cooldown ends
            public void _on_HitCooldown_timeout(){
                chargingBar.Visible = false;
                canCharge = true;
            }

        */
        // Handles hit
        public void handleHit()
        {
            death();
        }


        public void death()
        {
            GetTree().ChangeScene("res://scenes/GameOver.tscn");
        }

        /*

            // Unhandled input for common input (keyclicks, etc)
            public override void _UnhandledInput(InputEvent @event)
            {
                if (@event.IsActionPressed("shoot") && canShoot)
                {
                    shoot();
                }
            }
            */
    }
}
