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
    class RenderingEnemies : RenderingPlayer
    {
        Level level;

        public RenderingEnemies() { }
        public RenderingEnemies(ContentManager Content,Game game)
            : base(Content,game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;
        }

        //draw enemies
        public void Draw(GameTime gameTime, SpriteBatch sp, Vector2 visibleHArea, Vector2 visibleVArea)
        {
            SpriteEffects flip;
            if (level.switchLevel == 1)
            {
                for(int i = 0; i < level.enemyList1.Count; i++)
                {
                        if (level.enemyList1[i].direction == FaceDirection.Right)
                        {
                            flip = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            flip = SpriteEffects.None;
                        }

                        if ((level.enemyList1[i].position.X / Tile.Width) >= visibleHArea.X && (level.enemyList1[i].position.X / Tile.Width) <= visibleHArea.Y)
                        {
                            if ((level.enemyList1[i].position.Y / Tile.Height) >= visibleVArea.X && (level.enemyList1[i].position.Y / Tile.Height) <= visibleVArea.Y)
                            {
                                if (level.enemyList1[i].type == EnemyType.Monster)
                                {
                                    Vector2 positionEnemy = level.enemyList1[i].position - Vector2.UnitY * Tile.Height;
                                    level.enemyList1[i].sprite.Draw(gameTime, sp, positionEnemy, flip);
                                }
                                else if (level.enemyList1[i].type == EnemyType.Electricity)
                                {
                                    if (level.enemyList1[i].localBounds.Height > 0)
                                        level.enemyList1[i].sprite.Draw(gameTime, sp, level.enemyList1[i].position, flip);
                                }
                                else
                                {
                                    level.enemyList1[i].sprite.Draw(gameTime, sp, level.enemyList1[i].position, flip);
                                }
                            }
                        }
                }

            }
            else
            {
                for (int i = 0; i < level.enemyList2.Count; i++)
                {
                    if (level.enemyList2[i].direction == FaceDirection.Right)
                    {
                        flip = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        flip = SpriteEffects.None;
                    }

                    if ((level.enemyList2[i].position.X / Tile.Width) >= visibleHArea.X && (level.enemyList2[i].position.X / Tile.Width) <= visibleHArea.Y)
                    {
                        if ((level.enemyList2[i].position.Y / Tile.Height) >= visibleVArea.X && (level.enemyList2[i].position.Y / Tile.Height) <= visibleVArea.Y)
                        {
                            if (level.enemyList2[i].type == EnemyType.Monster)
                            {
                                Vector2 positionEnemy = level.enemyList2[i].position - Vector2.UnitY * Tile.Height;
                                level.enemyList2[i].sprite.Draw(gameTime, sp, positionEnemy, flip);
                            }
                            else if (level.enemyList2[i].type == EnemyType.Electricity)
                            {
                                if (level.enemyList2[i].localBounds.Height > 0)
                                    level.enemyList2[i].sprite.Draw(gameTime, sp, level.enemyList2[i].position, flip);
                            }
                            else
                            {
                                level.enemyList2[i].sprite.Draw(gameTime, sp, level.enemyList2[i].position, flip);
                            }
                        }
                    }
                }
            }
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
