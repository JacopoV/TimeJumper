using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LevelData;

namespace MyPlatformGame
{
    class SoundManager
    {
        public bool isSoundActive { get; set; }
        private SoundEffectInstance jump;
        private SoundEffectInstance diePlayer;
        private SoundEffectInstance[] music = new SoundEffectInstance[2];
        private SoundEffectInstance story;
        private SoundEffectInstance openDoor;
        private SoundEffectInstance door;
        private SoundEffectInstance nextLevel;
        private SoundEffectInstance goodSwitch;
        private SoundEffectInstance badSwitch;
        private SoundEffectInstance keyCollected;

        private Level level;
        private Random rand;

        private AudioListener cameraListener;

        public SoundManager(ContentManager content)
        {

            rand = new Random();
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;

            jump = content.Load<SoundEffect>("Sounds/PlayerJump").CreateInstance();
            diePlayer = content.Load<SoundEffect>("Sounds/PlayerKilled").CreateInstance();
            goodSwitch = content.Load<SoundEffect>("Sounds/goodSwitch").CreateInstance();
            badSwitch = content.Load<SoundEffect>("Sounds/badSwitch").CreateInstance();

            if (level.numLevel > 3)
            {
                music[0] = content.Load<SoundEffect>("Sounds/s2").CreateInstance();
                music[1] = content.Load<SoundEffect>("Sounds/h2").CreateInstance();
            }
            else
            {
                music[0] = content.Load<SoundEffect>("Sounds/s1").CreateInstance();
                music[1] = content.Load<SoundEffect>("Sounds/h1").CreateInstance();
            }
            
            openDoor = content.Load<SoundEffect>("Sounds/openDoor").CreateInstance();
            door = (content.Load<SoundEffect>("Sounds/door")).CreateInstance();
            nextLevel = content.Load<SoundEffect>("Sounds/ExitReached").CreateInstance();
            keyCollected = content.Load<SoundEffect>("Sounds/GemCollected").CreateInstance();

            openDoor.Volume = 0.5f;
            door.Volume = 0.5f;
            diePlayer.Volume = 0.5f;
            
            cameraListener = new AudioListener();
            cameraListener.Position =
                new Vector3(
                    GraphicsDeviceManager.DefaultBackBufferWidth / 2,
                    GraphicsDeviceManager.DefaultBackBufferHeight / 2,
                    0);
            isSoundActive = true;
        }

        public void updateSounds()
        {
            if (!isSoundActive)
                return;
            if (level.switchLevel == 1)
            {
                music[0].Volume = 0.7f;
                music[1].Volume = 0;
            }
            else
            {
                music[0].Volume = 0;
                music[1].Volume = 0.7f;
            }

            for (int i = 0; i < level.soundList.Count; i++)
            {
                switch (level.soundList[i])
                { 
                    case SoundType.jump:
                        jump.Play();
                        break;
                    case SoundType.goodSwitch:
                        goodSwitch.Play();
                        break;
                    case SoundType.badSwitch:
                        badSwitch.Play();
                        break;
                    case SoundType.key:
                        keyCollected.Play();
                        break;
                    case SoundType.musicPlay:
                        music[0].Play();
                        music[1].Play();
                        break;
                    case SoundType.musicStop:
                        music[0].Stop();
                        music[1].Stop();
                        break;
                    case SoundType.musicRestart:
                        music[0].Stop();
                        music[1].Stop();
                        System.Threading.Thread.Sleep(20);
                        music[0].Play();
                        music[1].Play();
                        break;
                    case SoundType.storyPlay:
                        break;
                    case SoundType.storyStop:
                        break;
                    case SoundType.diePlayer:
                        diePlayer.Play();
                        break;
                    case SoundType.openDoor:
                        openDoor.Play();
                        break;
                    case SoundType.door:
                        door.Play();
                        break;
                    case SoundType.nextLevel:
                        nextLevel.Play();
                        break;
                    case SoundType.noSound:
                        break;
                
                }            
            }

            level.soundList.Clear();

        }

        public void StopMusic()
        {
            for(int i=0;i<2;i++)
                music[i].Stop();
        }

        //update service whene load the next level
        public void UpdateLevelService(ContentManager content)
        {
            level = content.ServiceProvider.GetService(typeof(Level)) as Level;
            float temp = rand.Next(4);

            music[0].Stop();
            music[1].Stop();
            System.Threading.Thread.Sleep(10);
            if (level.numLevel > 2)
            {
                music[0] = content.Load<SoundEffect>("Sounds/s2").CreateInstance();
                music[1] = content.Load<SoundEffect>("Sounds/h2").CreateInstance();
            }
            else
            {
                music[0] = content.Load<SoundEffect>("Sounds/s1").CreateInstance();
                music[1] = content.Load<SoundEffect>("Sounds/h1").CreateInstance();
            }
            music[0].Play();
            music[1].Play();
        }

    }
}
