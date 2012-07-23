using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelData
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        Animation animation;

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(animation.FrameWidth/2, animation.FrameHeight); }
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (this.animation == animation)
                return;

            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }
        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation,int frameIndex)
        {
            // If this animation is already running, do not restart it.
            if (this.animation == animation)
                return;

            // Start the new animation.
            this.animation = animation;
            this.frameIndex = frameIndex;
            this.time = 0.0f;
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void ResetAnimation(Animation animation)
        {
            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (this.animation != null)
            {
                // Process passing time.
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                while (time > this.animation.FrameTime)
                {
                    time -= this.animation.FrameTime;

                    // Advance the frame index; looping or clamping as appropriate.
                    if (this.animation.IsLooping)
                    {
                        frameIndex = (frameIndex + 1) % this.animation.FrameCount;
                    }
                    else
                    {
                        frameIndex = Math.Min(frameIndex + 1, this.animation.FrameCount - 1);
                    }
                }

                // Calculate the source rectangle of the current frame.
                Rectangle source = new Rectangle(FrameIndex * this.animation.FrameWidth, 0, this.animation.FrameWidth, this.animation.FrameHeight);

                // Draw the current frame.
                spriteBatch.Draw(this.animation.Texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.0f);
            }
        }
    }
}
