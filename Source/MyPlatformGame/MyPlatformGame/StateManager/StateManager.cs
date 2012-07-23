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
using MyPlatformGame.Menu;


namespace MyPlatformGame.StateManager
{
    public static class StateManager
    {
        public static void ShowLogoScreen(this MainGame game, SpriteBatch spriteBatch)
        {
            game.CleanupComponents();

        }

        public static void ShowMainMenu(this MainGame game)
        {
            game.CleanupComponents();
            var menu = new MainMenu(game);

            menu.Exit += game.Exit;
            menu.NewGame += () => game.ShowLevelMenu();
            menu.ResumeGame += game.ResumeGame;
            menu.Credits += game.ShowCredits;

            game.Components.Add(menu);
        }

        public static void ShowLevelMenu(this MainGame game)
        {
            game.CleanupComponents();
            var menu = new SelectionLevelMenu(game);

            menu.Back += game.ShowMainMenu;
            menu.Exit += game.Exit;

            for (int i = 0; i < menu.action_Levels.Count(); ++i)
            {
                menu.action_Levels[i] += () => game.StartGame(true, i);
            }
            game.Components.Add(menu);
        }

        public static void ShowHelpMenu(this MainGame game)
        {
        }

        public static void ShowCredits(this MainGame game)
        {
            game.CleanupComponents();

            var menu = new CreditsMenu(game);
            menu.Back += game.ShowMainMenu;

            game.Components.Add(menu);
        }

        public static void StartGame(this MainGame game, bool forceNewgame, int selcetedLevel)
        {
            game.CleanupComponents();

            var mainGame = new MainGameComponent(game, forceNewgame, selcetedLevel);
            mainGame.Back += game.ShowMainMenu;
            mainGame.Win += () => game.ShowEndMenu(true);

            game.Components.Add(mainGame);
        }

        public static void ShowEndMenu(this MainGame game, bool isWinner)
        {
            game.CleanupComponents();

            var endMenu = new EndGameMenu(game, isWinner);
            endMenu.restart += () => game.StartGame(false, 0);
            endMenu.menu += game.ShowMainMenu;
            endMenu.Exit += game.Exit;

            game.Components.Add(endMenu);
        }

        public static void ResumeGame(this MainGame game)
        {
            game.CleanupComponents();

            var mainGame = new MainGameComponent(game, false, 0);
            mainGame.Back += game.ShowMainMenu;
            mainGame.Win += () => game.ShowEndMenu(true);

            game.Components.Add(mainGame);
        }

        public static void CleanupComponents(this MainGame game)
        {
            game.StartNewTransitionScreen();

            for (int i = 0; i < game.Components.Count; i++)
            {
                if (game.Components[i] is TransitionScreen) continue;
                ((GameComponent)game.Components[i]).Dispose();
                i--;
            }
        }

        static bool first_transition = true;
        public static void StartNewTransitionScreen(this MainGame game)
        {
            TransitionScreen transition = null;
            if (first_transition != true)
                transition = new TransitionScreen(game);

            if (first_transition != true)
            {
                game.Components.Add(transition);

                transition.OnTransitionStart += () =>
                {
                    for (int i = 0; i < game.Components.Count; i++)
                    {
                        GameComponent gc = (GameComponent)game.Components[i];
                        if (gc != transition)
                            gc.Enabled = false;
                    }
                };

                transition.OnTransitionEnd += () =>
                {
                    for (int i = 0; i < game.Components.Count; i++)
                    {
                        GameComponent gc = (GameComponent)game.Components[i];
                        if (gc != transition)
                            gc.Enabled = true;
                        else
                        {
                            game.Components.RemoveAt(i);
                            i--;
                        }
                    }
                };
            }
            first_transition = false;
        }
    }
}
