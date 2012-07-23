using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlatformGame
{
    class LayerEnemy : LayerGameObject
    {
        private Level level;
        Animation enemyIdleRed;
        Animation enemyRunRed;
        Animation electr;
        static Random rangen=new Random();

        Animation bladeRun;
        Animation thorns;

        public LayerEnemy(ContentManager content) : base(content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            enemyRunRed = new Animation(content.Load<Texture2D>("Sprites/MonsterA/RunEnemy"), 0.1f, true);
            enemyIdleRed = new Animation(content.Load<Texture2D>("Sprites/MonsterA/Idle"), 0.15f, true);
            electr = new Animation(content.Load<Texture2D>("Sprites/fulmine"), 0.15f, true,32);

            bladeRun = new Animation(content.Load<Texture2D>("Sprites/Guill"), 0.1f, true);
            thorns = new Animation(content.Load<Texture2D>("Sprites/Thorns"), 0.1f, true);

            Initialize();
        }


        public void Initialize()
        {

            for (int i = 0; i < level.enemyList1.Count; i++)
            {
                if (level.enemyList1[i].type == EnemyType.Monster)
                {
                                level.enemyList1[i].sprite.PlayAnimation(enemyRunRed);
                        level.enemyList1[i].localBounds = new Rectangle(0, 0, (int)(48), (int)(48));
                }
                else if (level.enemyList1[i].type == EnemyType.Guillottine)
                {
                    level.enemyList1[i].sprite.PlayAnimation(bladeRun);
                    level.enemyList1[i].localBounds = new Rectangle(0, 0, 30, 30);
                }
                else if (level.enemyList1[i].type == EnemyType.Thorns)
                {
                    level.enemyList1[i].sprite.PlayAnimation(thorns);
                    level.enemyList1[i].localBounds = new Rectangle(0, 0, 32, 24);
                }
                else if (level.enemyList1[i].type == EnemyType.Electricity)
                {
                    level.enemyList1[i].sprite.PlayAnimation(electr,rangen.Next(3));
                    level.enemyList1[i].localBounds = new Rectangle(0, 0, 32, 0);
                }
            }

            for (int i = 0; i < level.enemyList2.Count; i++)
            {
                if (level.enemyList2[i].type == EnemyType.Monster)
                {
                        level.enemyList2[i].sprite.PlayAnimation(enemyRunRed);
                        level.enemyList2[i].localBounds = new Rectangle(0, 0, (int)(48), (int)(48));
                }
                else if (level.enemyList2[i].type == EnemyType.Guillottine)
                {
                    level.enemyList2[i].sprite.PlayAnimation(bladeRun);
                    level.enemyList2[i].localBounds = new Rectangle(0, 0, 30, 30);
                }
                else if (level.enemyList2[i].type == EnemyType.Thorns)
                {
                    level.enemyList2[i].sprite.PlayAnimation(thorns);
                    level.enemyList2[i].localBounds = new Rectangle(0, 0, 32, 24);
                }
                else if (level.enemyList2[i].type == EnemyType.Electricity)
                {
                    level.enemyList2[i].sprite.PlayAnimation(electr);
                    level.enemyList2[i].localBounds = new Rectangle(0, 0, 32, 0);
                }
            }
        }
        //this function move the enemies and check if player collide with one enemy
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (level.switchLevel == 1)
            {
                for (int i = 0; i < level.enemyList1.Count; i++)
                {
                        // Calculate tile position based on the side we are walking towards.
                        float posX = level.enemyList1[i].position.X;
                        int tileX = (int)Math.Round(posX / Tile.Width);
                        int tileY = (int)Math.Floor(level.enemyList1[i].position.Y / Tile.Height);

                        if (level.enemyList1[i].waitTime > 0)
                        {
                            if (level.enemyList1[i].type == EnemyType.Guillottine)
                            {
                                level.enemyList1[i].direction = (FaceDirection)(-(int)level.enemyList1[i].direction);
                                level.enemyList1[i].waitTime = 0;
                            }
                            else
                            {
                                // Wait for some amount of time.
                                level.enemyList1[i].waitTime = Math.Max(0.0f, level.enemyList1[i].waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);

                                if (level.enemyList1[i].waitTime <= 0.0f)
                                {
                                    // Then turn around.
                                    level.enemyList1[i].direction = (FaceDirection)(-(int)level.enemyList1[i].direction);
                                    if (level.enemyList1[i].type == EnemyType.Monster)
                                                level.enemyList1[i].sprite.PlayAnimation(enemyRunRed);                                                
                                }
                            }
                        }
                        else
                        {
                            switch (level.enemyList1[i].type)
                            {
                                case EnemyType.Monster:
                                    {
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.GetCollision(tileX + (int)level.enemyList1[i].direction, tileY) == TileCollision.Impassable ||
                                            level.GetCollision(tileX + (int)level.enemyList1[i].direction, tileY - 1) == TileCollision.Impassable
                                            || level.GetCollision(tileX + (int)level.enemyList1[i].direction, tileY + 1) == TileCollision.Passable)
                                        {
                                            level.enemyList1[i].waitTime = level.enemyList1[i].MaxWaitTime;
                                            level.enemyList1[i].sprite.PlayAnimation(enemyIdleRed);
                                        }
                                        else
                                        {
                                            //   Move in the current direction.
                                            Vector2 velocity = new Vector2((int)level.enemyList1[i].direction * level.enemyList1[i].MoveSpeed * elapsed, 0.0f);
                                            level.enemyList1[i].position = level.enemyList1[i].position + velocity;
                                        }
                                        break;
                                    }
                                case EnemyType.Guillottine:
                                    {
                                        int checkPosition;
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.enemyList1[i].direction == FaceDirection.Left)
                                            checkPosition = (int)level.enemyList1[i].direction;
                                        else
                                            checkPosition = (int)level.enemyList1[i].direction - 1;

                                        if (level.GetCollision(tileX, tileY - checkPosition) == TileCollision.Impassable)
                                        {
                                            level.enemyList1[i].waitTime = level.enemyList1[i].MaxWaitTime;
                                        }
                                        else
                                        {
                                            // Move in the current direction.
                                            Vector2 velocity = new Vector2(0.0f, (int)(level.enemyList1[i].direction) * level.enemyList1[i].MoveSpeed * elapsed);
                                            level.enemyList1[i].position = level.enemyList1[i].position - velocity;
                                        }
                                        break;
                                    }


                                case EnemyType.Electricity:
                                    {
                                        int checkPosition;
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.enemyList1[i].direction == FaceDirection.Left)
                                            checkPosition = (int)level.enemyList1[i].direction;
                                        else
                                            checkPosition = (int)level.enemyList1[i].direction - 1;

                                        if (level.enemyList1[i].localBounds.Height > 0)
                                        {
                                            if (level.enemyList1[i].timeActive <= 0)
                                            {
                                                level.enemyList1[i].waitTime = level.enemyList1[i].temporization;
                                                level.enemyList1[i].localBounds.Height = 0;
                                            }
                                            else
                                                level.enemyList1[i].timeActive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                                        }
                                        else
                                        {
                                            level.enemyList1[i].localBounds.Height = Tile.Height * 3;
                                            level.enemyList1[i].timeActive = 1.0f;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (level.playerLevel.BoundingRectangle.Intersects(level.enemyList1[i].BoundingRectangle))
                        {
                            level.playerLevel.isAlive = false;
                            level.playerLevel.dieTime = 3.0f;
                            level.menuDieTime = 1.0f;
                            level.soundList.Add(SoundType.diePlayer);
                        }
                    }
                }
            else
            {
                for (int i = 0; i < level.enemyList2.Count; i++)
                {
                        // Calculate tile position based on the side we are walking towards.
                        float posX = level.enemyList2[i].position.X;
                        int tileX = (int)Math.Round(posX / Tile.Width);
                        int tileY = (int)Math.Floor(level.enemyList2[i].position.Y / Tile.Height);

                        if (level.enemyList2[i].waitTime > 0)
                        {
                            if (level.enemyList2[i].type == EnemyType.Guillottine)
                            {
                                level.enemyList2[i].direction = (FaceDirection)(-(int)level.enemyList2[i].direction);
                                level.enemyList2[i].waitTime = 0;
                            }
                            else
                            {
                                // Wait for some amount of time.
                                level.enemyList2[i].waitTime = Math.Max(0.0f, level.enemyList2[i].waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);

                                if (level.enemyList2[i].waitTime <= 0.0f)
                                {
                                    // Then turn around.
                                    level.enemyList2[i].direction = (FaceDirection)(-(int)level.enemyList2[i].direction);
                                    if (level.enemyList2[i].type == EnemyType.Monster)
                                        
                                                level.enemyList2[i].sprite.PlayAnimation(enemyRunRed);
                                        
                                }
                            }
                        }
                        else
                        {
                            switch (level.enemyList2[i].type)
                            {
                                case EnemyType.Monster:
                                    {
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.GetCollision(tileX + (int)level.enemyList2[i].direction, tileY) == TileCollision.Impassable ||
                                            level.GetCollision(tileX + (int)level.enemyList2[i].direction, tileY - 1) == TileCollision.Impassable
                                            || level.GetCollision(tileX + (int)level.enemyList2[i].direction, tileY + 1) == TileCollision.Passable)
                                        {
                                            level.enemyList2[i].waitTime = level.enemyList2[i].MaxWaitTime;
                                            level.enemyList2[i].sprite.PlayAnimation(enemyIdleRed);
                                        }
                                        else
                                        {
                                            //   Move in the current direction.
                                            Vector2 velocity = new Vector2((int)level.enemyList2[i].direction * level.enemyList2[i].MoveSpeed * elapsed, 0.0f);
                                            level.enemyList2[i].position = level.enemyList2[i].position + velocity;
                                        }
                                        break;
                                    }
                                case EnemyType.Guillottine:
                                    {
                                        int checkPosition;
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.enemyList2[i].direction == FaceDirection.Left)
                                            checkPosition = (int)level.enemyList2[i].direction;
                                        else
                                            checkPosition = (int)level.enemyList2[i].direction - 1;

                                        if (level.GetCollision(tileX, tileY - checkPosition) == TileCollision.Impassable)
                                        {
                                            level.enemyList2[i].waitTime = level.enemyList2[i].MaxWaitTime;
                                        }
                                        else
                                        {
                                            // Move in the current direction.
                                            Vector2 velocity = new Vector2(0.0f, (int)(level.enemyList2[i].direction) * level.enemyList2[i].MoveSpeed * elapsed);
                                            level.enemyList2[i].position = level.enemyList2[i].position - velocity;
                                        }
                                        break;
                                    }

                                case EnemyType.Electricity:
                                    {
                                        int checkPosition;
                                        // If we are about to run into a wall or off a cliff, start waiting.
                                        if (level.enemyList2[i].direction == FaceDirection.Left)
                                            checkPosition = (int)level.enemyList2[i].direction;
                                        else
                                            checkPosition = (int)level.enemyList2[i].direction - 1;

                                        if (level.enemyList2[i].localBounds.Height > 0)
                                        {
                                            if (level.enemyList2[i].timeActive <= 0)
                                            {
                                                level.enemyList2[i].waitTime = level.enemyList1[i].temporization;
                                                level.enemyList2[i].localBounds.Height = 0;
                                            }
                                            else
                                                level.enemyList2[i].timeActive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                                        }
                                        else
                                        {
                                            // Move in the current direction.
                                            //Vector2 velocity = new Vector2(0.0f, (int)(level.enemyList1[i].direction) * level.enemyList1[i].MoveSpeed * elapsed);
                                            //level.enemyList1[i].position = level.enemyList1[i].position - velocity;

                                            level.enemyList2[i].localBounds.Height = Tile.Height * 3;
                                            level.enemyList2[i].timeActive = 1.0f;
                                        }
                                        break;
                                    }
                            }

                        //check if player collide with enemies
                        if (level.playerLevel.BoundingRectangle.Intersects(level.enemyList2[i].BoundingRectangle))
                        {
                            level.playerLevel.isAlive = false;
                            level.playerLevel.dieTime = 3.0f;
                            level.menuDieTime = 1.0f;
                            level.soundList.Add(SoundType.diePlayer);
                        }
                    }
                }
            }
            base.Update(gameTime);
        }


        //update service when load the next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            this.Initialize();
            base.UpdateLevelService(content);
        }

        public void ResetEnemyes()
        {
            for (int i = 0; i < level.enemyList1.Count; i++)
            {
                level.enemyList1[i].position = level.enemyList1[i].start;
            
            }

            for (int i = 0; i < level.enemyList2.Count; i++)
            {
                level.enemyList2[i].position = level.enemyList2[i].start;
            }
        
        }

    }
}
