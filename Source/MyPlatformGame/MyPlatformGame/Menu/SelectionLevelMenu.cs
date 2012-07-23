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
using MyPlatformGame.StateManager;


namespace MyPlatformGame.Menu
{
    public class SelectionLevelMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont font;
        SoundEffect click;

        List<Rectangle> levels_rect;
        public List<Action> action_Levels;
        int numLevel;

        Rectangle back_rect;

        public event Action StartLevel;

        Texture2D background;
        Texture2D textureGood;
        Texture2D textureBad;
        Texture2D back_btn;
        MainGame gameMain;
        SoundEffectInstance song;

        public SelectionLevelMenu(MainGame game)
            : base(game)
        {
            gameMain = game;
            TouchPanel.EnabledGestures = GestureType.Tap;
            numLevel = LoadStateLevel();
            levels_rect = new List<Rectangle>();
            action_Levels = new List<Action>();
            var midd_w = GraphicsDeviceManager.DefaultBackBufferWidth / 2;
            int row = 0;
            int col = 0;

            int posX = 50;
            int posY = 100;

            for (int i = -1; i < numLevel; i++)
            {
                if (i <= 2)
                {
                    posX += 90;
                    posY += 60;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }
                else if (i <= 6)
                {
                    posX -= 90;
                    posY += 60;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }
                else if (i <= 10)
                {
                    posX += 110;
                    posY += 20;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }
                else if (i <= 14)
                {
                    posX += 110;
                    posY -= 20;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }
                else if (i <= 18)
                {
                    posX -= 90;
                    posY -= 60;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }
                else if (i <= 21)
                {
                    posX += 90;
                    posY -= 60;
                    levels_rect.Add(new Rectangle(posX, posY,
                                                    80, 80));
                }

                action_Levels.Add(StartLevel);

                col++;
                row++;
            }
        }

        public override void Initialize()
        {
            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Fonts/levelFont");
            background = Game.Content.Load<Texture2D>("MenuTexture/levels");
            back_btn = Game.Content.Load<Texture2D>("Textures/back");
            click = Game.Content.Load<SoundEffect>("Sounds/Menu/click");
            textureBad = Game.Content.Load<Texture2D>("MenuTexture/level_to_do");
            textureGood = Game.Content.Load<Texture2D>("MenuTexture/level_complete");
            song = Game.Content.Load<SoundEffect>("Sounds/Menu/menu").CreateInstance();
            song.IsLooped = true;
            song.Play();

            back_rect = new Rectangle(0, 0, back_btn.Width, back_btn.Height);

            base.LoadContent();
        }

        private void sendLevel(int i)
        {
                StateManager.StateManager.StartGame(gameMain, true, i);
        }


        //resume level
        private int LoadStateLevel()
        {
            object numLevel = 0;
            if (IS.FileExists(savePathNumLevel))
            {
                using (var saveFile = IS.OpenFile(savePathNumLevel, System.IO.FileMode.Open))
                {
                    var deserializer = new XmlSerializer(typeof(int));
                    numLevel = deserializer.Deserialize(saveFile);
                }
            }

            return (int)numLevel;
        }


        string savePathNumLevel = "currentNumLevel.sav";

        bool saveGameExists { get { return IS.FileExists(savePathNumLevel); } }

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
                   for (int i = 0; i < levels_rect.Count(); i++)
                    {
                        var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                        if (levels_rect[i].Contains(pos))
                        {
                            sendLevel(i);
                        }
        else if (back_rect.Contains(pos))
                    {
                        if (Back != null) Back();
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
                for (int i = 0; i < levels_rect.Count(); i++)
                {
                    if (levels_rect[i].Contains(pos))
                    {
                        click.Play();
                        song.Stop();
                        System.Threading.Thread.Sleep(20);
                        sendLevel(i);
                    }
                }
                if (back_rect.Contains(pos))
                {
                    if (Back != null)
                    {
                        click.Play();
                        song.Stop();
                        System.Threading.Thread.Sleep(20);
                        Back();
                    }
                }
            }
            base.Update(gameTime);
        }
#endif

        public event Action Back;
        public event Action Exit;

        public Vector2 vectorAdd3 = Vector2.One * 4.0f;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height), Color.White);

            for (int i = 0; i < levels_rect.Count; i++)
            {
                if (i < numLevel)
                {
                    spriteBatch.Draw(textureGood, levels_rect[i], Color.White);
                    spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(levels_rect[i].Center.X-5, levels_rect[i].Center.Y-5), Color.Black);
                }
                else
                {
                    spriteBatch.Draw(textureBad, levels_rect[i], Color.White);
                    spriteBatch.DrawString(font, (i + 1).ToString(), new Vector2(levels_rect[i].Center.X-5, levels_rect[i].Center.Y-5), Color.White);
                }
            }

            spriteBatch.Draw(back_btn, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
