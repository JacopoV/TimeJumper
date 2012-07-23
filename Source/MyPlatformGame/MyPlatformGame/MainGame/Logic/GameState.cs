using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using LevelData;

namespace MyPlatformGame
{
    class GameState : LayerPlayer
    {
        private Level level;


        public GameState(ContentManager content)
            :base(content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
        }


        //update input
        public void Update(GameTime gameTime)
        {
            UpdateInput();
            spaceBarCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!level.storyMode)
                base.Update(gameTime);
        }
        
        //get input and modify movement value of player
        float spaceBarMaxCooldown = 0.3f;
        float spaceBarCooldown = 0.3f;
        public void UpdateInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (level.playerLevel.isAlive == false)
            {
                if (keyboardState.IsKeyDown(Keys.Space) && spaceBarCooldown<0)
                {
                    Restart();
                    spaceBarCooldown = spaceBarMaxCooldown;
                }
            }
            else if (!level.storyMode)
            {
                if (!level.playerLevel.onExit)
                {
                    if (keyboardState.IsKeyDown(Keys.Space) && spaceBarCooldown < 0)
                    {
                        if (level.GetSwitchCollision())
                        {
                            if(level.switchLevel == 1)
                                level.soundList.Add(SoundType.goodSwitch);
                            level.switchLevel = 2;
                        }
                        else
                        {
                            level.soundList.Add(SoundType.badSwitch);
                            level.errorSwitch = 0.1f;
                        }
                    }
                }

                // Ignore small movements to prevent running in place.
                if (Math.Abs(level.playerLevel.movement) < 0.5f)
                    level.playerLevel.movement = 0.0f;

                //// If any digital horizontal movement input is found, override the analog movement.
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    level.playerLevel.movement = -1.0f;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    level.playerLevel.movement = 1.0f;
                }
                else if (keyboardState.IsKeyDown(Keys.A))
                {
                    level.playerLevel.movement = -0.4f;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    level.playerLevel.movement = 0.3f;
                }


                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    level.playerLevel.isJumping = true;
                }
                else
                {
                    level.playerLevel.isJumping = false;
                }

                if (level.playerLevel.onExit == true)
                {
                      if (keyboardState.IsKeyDown(Keys.Space) && spaceBarCooldown < 0)
                    {
                        spaceBarCooldown = spaceBarMaxCooldown;
                        level.playerLevel.exit = true;
                    }
                }

                    if (keyboardState.IsKeyDown(Keys.Tab))
                    {
                        level.playerLevel.exit = true;
                    }

            }
            else if (keyboardState.IsKeyDown(Keys.Space) && spaceBarCooldown < 0)
            {
                spaceBarCooldown = spaceBarMaxCooldown;
                level.storyMode = false;
                level.soundList.Add(SoundType.musicPlay);
                level.soundList.Add(SoundType.storyStop);
            }
        }

        //update service whene load the next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            base.UpdateLevelService(content);
        
        }
    }
}
