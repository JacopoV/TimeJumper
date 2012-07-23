using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

using LevelData;
using MyPlatformGame.Resources;

namespace MyPlatformGame
{
    public enum WinLoseState
    {
        Lose,
        None
    }

    public class MainGameComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        RenderingState renderingState;
        GameState gameState;
        SoundManager soundState;
        Level level;  
        Rectangle yes_rect, no_rect, main_rect;
        bool pause = false;        

        Texture2D mainMenuButton;
        SpriteFont font;


        public event Action Back;
        public event Action Win;

#if WINDOWS_PHONE
        IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForApplication();
#endif
#if WINDOWS
        IsolatedStorageFile IS = IsolatedStorageFile.GetUserStoreForDomain();
#endif
        string savePathNumLevel = "./currentNumLevel.sav";

        public MainGameComponent(MainGame game, bool forceNewGame, int selectedLevel)
            : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;

            if (forceNewGame)
            {
                Game.Content.Unload();
                LoadNextLevel(true, selectedLevel);
            }
            else
            {
                LoadNextLevel(true, NumLevelState());
            }
            
            Game.Services.AddService(typeof(Level), level);
            gameState = new GameState(Content);
            renderingState = new RenderingState(Content,game);
            soundState = new SoundManager(Content);

            yes_rect = new Rectangle(game.spriteBatch.GraphicsDevice.Viewport.Width / 2 - 280, 380, 100, 80);
            no_rect = new Rectangle(game.spriteBatch.GraphicsDevice.Viewport.Width/2+ 120, 380, 100, 80);

            main_rect = new Rectangle(game.spriteBatch.GraphicsDevice.Viewport.Width/2-250, 180, 500, 300);
        }

        private void ReloadLevel()
        {
            int numCurrentLevel = level.numLevel;

            Content.Unload();

            level = Content.Load<Level>("Level/level" + numCurrentLevel);
            level.storyMode = false;
        }

        ContentManager Content { get { return Game.Content; } }

        protected override void LoadContent()
        {
            mainMenuButton = Content.Load<Texture2D>("Textures/BACK_MENU");
            font = Content.Load<SpriteFont>("Fonts/menuFont");

            base.LoadContent();
        }


        public override void Initialize()
        {
            spriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

            renderingState.rtarget = new RenderTarget2D(spriteBatch.GraphicsDevice, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth, spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight);
            base.Initialize();

        }


        private int NumLevelState()
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


        //save game
        private void SaveGameState()
        {
            try
            {
                if (NumLevelState() <= level.numLevel)
                {
                    using (var saveFile = IS.CreateFile(savePathNumLevel))
                    {
                        var serializer = new XmlSerializer(typeof(int));
                        serializer.Serialize(saveFile, level.numLevel);
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        //Main update
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = new KeyboardState();
            if (keyboardState.IsKeyDown(Keys.F3))
            {
                Game.Exit();
            }
            if (!pause)
            {
                if (!level.playerLevel.onExit && level.playerLevel.isAlive)
                {
                    if (!level.storyMode)
                    {
                        level.timeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (level.timeRemaining <= 0)
                            level.playerLevel.isAlive = false;
                    }
                }
                else
                {
                    if (level.timeRemaining <= 60)
                        level.timeRemaining += (float)gameTime.ElapsedGameTime.TotalSeconds * 15;
                }
            }
            UpdateMainGame(gameTime);
            base.Update(gameTime);
        }

        private void UpdateMainGame(GameTime gameTime)
        {

            if(!pause)
            {
            //call update for the game
            gameState.Update(gameTime);
           }
            //check if player end level
            if (level.playerLevel.exit)
            {
                soundState.StopMusic();
                level.playerLevel.exit = false;
                SaveGameState();
                LoadNextLevel(false, 0);
            }

            if (!level.storyMode)
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Escape))
                {
                    pause = true;
                }

                if (pause)
                {
                    //check if back is pressed
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        var pos = new Point((int)Mouse.GetState().X, (int)Mouse.GetState().Y);
                        if (yes_rect.Contains(pos))
                        {
                            if (Back != null)
                            {
                                SaveGameState();
                                soundState.StopMusic();
                                Game.Services.RemoveService(typeof(Level));
                                Back();
                            }
                        }
                        else if (no_rect.Contains(pos))
                        {
                            pause = false;
                        }
                    }
                }
            }

            //update sound
             soundState.updateSounds();
        }

        //load next level when player arrive at the end of level
        public void LoadNextLevel(bool newGame, int selectedLevel)
        {
            //check if this is a new game
            if (newGame)
            {
                level = Content.Load<Level>("Level/level" + selectedLevel.ToString());
                level.soundList.Add(SoundType.musicStop);
                level.soundList.Add(SoundType.storyPlay);
            }
            else
            {
                //calculate new level index
                int numNextLevel = level.numLevel + 1;

                //remove old level from service
                Game.Services.RemoveService(typeof(Level));

                //load new level
                if (numNextLevel < 7)
                {
                    Level newLevel = Content.Load<Level>("Level/level" + numNextLevel);

                    //set player life and points from old level
                    newLevel.playerLevel.points = level.playerLevel.points;
                    level.numLevel = numNextLevel;

                    //update level variable
                    level = newLevel;

                    //add newlevel into services
                    Game.Services.AddService(typeof(Level), level);

                    //update level services from gameState and renderingState
                    gameState.UpdateLevelService(Content);
                    renderingState.UpdateLevelService(Content);
                    soundState.UpdateLevelService(Content);
                    level.soundList.Add(SoundType.musicStop);
                    level.soundList.Add(SoundType.storyPlay);
                }
                else
                {
                    //WIN
                    Win();
                }
            }
        }

        //draw method
        public override void Draw(GameTime gameTime)
        {
            renderingState.Draw(gameTime, spriteBatch);
            // APPLY EFFECT
            if (pause)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(mainMenuButton, main_rect, Color.White);
                spriteBatch.DrawString(font, "YES", new Vector2(yes_rect.Center.X, yes_rect.Center.Y), Color.White);
                spriteBatch.DrawString(font, "NO", new Vector2(no_rect.Center.X, no_rect.Center.Y), Color.White);
                spriteBatch.End();
            }
            if(!level.storyMode)
                renderingState.DrawHud(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
