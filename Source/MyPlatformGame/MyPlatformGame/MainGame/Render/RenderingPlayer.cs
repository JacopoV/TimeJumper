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
    class RenderingPlayer:RenderingEffect
    {

        Level level;
        Texture2D invulnerability;
        private Animation playerDie;
        private AnimationPlayer play;

        public RenderingPlayer() { }
        public RenderingPlayer(ContentManager Content,Game game):base(Content,game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;
            play.PlayAnimation(playerDie);
        }

        //draw the player
        public void Draw(GameTime gameTime, SpriteBatch sp)
        {
            SpriteEffects flip;
            player player = level.playerLevel;

            if (level.playerLevel.velocity.X > 0)
            {
                flip = SpriteEffects.None;
                level.playerLevel.orientationSx = false;
            }
            else if (level.playerLevel.velocity.X < 0)
            {
                flip = SpriteEffects.FlipHorizontally; //None
                level.playerLevel.orientationSx = true;
            }
            else
            {
                if (level.playerLevel.orientationSx)
                    flip = SpriteEffects.FlipHorizontally;
                else
                    flip = SpriteEffects.None;
            }

            player.sprite.Draw(gameTime, sp, player.position, flip);
            base.Draw(gameTime, sp);
        }


        //update service when load a next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            base.UpdateLevelService(content);
        }
    }
}
