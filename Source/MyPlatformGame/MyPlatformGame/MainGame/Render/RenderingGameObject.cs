using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyPlatformGame
{
    //draw money
    class RenderingGameObject : RenderingEnemies
    {

        Level level;

        public RenderingGameObject() { }
        public RenderingGameObject(ContentManager Content,Game game)
            : base(Content,game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;            
        }

        //draw money
        public void Draw(GameTime gameTime, SpriteBatch sp, Vector2 visibleHArea, Vector2 visibleVArea)
        {
            if (level.switchLevel == 1)
            {
                for (int i = 0; i < level.ObjectList1.Count; i++)
                {
                    if (level.ObjectList1[i].posX >= visibleHArea.X && level.ObjectList1[i].posX <= visibleHArea.Y)
                    {
                        if (level.ObjectList1[i].posY >= visibleVArea.X && level.ObjectList1[i].posY <= visibleVArea.Y)
                        {
                            if(level.ObjectList1[i].type == ObjectType.KeyDoorA)
                            {
                                if(!level.playerLevel.openDoorA)
                                    level.ObjectList1[i].sprite.Draw(gameTime, sp, level.ObjectList1[i].position, SpriteEffects.None);
                            }
                            else if (level.ObjectList1[i].type == ObjectType.KeyDoorB)
                            {
                                if (!level.playerLevel.openDoorB)
                                    level.ObjectList1[i].sprite.Draw(gameTime, sp, level.ObjectList1[i].position, SpriteEffects.None);
                            }
                            else if (level.ObjectList1[i].type == ObjectType.KeyDoorC)
                            {
                                if (!level.playerLevel.openDoorC)
                                    level.ObjectList1[i].sprite.Draw(gameTime, sp, level.ObjectList1[i].position, SpriteEffects.None);
                            }
                            else 
                            {
                                level.ObjectList1[i].sprite.Draw(gameTime, sp, level.ObjectList1[i].position, SpriteEffects.None);
                            }

                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < level.ObjectList2.Count; i++)
                {
                    if (level.ObjectList2[i].posX >= visibleHArea.X && level.ObjectList2[i].posX <= visibleHArea.Y)
                    {
                        if (level.ObjectList2[i].posY >= visibleVArea.X && level.ObjectList2[i].posY <= visibleVArea.Y)
                        {
                            if (level.ObjectList2[i].type == ObjectType.KeyDoorA)
                            {
                                if (!level.playerLevel.openDoorA)
                                    level.ObjectList2[i].sprite.Draw(gameTime, sp, level.ObjectList2[i].position, SpriteEffects.None);
                            }
                            else if (level.ObjectList2[i].type == ObjectType.KeyDoorB)
                            {
                                if (!level.playerLevel.openDoorB)
                                    level.ObjectList2[i].sprite.Draw(gameTime, sp, level.ObjectList2[i].position, SpriteEffects.None);
                            }
                            else if (level.ObjectList2[i].type == ObjectType.KeyDoorC)
                            {
                                if (!level.playerLevel.openDoorC)
                                    level.ObjectList2[i].sprite.Draw(gameTime, sp, level.ObjectList2[i].position, SpriteEffects.None);
                            }
                            else if (level.ObjectList2[i].type == ObjectType.backTeleport)
                            {
                                level.ObjectList2[i].sprite.Draw(gameTime, sp, level.ObjectList2[i].position - Vector2.UnitY * 12, SpriteEffects.None);
                            }
                            else
                            {
                                level.ObjectList2[i].sprite.Draw(gameTime, sp, level.ObjectList2[i].position, SpriteEffects.None);
                            }

                        }
                    }
                }
            }
            base.Draw(gameTime, sp, visibleHArea, visibleVArea);
        }

        //update service when load a next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            base.UpdateLevelService(content);

        }
    }
}
