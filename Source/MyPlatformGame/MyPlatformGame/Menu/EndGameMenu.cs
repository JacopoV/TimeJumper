using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyPlatformGame.Menu
{
    class EndGameMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont fontOption;
        Texture2D menuBackgroundLose;
        Texture2D menuBackgroundWin;
        Rectangle restartRectangle;
        Rectangle mainMenuRectangleWin;
        Rectangle mainMenuRectangleLose;
        SoundEffectInstance song;
        SoundEffect click;

        public bool resultGame;

        public event Action menu;
        public event Action restart;
        public event Action Exit;

        public EndGameMenu(Game game, bool win)
        : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;

            resultGame = win;

            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            loadContent();

             restartRectangle = new Rectangle(game.GraphicsDevice.Viewport.Width - 350, game.GraphicsDevice.Viewport.Height - 180, 250, 40);
             mainMenuRectangleLose = new Rectangle(game.GraphicsDevice.Viewport.Width - 350, game.GraphicsDevice.Viewport.Height - 110, 200, 40);
             mainMenuRectangleWin = new Rectangle(game.GraphicsDevice.Viewport.Width - 210, 20, 180, 30);
        }

        public void loadContent()
        {
            menuBackgroundWin = Game.Content.Load<Texture2D>("MenuTexture/game_complete");
            font = Game.Content.Load<SpriteFont>("Fonts/gameOverFont");
            fontOption = Game.Content.Load<SpriteFont>("Fonts/menuFont");
            song = Game.Content.Load<SoundEffect>("Sounds/Menu/menu").CreateInstance();
            click = Game.Content.Load<SoundEffect>("Sounds/Menu/click");
            song.IsLooped = true;
            song.Play();
        }

        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                    if (restartRectangle.Contains(pos))
                    {
                        if (restartRectangle != null)
                        {
                            click.Play();
                            song.Stop();
                            restart();
                        }
                    }
                    else if (resultGame)
                    {
                        if (mainMenuRectangleWin.Contains(pos))
                        {
                            if (mainMenuRectangleWin != null)
                            {
                                click.Play();
                                song.Stop();
                                menu();
                            }
                        }
                    }
                    else
                    {
                        if (mainMenuRectangleLose.Contains(pos))
                        {
                            if (mainMenuRectangleLose != null) menu();
                        }
                    }
                }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (Exit != null)
                {
                    Exit();
                }
#endif
        }

        public override void Draw(GameTime gametime)
        {
            spriteBatch.Begin();

            if(resultGame)
                spriteBatch.Draw(menuBackgroundWin, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);
            else
                spriteBatch.Draw(menuBackgroundLose, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
        }

    }
}