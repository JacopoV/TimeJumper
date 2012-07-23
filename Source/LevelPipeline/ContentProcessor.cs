using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using LevelData;

// TODO: replace these with the processor input and output types.
using TInput = System.String;
using TOutput = LevelData.Level;

namespace LevelPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "LevelProcessor")]
    public class ContentProcessor : ContentProcessor<TInput, TOutput>
    {
        private int countTiles;
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // TODO: process the input object, and return the modified data.
            Level lvl = new Level();

            char[] p = input.ToCharArray();

            lvl.ObjectList1.Clear();
            lvl.ObjectList2.Clear();

            lvl.enemyList1.Clear();
            lvl.enemyList2.Clear();

            lvl.blockList1.Clear();
            lvl.blockList2.Clear();

            Random rand = new Random();
            int posX = 0;
            int posY = 0;

            lvl.numLevel = (int)p[0] - 48;

            lvl.storyMode = true;

            Random roll = new Random();
            //int numberType = (int)p[1] - 48; //roll.Next() % 7;

            for (int i = 1; i < p.Length; i++)
            {
                if (i < p.Length / 2+1)
                {
                    if (p[i] == '#')
                    {
                        lvl.blockList1.Add(new block(posX, posY, 1));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'D')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.DoorA));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'd')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.KeyDoorA));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'Q')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.DoorB));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'q')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.KeyDoorB));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'W')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.DoorC));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'w')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.KeyDoorC));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'A')
                    {
                        lvl.enemyList1.Add(new Enemy(posX, posY, EnemyType.Monster, ColorEnemy.Red));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'B')
                    {
                        lvl.enemyList1.Add(new Enemy(posX, posY, EnemyType.Thorns, ColorEnemy.noColor));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'C')
                    {
                        lvl.enemyList1.Add(new Enemy(posX, posY, EnemyType.Guillottine, ColorEnemy.noColor));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'E')
                    {
                        lvl.enemyList1.Add(new Enemy(posX, posY, EnemyType.Electricity, ColorEnemy.noColor));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '1')
                    {
                        lvl.playerLevel = new player(posX, posY);
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        //lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'X')
                    {
                        lvl.ObjectList1.Add(new ObjGame(posX, posY, ObjectType.Exit));
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '.')
                    {
                        lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '\r')
                    {
                        countTiles = posX;
                        posX = 0;
                        posY++;
                        if (posY >= 32)
                        {
                            posY = 0;
                            countTiles = 0;
                        }
                    }
                }
                else
                {
                    if (p[i] == '#')
                    {
                        lvl.blockList2.Add(new block(posX, posY, roll.Next()%3+1));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'D')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.DoorA));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'd')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.KeyDoorA));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'Q')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.DoorB));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'q')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.KeyDoorB));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'W')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.DoorC));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Impassable));
                        posX++;
                    }
                    else if (p[i] == 'w')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.KeyDoorC));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'A')
                    {
                        lvl.enemyList2.Add(new Enemy(posX, posY, EnemyType.Monster, ColorEnemy.Red));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'B')
                    {
                        lvl.enemyList2.Add(new Enemy(posX, posY, EnemyType.Thorns, ColorEnemy.noColor));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'C')
                    {
                        lvl.enemyList2.Add(new Enemy(posX, posY, EnemyType.Guillottine, ColorEnemy.noColor));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'E')
                    {
                        lvl.enemyList2.Add(new Enemy(posX, posY, EnemyType.Electricity, ColorEnemy.noColor));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '1')
                    {
                        lvl.playerLevel = new player(posX, posY);
                        //lvl.tiles1.Add(new Tile(LevelData.TileCollision.Passable));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == 'S')
                    {
                        lvl.ObjectList2.Add(new ObjGame(posX, posY, ObjectType.backTeleport));
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '.')
                    {
                        lvl.tiles2.Add(new Tile(LevelData.TileCollision.Passable));
                        posX++;
                    }
                    else if (p[i] == '\r')
                    {
                        countTiles = posX;
                        posX = 0;
                        posY++;
                    }
                }

            }

            lvl.lenghtX = 32;//countTiles;
            lvl.lenghtY = 32;//posY+1;

            return lvl;
        }
    }
}