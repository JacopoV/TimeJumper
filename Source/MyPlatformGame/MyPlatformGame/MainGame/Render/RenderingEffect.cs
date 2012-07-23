using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MyPlatformGame
{
    //draw player
    class RenderingEffect
    {
        Level level;

        Random rangen=new Random();


        const float flashDuration = 0.5f;
        Texture2D pixel;
        GraphicsDevice dev;
        Rectangle screenRect;
        float effectTime = 0;
        int oldLevelSwitch = 1; // Usiamo questo per salvarci il vecchio stato del livello, se viene switchato facciamo un flash
        public RenderingEffect() { }
        public RenderingEffect(ContentManager Content,Game game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;
            dev = game.GraphicsDevice;
            pixel = new Texture2D(dev, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            screenRect = new Rectangle(0, 0, dev.Viewport.Width, dev.Viewport.Height);
        }
        
        //draw the player
        public void Draw(GameTime gameTime, SpriteBatch sp)
        {
            float dt=(float)gameTime.ElapsedGameTime.TotalSeconds;
            if (level.switchLevel != oldLevelSwitch)
                effectTime = flashDuration;
            else
                effectTime-=dt;
            sp.End();
            sp.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (effectTime > 0)
            {
                float f = (float)Math.Cos((1-effectTime / flashDuration) * Math.PI/2);
                sp.Draw(pixel, screenRect, new Color(f,f,f,f));
            }
            oldLevelSwitch = level.switchLevel;
            
        }

        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
        }
    }
}