using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LevelData;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlatformGame
{
    class LayerGameObject
    {
        private Level level;

        private Animation portal;
        private Animation doorClose;
        private Animation openDoor;

        private Animation pass1;
        private Animation passClose1;
        private Animation handle1;

        private Animation pass2;
        private Animation passClose2;
        private Animation handle2;

        private Animation pass3;
        private Animation passClose3;
        private Animation handle3;

        public LayerGameObject(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            portal = new Animation(content.Load<Texture2D>("Sprites/portal"), 0.32f, true);

            doorClose = new Animation(content.Load<Texture2D>("Sprites/doorClose"), 0.1f, false);
            pass1 = new Animation(content.Load<Texture2D>("Sprites/pass1"), 0.1f, false);
            passClose1 = new Animation(content.Load<Texture2D>("Sprites/passClose1"), 0.1f, false);
            pass2 = new Animation(content.Load<Texture2D>("Sprites/pass2"), 0.1f, false);
            passClose2 = new Animation(content.Load<Texture2D>("Sprites/passClose2"), 0.1f, false);
            pass3 = new Animation(content.Load<Texture2D>("Sprites/pass3"), 0.1f, false);
            passClose3 = new Animation(content.Load<Texture2D>("Sprites/passClose3"), 0.1f, false);
            openDoor = new Animation(content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false);

            handle1 = new Animation(content.Load<Texture2D>("Sprites/handle1"), 0.1f, false);
            handle2 = new Animation(content.Load<Texture2D>("Sprites/handle2"), 0.1f, false);
            handle3 = new Animation(content.Load<Texture2D>("Sprites/handle3"), 0.1f, false);

            Initialize();
        }

        public void Initialize()
        {

            for (int i = 0; i < level.ObjectList1.Count; i++)
            {
                switch (level.ObjectList1[i].type)
                {
                    case ObjectType.DoorA:
                        level.ObjectList1[i].sprite.PlayAnimation(passClose1);
                        level.ObjectList1[i].typeDoor = 0;
                        break;
                    case ObjectType.DoorB:
                        level.ObjectList1[i].sprite.PlayAnimation(passClose2);
                        level.ObjectList1[i].typeDoor = 1;
                        break;
                    case ObjectType.DoorC:
                        level.ObjectList1[i].sprite.PlayAnimation(passClose3);
                        level.ObjectList1[i].typeDoor = 2;
                        break;
                    case ObjectType.KeyDoorA:
                        level.ObjectList1[i].sprite.PlayAnimation(handle1);
                        level.ObjectList1[i].typeDoor = 0;
                        break;
                    case ObjectType.KeyDoorB:
                        level.ObjectList1[i].sprite.PlayAnimation(handle2);
                        level.ObjectList1[i].typeDoor = 1;
                        break;
                    case ObjectType.KeyDoorC:
                        level.ObjectList1[i].sprite.PlayAnimation(handle3);
                        level.ObjectList1[i].typeDoor = 2;
                        break;
                    case ObjectType.Exit:
                        level.ObjectList1[i].sprite.PlayAnimation(doorClose);
                        level.ObjectList1[i].typeDoor = 3;
                        break;
                }
            }

            for (int i = 0; i < level.ObjectList2.Count; i++)
            {
                switch (level.ObjectList2[i].type)
                {
                    case ObjectType.DoorA:
                        level.ObjectList2[i].sprite.PlayAnimation(passClose1);
                        level.ObjectList2[i].typeDoor = 0;
                        break;
                    case ObjectType.DoorB:
                        level.ObjectList2[i].sprite.PlayAnimation(passClose2);
                        level.ObjectList2[i].typeDoor = 1;
                        break;
                    case ObjectType.DoorC:
                        level.ObjectList2[i].sprite.PlayAnimation(passClose3);
                        level.ObjectList2[i].typeDoor = 2;
                        break;
                    case ObjectType.KeyDoorA:
                        level.ObjectList2[i].sprite.PlayAnimation(handle1);
                        level.ObjectList2[i].typeDoor = 0;
                        break;
                    case ObjectType.KeyDoorB:
                        level.ObjectList2[i].sprite.PlayAnimation(handle2);
                        level.ObjectList2[i].typeDoor = 1;
                        break;
                    case ObjectType.KeyDoorC:
                        level.ObjectList2[i].sprite.PlayAnimation(handle3);
                        level.ObjectList2[i].typeDoor = 2;
                        break;
                    case ObjectType.backTeleport:
                        level.ObjectList2[i].sprite.PlayAnimation(portal);
                        level.ObjectList2[i].typeDoor = 3;
                        break;
                }
            }
        }

        //chec if player collide with money
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (level.switchLevel == 1)
            {
                for (int i = 0; i < level.ObjectList1.Count; i++)
                {

                    Rectangle checkCollision = new Rectangle(level.ObjectList1[i].BoundingRectangle.X-5, level.ObjectList1[i].BoundingRectangle.Y,
                                                             level.ObjectList1[i].BoundingRectangle.Width + 10,
                                                             level.ObjectList1[i].BoundingRectangle.Height);

                    if (level.playerLevel.BoundingRectangle.Intersects(checkCollision))
                    {
                        switch (level.ObjectList1[i].type)
                        {
                            case ObjectType.DoorA:
                                OnOpenDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.DoorB:
                                OnOpenDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.DoorC:
                                OnOpenDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.KeyDoorA:
                                OnKeyDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.KeyDoorB:
                                OnKeyDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.KeyDoorC:
                                OnKeyDoor(level.ObjectList1[i]);
                                break;
                            case ObjectType.Exit:
                                OnOpenDoor(level.ObjectList1[i]);
                                break;
                        }
                    }
                }
            }
            else
            {

                for (int i = 0; i < level.ObjectList2.Count; i++)
                {

                    Rectangle checkCollision = new Rectangle(level.ObjectList2[i].BoundingRectangle.X - 5, level.ObjectList2[i].BoundingRectangle.Y - 5,
                                                             level.ObjectList2[i].BoundingRectangle.Width + 10,
                                                             level.ObjectList2[i].BoundingRectangle.Height + 10);

                    if (level.playerLevel.BoundingRectangle.Intersects(checkCollision))
                    {
                        switch (level.ObjectList2[i].type)
                        {
                            case ObjectType.DoorA:
                                OnOpenDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.DoorB:
                                OnOpenDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.DoorC:
                                OnOpenDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.KeyDoorA:
                                OnKeyDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.KeyDoorB:
                                OnKeyDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.KeyDoorC:
                                OnKeyDoor(level.ObjectList2[i]);
                                break;
                            case ObjectType.backTeleport:
                                OnOpenDoor(level.ObjectList2[i]);
                                break;
                        }
                    }
                }
            
            }
        }

        private void OnKeyDoor(ObjGame objGame)
        {
            switch (objGame.type)
            {
                case ObjectType.KeyDoorA:
                    if(!level.playerLevel.openDoorA)
                        level.soundList.Add(SoundType.key);
                    level.playerLevel.openDoorA = true;
                    objGame.sprite.PlayAnimation(handle1);
                    break;
                case ObjectType.KeyDoorB:
                    if (!level.playerLevel.openDoorB)
                    level.soundList.Add(SoundType.key);
                    level.playerLevel.openDoorB = true;
                    objGame.sprite.PlayAnimation(handle2);
                    break;
                case ObjectType.KeyDoorC:
                    if (!level.playerLevel.openDoorC)
                    level.soundList.Add(SoundType.key);
                    level.playerLevel.openDoorC = true;
                    objGame.sprite.PlayAnimation(handle3);
                    break;
            }
        }


        private void OnOpenDoor(ObjGame objGame)
        {
            if (level.switchLevel == 1)
            {
                for (int i = 0; i < level.ObjectList1.Count; i++)
                {
                    //add sprite push button
                    if (level.ObjectList1[i].typeDoor == objGame.typeDoor && !level.ObjectList1[i].open)
                    {
                        switch (level.ObjectList1[i].type)
                        {
                            case ObjectType.DoorA:
                                if (level.playerLevel.openDoorA)
                                {
                                    objGame.open = true;
                                    level.ObjectList1[i].sprite.PlayAnimation(pass1);
                                    level.SetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;

                            case ObjectType.DoorB:
                                if (level.playerLevel.openDoorB)
                                {
                                    objGame.open = true;
                                    level.ObjectList1[i].sprite.PlayAnimation(pass2);
                                    level.SetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;

                            case ObjectType.DoorC:
                                if (level.playerLevel.openDoorC)
                                {
                                    objGame.open = true;
                                    level.ObjectList1[i].sprite.PlayAnimation(pass3);
                                    level.SetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;
                            case ObjectType.Exit:
                                level.playerLevel.onExit = true;
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < level.ObjectList2.Count; i++)
                {
                    //add sprite push button
                    if (level.ObjectList2[i].typeDoor == objGame.typeDoor)
                    {
                        switch (level.ObjectList2[i].type)
                        {
                            case ObjectType.DoorA:
                                if (level.playerLevel.openDoorA)
                                {
                                    level.ObjectList2[i].sprite.PlayAnimation(pass1);
                                    level.SetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;

                            case ObjectType.DoorB:
                                if (level.playerLevel.openDoorB)
                                {
                                    level.ObjectList2[i].sprite.PlayAnimation(pass2);
                                    level.SetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;

                            case ObjectType.DoorC:
                                if (level.playerLevel.openDoorC)
                                {
                                    level.ObjectList2[i].sprite.PlayAnimation(pass3);
                                    level.SetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                                    level.soundList.Add(SoundType.door);
                                }
                                break;

                            case ObjectType.backTeleport:
                                level.switchLevel = 1;
                                break;
                        }
                    }
                }
            }
        }
        //update the service whrn load the next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            Initialize();
        }

        public void ResetObject()
        {
            for (int i = 0; i < level.ObjectList1.Count; i++)
            {
                if (level.ObjectList1[i].type == ObjectType.DoorA)
                {
                    level.ObjectList1[i].open = false;
                    level.ResetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                    level.ObjectList1[i].sprite.PlayAnimation(passClose1);
                }
                if (level.ObjectList1[i].type == ObjectType.DoorB)
                {
                    level.ObjectList1[i].open = false;
                    level.ResetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                    level.ObjectList1[i].sprite.PlayAnimation(passClose2);
                }
                if (level.ObjectList1[i].type == ObjectType.DoorC)
                {
                    level.ObjectList1[i].open = false;
                    level.ResetCollision(level.ObjectList1[i].posX, level.ObjectList1[i].posY);
                    level.ObjectList1[i].sprite.PlayAnimation(passClose3);
                }

            }

            for (int i = 0; i < level.ObjectList2.Count; i++)
            {
                if (level.ObjectList2[i].type == ObjectType.DoorA)
                {
                    level.ObjectList2[i].open = false;
                    level.ResetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                    level.ObjectList2[i].sprite.PlayAnimation(passClose1);
                }
                if (level.ObjectList2[i].type == ObjectType.DoorB)
                {
                    level.ObjectList2[i].open = false;
                    level.ResetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                    level.ObjectList2[i].sprite.PlayAnimation(passClose2);
                }
                if (level.ObjectList2[i].type == ObjectType.DoorC)
                {
                    level.ObjectList2[i].open = false;
                    level.ResetCollision(level.ObjectList2[i].posX, level.ObjectList2[i].posY);
                    level.ObjectList2[i].sprite.PlayAnimation(passClose3);
                }

            }
        }

    }
}
