using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using MyPlatformGame.Resources;

namespace MyPlatformGame
{
    class RenderingState: RenderingBlocks
    {
        Level level;

        Texture2D background1l1, background1l2, background2l1, background2l2;
        Texture2D infoDie;
        Texture2D infoWin;
        Texture2D infoStory;
        Texture2D imageStory;
        Texture2D levelBadBuzz;
        Texture2D keyA;
        Texture2D keyB;
        Texture2D keyC;
        Texture2D hud;
        SpriteFont font;
        SpriteFont fontStory;
        Effect effect;
        public RenderTarget2D rtarget;



        ParticleSample.SmokePlumeParticleSystem smokeParticles;
        const float TimeBetweenSmokePlumePuffs = 1.0f;
        float timeTillPuff = 0.0f;

        Vector2 position = Vector2.Zero;


        public RenderingState() { }
        public RenderingState(ContentManager Content,Game game)
            : base(Content,game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;
            background2l1 = Content.Load<Texture2D>("Backgrounds/skyline_present_bkg");
            if (level.numLevel == 0)
            {
                background2l2 = Content.Load<Texture2D>("Backgrounds/tutorial");
            }
            else
            {
                background2l2 = Content.Load<Texture2D>("Backgrounds/skyline_present_bkg1");
            }
            background1l1 = Content.Load<Texture2D>("Backgrounds/skyline_future_bkg");
            background1l2 = Content.Load<Texture2D>("Backgrounds/skyline_future_bkg1");
            keyA = Content.Load<Texture2D>("Sprites/handle1");
            keyB = Content.Load<Texture2D>("Sprites/handle2");
            keyC = Content.Load<Texture2D>("Sprites/handle3");
            hud = Content.Load<Texture2D>("Textures/HUD");

            effect = Content.Load<Effect>("Effects/Tone");

            smokeParticles = new ParticleSample.SmokePlumeParticleSystem(game, 100);
            smokeParticles.Initialize();

            font = Content.Load<SpriteFont>("Fonts/Hud");
            fontStory = Content.Load<SpriteFont>("Fonts/storyFont");
            infoDie = Content.Load<Texture2D>("Textures/LOOSE");
            infoWin = Content.Load<Texture2D>("Textures/WIN");
            imageStory = Content.Load<Texture2D>("Textures/storyImage0");
            levelBadBuzz = Content.Load<Texture2D>("Textures/levelBad"+level.numLevel.ToString());
        }

