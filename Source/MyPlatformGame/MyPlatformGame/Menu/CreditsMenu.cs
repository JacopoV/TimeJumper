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
    public class CreditsMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        
        SpriteFont font;
        Rectangle back_rect;
        Texture2D back_btn;
         
        Texture2D background;
        SoundEffect click;
        SoundEffectInstance song;


        public CreditsMenu(MainGame game)
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
            back_btn = Game.Content.Load<Texture2D>("Textures/back");
            background = Game.Content.Load<Texture2D>("MenuTexture/credits");
            click = Game.Content.Load<SoundEffect>("Sounds/Menu/click");

            song = Game.Content.Load<SoundEffect>("Sounds/h1").CreateInstance();
            song.IsLooped = true;
            song.Play();

            back_rect = new Rectangle(0, 0, back_btn.Width, back_btn.Height);

            base.LoadContent();
        }
#if WINDOWS_PHONE
        public override void Update(GameTime gameTime)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.Tap)
                {
                    var pos = new Point((int)gs.Position.X, (int)gs.Position.Y);

                  if (back_rect.Contains(pos))
                    {
                        if (Back != null)
                    {
                        click.Play();
                        Back();
                    }
                    }
                }
            }
             if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                if (Exit != null)
                {
                    MainMenu.song.Stop();
                    Exit();
                }

            base.Update(gameTime);
        }
#endif

#if WINDOWS
        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                if (back_rect.Contains(pos))
                {
                    if (Back != null)
                    {
                        click.Play();
                        song.Stop();
                        Back();
                    }
                }

            }

            base.Update(gameTime);
        }
#endif
        public event Action Back;
        public Vector2 vectorAdd3 = Vector2.One * 4.0f;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0,0,spriteBatch.GraphicsDevice.Viewport.Width,
                spriteBatch.GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(back_btn, back_rect, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
