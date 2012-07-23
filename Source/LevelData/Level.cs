using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace LevelData
{
    //this is the type of collision
    public enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }

    //this is the type of object
    public enum ObjectType
    {
        DoorA = 98,
        KeyDoorA = 96,
        DoorB = 97,
        KeyDoorB = 94,
        DoorC = 95,
        KeyDoorC = 91,
        backTeleport = 93,
        Exit=92
    }

    //this is the type of enemy
    public enum EnemyType
    {
        Monster = 150,
        Thorns = 0,
        Guillottine = 300,
        Electricity = 1
    }

    //this is the color of enemy
    public enum ColorEnemy
    {
        Yellow = 0,
        Red,
        Green,
        noColor
    }

    public enum SoundType
    {
        jump = 0,
        battlePlayer = 1,
        battleEnemy = 17,
        diePlayer = 2,
        dieEnemy = 16,
        star = 4,
        openDoor = 5,
        door = 11,
        nextLevel = 6,
        poison = 7,
        dollar = 8,
        invulnerability = 9,
        noSound = 10,
        musicPlay = 15,
        musicStop = 12,
        musicRestart = 18,
        storyPlay = 13,
        storyStop = 14,
        goodSwitch = 19,
        badSwitch = 20,
        key = 21
    }

    //this is a player direction when run
    public enum FaceDirection
    {
        Left = -1,
        Right = 1,
    }

    //this is a structure of a tile
    public class Tile
    {
        public TileCollision Collision;
#if WINDOWS_PHONE
        public const int Width = 50;
        public const int Height = 40;
#else
        public const int Width = 32;//25;
        public const int Height = 24;//20;
#endif
        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile() { }
        public Tile(TileCollision collision)
        {
            Collision = collision;
        }
    }

    public class ObjGame
    {
        public ObjectType type;
        public AnimationPlayer sprite;
        public int typeDoor;
        public int posX;
        public int posY;

        public bool open = false;
        private Rectangle boundingRectangle;

        public Vector2 position;

        public ObjGame() { }
        public ObjGame(int x, int y, ObjectType type)
        {
            position = new Vector2(x * Tile.Size.X, y * Tile.Size.Y);
            this.type = type;
            posX = x;
            posY = y;
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, Tile.Width, Tile.Height);
        }

        public Rectangle BoundingRectangle
        {
            set
            {
                boundingRectangle = value;
            }
            get
            {
                return boundingRectangle;
            }
        }
    }

    public class Enemy
    {
        public int left;
        public int top;
        public Vector2 position;
        public Vector2 start;
        public FaceDirection direction = FaceDirection.Left;
        public float waitTime = 0.0f;
        public float MaxWaitTime = 0.5f;
        public float MoveSpeed;
        public EnemyType type;
        public AnimationPlayer sprite;

        public int posX;
        public int posY;

        public int height;

        public ColorEnemy color;
        public float temporization = 1;
        public float timeActive = 0;

        public Rectangle localBounds;

        public Enemy() { }
        public Enemy(int x, int y, EnemyType type, ColorEnemy color)
        {
            start = new Vector2(x * Tile.Size.X, y * Tile.Size.Y);
            position = start;
            height = 0;
            posX = x;
            posY = y;
            this.type = type;
            this.color = color;
            MoveSpeed = (int)type;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                left = (int)Math.Round(position.X) + localBounds.X;
                top = (int)Math.Round(position.Y) + localBounds.Y;
                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }
    }

    public struct block
    {
        public int width;
        public int height;

        public int posX;
        public int posY;
        public int typeBlock;

        public block(int x, int y, int type)
        {
            posX = x;
            posY = y;
            this.typeBlock = type;
            width = Tile.Width;
            height = Tile.Height;
        }
    }

    public class player
    {
        public int left;
        public int top;
        public Vector2 position;
        public Vector2 start;
        public float movement;
        public bool isJumping;
        public bool isRunning;
        public bool wasJumping;
        public float jumpTime;
        public bool isOnGround;
        public bool isAlive;
        public Vector2 velocity;
        public float previousBottom;
        public bool onExit;
        public int points = 0;
        public bool exit = false;
        public bool stopSound = false;
        public bool orientationSx = false;
        public bool openDoorA = false;
        public bool openDoorB = false;
        public bool openDoorC = false;
        public AnimationPlayer sprite;
        public float dieTime = 0;

        public Rectangle localBounds;

        public player() { }
        public player(int x, int y)
        {
            position = new Vector2(x * Tile.Size.X, (y - 1) * Tile.Size.Y);
            start = position;
            movement = 0.0f;
            isJumping = false;
            isRunning = false;
            wasJumping = false;
            jumpTime = 0.0f;
            isOnGround = false;
            isAlive = true;
            velocity = Vector2.Zero;
            previousBottom = 0;

            //this is a bounding rect of the player
            localBounds = new Rectangle(9, 0, (int)(30), (int)(48));
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                left = (int)Math.Round(position.X) + localBounds.X;
                top = (int)Math.Round(position.Y) + localBounds.Y;
                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }
    }

    public class Level
    {
        public List<ObjGame> ObjectList1 = new List<ObjGame>();
        public List<ObjGame> ObjectList2 = new List<ObjGame>();

        public List<Enemy> enemyList1 = new List<Enemy>();
        public List<Enemy> enemyList2 = new List<Enemy>();

        public List<block> blockList1 = new List<block>();
        public List<block> blockList2 = new List<block>();

        public player playerLevel = new player();

        public List<Tile> tiles1 = new List<Tile>();
        public List<Tile> tiles2 = new List<Tile>();

        public List<SoundType> soundList = new List<SoundType>();

        public int lenghtX = 0;
        public int lenghtY = 0;

        public int numLevel = 0;

        public float maxTimeRemaining = 60.0f;
        public float timeRemaining = 60.0f;
        public float menuDieTime = 0.0f;
        public float errorSwitch = 0;

        public bool storyMode = false;
        public int switchLevel = 1;

        public Level()
        {
        }

        //return the collision type of a tile
        public TileCollision GetCollision(int x, int y)
        {
            int position = x + y * lenghtX;

            if (switchLevel == 1)
            {
                if (position < tiles1.Count && position >= 0)
                {
                    return tiles1[position].Collision;
                }
                else
                {
                    return TileCollision.Passable;
                }
            }
            else
            {
                if (position < tiles2.Count && position >= 0)
                {
                    return tiles2[position].Collision;
                }
                else
                {
                    return TileCollision.Passable;
                }
            }
        }

        //return the collision type of a tile
        public bool GetSwitchCollision()
        {
            Rectangle bounds = playerLevel.BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height)) - 1;

            for (int i = topTile; i <= bottomTile; i++)
            {
                for (int j = leftTile; j <= rightTile; j++)
                {
                    if (tiles2[j + (lenghtX * i)].Collision == TileCollision.Impassable)
                        return false;
                }
            }
            return true;
        }


        public void SetCollision(int x, int y)
        {
            int position = x + y * lenghtX;

            if (switchLevel == 1)
                tiles1[position].Collision = TileCollision.Passable;
            else
                tiles2[position].Collision = TileCollision.Passable;

        }

        public void ResetCollision(int x, int y)
        {
            int position = x + y * lenghtX;

            if (switchLevel == 1)
                tiles1[position].Collision = TileCollision.Impassable;
            else
                tiles2[position].Collision = TileCollision.Impassable;

        }

        //get the bound of a tile
        public Rectangle GetBounds(int x, int y)
        {
            if (switchLevel == 1)
            {
                for (int i = 0; i < ObjectList1.Count(); i++)
                    if (ObjectList1[i].posX == x && ObjectList1[i].posY == y)
                        return ObjectList1[i].BoundingRectangle;

                return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
            }
            else
            {
                for (int i = 0; i < ObjectList2.Count(); i++)
                    if (ObjectList2[i].posX == x && ObjectList2[i].posY == y)
                        return ObjectList2[i].BoundingRectangle;

                return new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height);
            }

        }

        public Vector2 checkVisibleHArea(float cameraPosition)
        {
            Vector2 visibleHArea = new Vector2();

            visibleHArea.X = (int)Math.Floor(cameraPosition / Tile.Width);
            visibleHArea.Y = visibleHArea.X + GraphicsDeviceManager.DefaultBackBufferWidth / Tile.Width;
            visibleHArea.Y = Math.Min(visibleHArea.Y, lenghtX);

            return visibleHArea;
        }

        public Vector2 checkVisibleVArea(float cameraPosition)
        {
            Vector2 visibleArea = new Vector2();

            visibleArea.X = (int)Math.Floor(cameraPosition / Tile.Height);
            visibleArea.Y = visibleArea.X + GraphicsDeviceManager.DefaultBackBufferHeight / Tile.Height;
            visibleArea.Y = Math.Min(visibleArea.Y, lenghtY);

            return visibleArea;
        }

    }
}

