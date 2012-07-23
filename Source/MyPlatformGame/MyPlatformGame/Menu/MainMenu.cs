using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;
using MyPlatformGame.Resources;


namespace MyPlatformGame.Menu
{
    public class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Rectangle res_rect, new_gam_rect, credits_rect, back_rect;
        Vector2 res_pos, new_gam_pos, credits_pos, back_pos;
        SpriteFont font, fontCredits;
        SoundEffect click;
        SoundEffectInstance song;
        Texture2D background;

        public MainMenu(MainGame game)
            : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        public override void Initialize()
        {
            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/menuFont");
            fontCredits = Game.Content.Load<SpriteFont>("Fonts/levelFont");
            background = Game.Content.Load<Texture2D>("MenuTexture/backgroundMenu");
            click = Game.Content.Load<SoundEffect>("Sounds/Menu/click");
            song = Game.Content.Load<SoundEffect>("Sounds/Menu/menu").CreateInstance();
            song.IsLooped = true;
            song.Play();

            var midd_w = GraphicsDevice.Viewport.Width;

            res_rect = new Rectangle(midd_w - (int)(font.MeasureString(Strings.NewGame).X) - 30,
                200, (int)font.MeasureString(Strings.ResumeGame).X, (int)font.MeasureString(Strings.ResumeGame).Y);
            res_pos = new Vector2(res_rect.X, res_rect.Y);

            new_gam_rect = new Rectangle(midd_w - (int)(font.MeasureString(Strings.NewGame).X) - 30,
                270, (int)font.MeasureString(Strings.NewGame).X, (int)font.MeasureString(Strings.NewGame).Y);
            new_gam_pos = new Vector2(new_gam_rect.X, new_gam_rect.Y);

            credits_rect = new Rectangle(80,
               400, (int)font.MeasureString(Strings.Credits).X, (int)font.MeasureString(Strings.Credits).Y);
            credits_pos = new Vector2(credits_rect.X, credits_rect.Y);

            back_rect = new Rectangle(midd_w - (int)(font.MeasureString(Strings.NewGame).X) - 30,
               370, (int)font.MeasureString(Strings.Exit).X, (int)font.MeasureString(Strings.Exit).Y);
            back_pos = new Vector2(back_rect.X, back_rect.Y);

            base.LoadContent();
        }

        string savePath = "currentGame.sav";

        bool saveGameExists { get { return IS.FileExists(savePath); } }

#if WINDOWS_PHONE
        IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForApplication();
#endif
#if WINDOWS
        IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForDomain();
#endif

#if WINDOWS_PHONE
        public override void Update(GameTime gameTime)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.Tap)
                {
                    var pos = new Point((int)gs.Position.X, (int)gs.Position.Y);
                    if (res_rect.Contains(pos))
                    {
                        if (ResumeGame != null && saveGameExists)
                        {
                            click.Play();
                            song.Stop();
                            ResumeGame();
                        }
                    }
                    else if (new_gam_rect.Contains(pos))
                    {
                        if (NewGame != null)
                        {
                            click.Play();
                            NewGame();
                        }
                    }
                    else if (help_rect.Contains(pos))
                    {
                        if (Help != null)
                        {
                            click.Play();
                            Help();
                        }
                    }
                    else if (credits_rect.Contains(pos))
                    {
                        if (Credits != null)
                        {
                            click.Play();
                            Credits();
                        }
                    }
                    else if (back_rect.Contains(pos))
                     {
                        if (Exit != null)
                        {
                            click.Play();
                            Exit();
                        }
                    }
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (Exit != null)
                {
                    click.Play();
                    song.Stop();
                    Exit();
                }

            base.Update(gameTime);
        }
#endif

#if WINDOWS
        public override void Update(GameTime gameTime)
        {

            if (song.State == SoundState.Paused)
                song.Play();


                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                    if (res_rect.Contains(pos))
                    {
                        if (ResumeGame != null && saveGameExists)
                        {
                            click.Play();
                            song.Stop();
                            ResumeGame();
                        }
                    }
                    else if (new_gam_rect.Contains(pos))
                    {
                        if (NewGame != null)
                        {
                            click.Play();
                            song.Pause();
                            NewGame();
                        }
                    }
                    else if (credits_rect.Contains(pos))
                    {
                        if (Credits != null)
                        {
                            click.Play();
                            song.Pause();
                            Credits();
                        }
                    }
                    else if (back_rect.Contains(pos))
                    {
                        if (Exit != null)
                        {
                            click.Play();
                            song.Stop();
                            Exit();
                        }
                    }
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    if (Exit != null)
                    {
                        click.Play();
                        song.Stop();
                        Exit();
                    }

            base.Update(gameTime);
        }
#endif

        public event Action ResumeGame;
        public event Action NewGame;
        public event Action Credits;
        public event Action Exit;

        public Vector2 vectorAdd3 = Vector2.One * 4.0f;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.DrawString(font, Strings.ResumeGame, res_pos + vectorAdd3, Color.Black * 0.5f);
            spriteBatch.DrawString(font, Strings.NewGame, new_gam_pos + vectorAdd3, Color.Black * 0.5f);
            spriteBatch.DrawString(font, Strings.Exit, back_pos + vectorAdd3, Color.Black * 0.5f);

            spriteBatch.DrawString(fontCredits, Strings.Credits, credits_pos + vectorAdd3, Color.Black * 0.5f);

            spriteBatch.DrawString(font, Strings.ResumeGame, res_pos, saveGameExists ? Color.White : Color.White * 0.7f);
            spriteBatch.DrawString(font, Strings.NewGame, new_gam_pos, Color.White);
            spriteBatch.DrawString(font, Strings.Exit, back_pos, Color.White);

            spriteBatch.DrawString(fontCredits, Strings.Credits, credits_pos, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