        public void Draw(GameTime gameTime, SpriteBatch sp)
        {
            if (level.storyMode)
            {
                String levelString = GetLevel(level.numLevel);
                if (position == Vector2.Zero)
                     position = new Vector2(sp.GraphicsDevice.Viewport.Width / 2 - 140, (int)(sp.GraphicsDevice.Viewport.Height / 2));

                sp.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend);

                sp.Draw(imageStory, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                position.Y -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 50);
                string[] strings=levelString.Replace("\r","").Split('\n');
                for (int i = 0; i < strings.Length;i++ )
                {
                    if((position.Y + i * 32)>256.0f)
                        sp.DrawString(fontStory, strings[i], new Vector2(position.X, position.Y + i * 32), Color.WhiteSmoke);
                }
                sp.End();
            }
            else
            {

                sp.GraphicsDevice.SetRenderTarget(rtarget);
                position = Vector2.Zero;
                //draw background
                sp.Begin();
                if (level.switchLevel == 2)
                {
                    sp.Draw(background1l1, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                    sp.End();
                    timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeTillPuff < 0)
                    {
                        smokeParticles.AddParticles(new Vector2(0.12f * sp.GraphicsDevice.PresentationParameters.BackBufferWidth, 0.52f * sp.GraphicsDevice.PresentationParameters.BackBufferHeight));
                        smokeParticles.AddParticles(new Vector2(0.6f * sp.GraphicsDevice.PresentationParameters.BackBufferWidth, 0.43f * sp.GraphicsDevice.PresentationParameters.BackBufferHeight));
                        timeTillPuff = TimeBetweenSmokePlumePuffs;
                    }
                    smokeParticles.Update(gameTime);
                    smokeParticles.Draw(gameTime);

                    sp.Begin();
                    sp.Draw(background1l2, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                }
                else
                {
                    sp.Draw(background2l1, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                    sp.End();
                    timeTillPuff -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeTillPuff < 0)
                    {
                        smokeParticles.AddParticles(new Vector2(0.12f * sp.GraphicsDevice.PresentationParameters.BackBufferWidth, 0.52f * sp.GraphicsDevice.PresentationParameters.BackBufferHeight));
                        smokeParticles.AddParticles(new Vector2(0.6f * sp.GraphicsDevice.PresentationParameters.BackBufferWidth, 0.43f * sp.GraphicsDevice.PresentationParameters.BackBufferHeight));
                        timeTillPuff = TimeBetweenSmokePlumePuffs;
                    }
                    smokeParticles.Update(gameTime);
                    sp.Begin();
                    sp.Draw(background2l2, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                    if (level.errorSwitch > 0)
                    {
                        sp.Draw(levelBadBuzz, new Rectangle(0, 0, sp.GraphicsDevice.Viewport.Width, sp.GraphicsDevice.Viewport.Height), Color.White);
                    }
                }

                base.Draw(gameTime, sp, new Vector2(0,32), new Vector2(0,32));
                sp.End();


                sp.GraphicsDevice.SetRenderTarget(null);
                sp.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                effect.Parameters["Tone"].SetValue(1 - level.timeRemaining / level.maxTimeRemaining);
                effect.Parameters["hwRatio"].SetValue(sp.GraphicsDevice.PresentationParameters.BackBufferWidth * 1.0f / sp.GraphicsDevice.PresentationParameters.BackBufferHeight);
                Rectangle playerRect = level.playerLevel.BoundingRectangle;
                effect.Parameters["Position"].SetValue(new Vector2(playerRect.Center.X * 1.0f / sp.GraphicsDevice.PresentationParameters.BackBufferWidth, playerRect.Center.Y * 1.0f / sp.GraphicsDevice.PresentationParameters.BackBufferHeight));
                effect.CurrentTechnique.Passes[0].Apply();
                sp.Draw(rtarget, Vector2.Zero, Color.White);
                sp.End();
                if (level.playerLevel.isAlive == false && level.menuDieTime <= 0.0f)
                {
                    sp.Begin();
                    sp.Draw(infoDie, new Rectangle(sp.GraphicsDevice.Viewport.Width / 2 - infoDie.Width / 2,
                                                   sp.GraphicsDevice.Viewport.Height / 2 - infoDie.Height / 2,
                                                   infoDie.Width, infoDie.Height), Color.White);
                    sp.End();
                    level.menuDieTime = 0.0f;
                }



                if (level.playerLevel.onExit == true)
                {
                    sp.Begin();
                    sp.Draw(infoWin, new Rectangle(sp.GraphicsDevice.Viewport.Width / 2 - infoWin.Width / 2,
                                                   sp.GraphicsDevice.Viewport.Height / 2 - infoWin.Height / 2,
                                                   infoWin.Width, infoWin.Height), Color.White);
                    sp.End();
                }
            }
        }

        public void DrawHud(SpriteBatch sp)
        {

            //draw the statistic
            Rectangle titleSafeArea = sp.GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);

            sp.Begin();

            sp.Draw(hud, new Rectangle(sp.GraphicsDevice.Viewport.Width/2 - 250, -10, 500, 60), Color.White);

            sp.DrawString(font, "TIME REMAINING: " + ((int)(level.timeRemaining)).ToString() + "s", hudLocation + new Vector2(sp.GraphicsDevice.Viewport.Width / 2 - 185, 10), Color.Black);
            sp.DrawString(font, "LEVEL: " + ((int)(level.numLevel + 1)).ToString(), new Vector2(sp.GraphicsDevice.Viewport.Width / 2 - 50, sp.GraphicsDevice.Viewport.Height - 20), Color.White);
            sp.DrawString(font, "KEYS: ", hudLocation + new Vector2(sp.GraphicsDevice.Viewport.Width / 2 + 40, 10), Color.Black);

            if (level.playerLevel.openDoorA)
                sp.Draw(keyA, hudLocation + new Vector2(sp.GraphicsDevice.Viewport.Width / 2 + 100, 10), Color.White);
            if (level.playerLevel.openDoorB)
                sp.Draw(keyB, hudLocation + new Vector2(sp.GraphicsDevice.Viewport.Width / 2 + 130, 10), Color.White);
            if (level.playerLevel.openDoorC)
                sp.Draw(keyC, hudLocation + new Vector2(sp.GraphicsDevice.Viewport.Width / 2 + 150, 10), Color.White);
            sp.End();

        }

        //update the service when load a next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            levelBadBuzz = content.Load<Texture2D>("Textures/levelBad" + level.numLevel.ToString());
            if(level.numLevel > 0)
                background2l2 = content.Load<Texture2D>("Backgrounds/skyline_present_bkg1");
            base.UpdateLevelService(content);

        }

        private string GetLevel(int i)
        {
            switch (i)
            { 
                case 0:
                    return Strings.level0;
                case 1:
                    return Strings.level1;
                case 2:
                    return Strings.level2;
                case 3:
                    return Strings.level3;
                case 4:
                    return Strings.level4;
                case 5:
                    return Strings.level5;
                case 6:
                    return Strings.level6;
                default:
                    return "";
            }
        }
    }
}

