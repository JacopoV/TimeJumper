using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlatformGame
{
    class LayerPlayer : LayerEnemy
    {
        // Constants for controling horizontal movement
        private const float MoveAcceleration = 5000.0f;
        private const float MaxMoveSpeed = 6000.0f;
        private const float GroundDragFactor = 0.58f * 1.25f;
        private const float AirDragFactor = 0.58f * 1.25f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -2000.0f;
        private const float GravityAcceleration = 2000.0f;
        private const float MaxFallSpeed = 400.0f;
        private const float JumpControlPower = 0.16f;

        private Level level;
        //private LayerAnimation layerAnimation;

        //Animation
        private Animation playerIdle;
        private Animation playerRun;
        private Animation playerWalk;
        private Animation playerJump;
        private Animation playerWin;
        private Animation playerDie;
        private Animation playerAttack;


        public LayerPlayer(ContentManager content)
            :base(content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            playerIdle = new Animation(content.Load<Texture2D>("Sprites/Player/Idle"), 0.1f, true);
            playerRun = new Animation(content.Load<Texture2D>("Sprites/Player/player"), 0.03f, true);
            playerWalk = new Animation(content.Load<Texture2D>("Sprites/Player/player"), 0.07f, true);
            playerJump = new Animation(content.Load<Texture2D>("Sprites/Player/jumpPlayer"), 0.1f, false);
            playerWin = new Animation(content.Load<Texture2D>("Sprites/Player/player"), 0.1f, false);
            playerDie = new Animation(content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false);

            //set default animation sprite
            level.playerLevel.sprite.PlayAnimation(playerIdle);
        }

        
        //This manage the player movement and check the input for the player
        public void Update(GameTime gameTime)
        {
            if (level.playerLevel.onExit)
            {
                level.playerLevel.sprite.PlayAnimation(playerWin);
                return;
            }

            if(level.errorSwitch > 0)
                level.errorSwitch -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (level.timeRemaining <= 0 && level.playerLevel.isAlive)
            {
                level.playerLevel.isAlive = false;
                level.playerLevel.dieTime = 3.0f;
                level.menuDieTime = 1.0f;
                level.soundList.Add(SoundType.diePlayer);
            }

            //check if player is die
            if (!level.playerLevel.isAlive)
            {
                if (level.menuDieTime >= 0.0f)
                {
                    level.playerLevel.sprite.PlayAnimation(playerDie);
                    level.menuDieTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                if (level.playerLevel.dieTime >= 0.0f)
                {
                    level.playerLevel.dieTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                ApplyPhysics(gameTime);

                if (level.playerLevel.isAlive && level.playerLevel.isOnGround)
                {
                    if (Math.Abs(level.playerLevel.velocity.X) > 0.02f)
                    {
                        //if (Math.Abs(level.playerLevel.velocity.X) > 200)
                            level.playerLevel.sprite.PlayAnimation(playerRun);
                        //else
                        //    level.playerLevel.sprite.PlayAnimation(playerWalk);
                    }
                    else
                    {
                        level.playerLevel.sprite.PlayAnimation(playerIdle);
                    }
                }

                //clear movement
                level.playerLevel.movement = 0.0f;
                level.playerLevel.isJumping = false;

                base.Update(gameTime);
            }
        }

        // Updates the player's velocity and position based on input, gravity, etc.
        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = level.playerLevel.position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            level.playerLevel.velocity.X += level.playerLevel.movement * MoveAcceleration * elapsed;
            level.playerLevel.velocity.Y = MathHelper.Clamp(level.playerLevel.velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            level.playerLevel.velocity.Y = DoJump(gameTime, level.playerLevel.velocity.Y);

            // Apply pseudo-drag horizontally.
            if (level.playerLevel.isOnGround)
                level.playerLevel.velocity.X *= GroundDragFactor;
            else
                level.playerLevel.velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            level.playerLevel.velocity.X = MathHelper.Clamp(level.playerLevel.velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            level.playerLevel.position += level.playerLevel.velocity * elapsed;
            level.playerLevel.position = new Vector2((float)Math.Round(level.playerLevel.position.X), (float)Math.Round(level.playerLevel.position.Y));

            // If the player is now colliding with the level, separate them.
            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (level.playerLevel.position.X == previousPosition.X)
                level.playerLevel.velocity.X = 0;

            if (level.playerLevel.position.Y == previousPosition.Y)
                level.playerLevel.velocity.Y = 0;
        }

        //this function manage the jump of the player
        private float DoJump(GameTime gameTime, float velocityY)
        {            
            // If the player wants to jump
            if (level.playerLevel.isJumping)
            {
                // Begin or continue a jump
                if ((!level.playerLevel.wasJumping && level.playerLevel.isOnGround) || level.playerLevel.jumpTime > 0.0f)
                {
                    level.playerLevel.jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    level.playerLevel.sprite.PlayAnimation(playerJump);
                    if (!level.playerLevel.wasJumping && level.playerLevel.isOnGround)
                    {
                        level.soundList.Add(SoundType.jump);
                    }
                }

                // If we are in the ascent of the jump
                if (0.0f < level.playerLevel.jumpTime && level.playerLevel.jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(level.playerLevel.jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    level.playerLevel.jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                level.playerLevel.jumpTime = 0.0f;
            }

            level.playerLevel.wasJumping = level.playerLevel.isJumping;

            return velocityY;
        }

        //manage the collision
        private void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = level.playerLevel.BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            // Reset flag to search for ground collision.
            level.playerLevel.isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY <= absDepthX)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (level.playerLevel.previousBottom <= tileBounds.Top)
                                    level.playerLevel.isOnGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || level.playerLevel.isOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    level.playerLevel.position = new Vector2(level.playerLevel.position.X, level.playerLevel.position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = level.playerLevel.BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                //Console.WriteLine("Collision detected:"+level.playerLevel.position);
                                level.playerLevel.position = new Vector2(level.playerLevel.position.X + depth.X, level.playerLevel.position.Y);
                                //Console.WriteLine("Collision evaluated:" + level.playerLevel.position);

                                // Perform further collisions with the new bounds.
                                bounds = level.playerLevel.BoundingRectangle;
                            }
                        }
                    }
                }
            }

            //check if player goes down in a hole
            if (level.playerLevel.position.Y > (level.lenghtY + 3) * Tile.Height)
            {
                level.playerLevel.sprite.PlayAnimation(playerDie);
                level.playerLevel.isAlive = false;
                level.soundList.Add(SoundType.diePlayer);
            }

            // Save the new bounds bottom.
            level.playerLevel.previousBottom = bounds.Bottom;
        }
        
        //update the service when load a next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            base.UpdateLevelService(content);

        }

        //this function restart a level and decrement life number
        public void Restart()
        {
            level.playerLevel.velocity = Vector2.Zero;
            level.playerLevel.position = level.playerLevel.start;
            level.playerLevel.isAlive = true;
            level.playerLevel.sprite.PlayAnimation(playerIdle);
            level.switchLevel = 1;
            level.playerLevel.openDoorA = false;
            level.playerLevel.openDoorB = false;
            level.playerLevel.openDoorC = false;
            level.timeRemaining = 60;
            level.soundList.Add(SoundType.musicRestart);
            base.ResetObject();
            base.ResetEnemyes();
        }
    }
}
