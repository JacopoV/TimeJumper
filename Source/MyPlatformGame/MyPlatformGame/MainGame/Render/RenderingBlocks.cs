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
    //draw block
    class RenderingBlocks : RenderingGameObject
    {
        Level level;
        Texture2D block1;
        Texture2D block2a;
        Texture2D block2b;
        Texture2D block2c;        

        public RenderingBlocks() { }
        public RenderingBlocks(ContentManager Content,Game game)
            : base(Content, game)
        {
            level = Content.ServiceProvider.GetService(typeof(Level)) as Level;
            block1 = Content.Load<Texture2D>("Tiles/block0-1");
            block2a = Content.Load<Texture2D>("Tiles/block0-2a");
            block2b = Content.Load<Texture2D>("Tiles/block0-2b");
            block2c = Content.Load<Texture2D>("Tiles/block0-2c");
        }

        //draw block and platform
        public void Draw(GameTime gameTime, SpriteBatch sp, Vector2 visibleHArea, Vector2 visibleVArea)
        {
            if (level.switchLevel == 1)
            {
                for (int i = 0; i < level.blockList1.Count; i++)
                {
                    if (level.blockList1[i].posX >= visibleHArea.X && level.blockList1[i].posX <= visibleHArea.Y)
                    {
                        if (level.blockList1[i].posY >= visibleVArea.X && level.blockList1[i].posY <= visibleVArea.Y)
                        {
                            sp.Draw(block1, new Rectangle(level.blockList1[i].posX * level.blockList1[i].width, level.blockList1[i].posY * level.blockList1[i].height, level.blockList1[i].width, level.blockList1[i].height), Color.White);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < level.blockList2.Count; i++)
                {
                    if (level.blockList2[i].posX >= visibleHArea.X && level.blockList2[i].posX <= visibleHArea.Y)
                    {
                        if (level.blockList2[i].posY >= visibleVArea.X && level.blockList2[i].posY <= visibleVArea.Y)
                        {
                            if (level.blockList2[i].typeBlock == 1)
                            {
                                sp.Draw(block2a, new Rectangle(level.blockList2[i].posX * level.blockList2[i].width, level.blockList2[i].posY * level.blockList2[i].height, level.blockList2[i].width, level.blockList2[i].height), Color.White);
                            }
                            else if(level.blockList2[i].typeBlock == 2)
                            {
                                sp.Draw(block2b, new Rectangle(level.blockList2[i].posX * level.blockList2[i].width, level.blockList2[i].posY * level.blockList2[i].height, level.blockList2[i].width, level.blockList2[i].height), Color.White);
                            }
                            else if(level.blockList2[i].typeBlock == 3)
                            {
                                sp.Draw(block2c, new Rectangle(level.blockList2[i].posX * level.blockList2[i].width, level.blockList2[i].posY * level.blockList2[i].height, level.blockList2[i].width, level.blockList2[i].height), Color.White);
                            }
                        }
                    }
                }
            }

            //for (int i = 0; i < level.platformList.Count; i++)
            //{
            //    sp.Draw(block0, new Rectangle(level.platformList[i].posX * level.platformList[i].width, level.platformList[i].posY * level.blockList1[i].height, level.platformList[i].width, level.platformList[i].height), Color.White);
            //}

            base.Draw(gameTime, sp, visibleHArea, visibleVArea);
        }

        //update service when load a next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            //block1 = content.Load<Texture2D>("Tiles/block" + level.numLevel.ToString() + "-1");
            //block2a = content.Load<Texture2D>("Tiles/block" + level.numLevel.ToString() + "-2a");
            //block2b = content.Load<Texture2D>("Tiles/block" + level.numLevel.ToString() + "-2b");
            base.UpdateLevelService(content);
        }
    }
}
