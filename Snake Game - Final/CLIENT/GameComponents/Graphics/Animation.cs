using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Client.GameComponents.Graphics
{
    public class Animation
    {
        public Texture2D Texture { get; private set; }
        public int CurrentFrame { get; private set; }
        public int FrameCount { get; private set; }
        public int FrameWidth => Texture.Width / FramesPerRow;
        public int FrameHeight => Texture.Height / FramesPerColumn;
        public float FrameSpeed { get; set; }
        public bool IsLooping { get; set; }
        public Rectangle CurrentFrameToDraw;

        private double timeSinceLastFrame = 0.0;
        private int FramesPerRow;
        private int FramesPerColumn;

        public Animation(Texture2D texture, int framesPerRow, int framesPerColumn)
        {
            Texture = texture;
            FrameCount = framesPerRow * framesPerColumn; // Total frames
            FramesPerRow = framesPerRow;
            FramesPerColumn = framesPerColumn;
            IsLooping = true;
            FrameSpeed = 0.05f; // Time each frame is displayed
            CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            // Accumulate elapsed time
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            // Check if the time since the last frame has surpassed FrameSpeed
            if (timeSinceLastFrame >= FrameSpeed)
            {
                // Move to the next frame
                CurrentFrame++;
                // Reset the timer
                timeSinceLastFrame = 0;

                // If CurrentFrame has exceeded the number of frames and animation should loop
                if (CurrentFrame >= FrameCount)
                {
                    if (IsLooping)
                        CurrentFrame = 0; // Loop back to start
                    else
                        CurrentFrame = FrameCount - 1; // Stay on the last frame
                }
            }

            // Calculate frame row and column
            int row = CurrentFrame / FramesPerRow;
            int column = CurrentFrame % FramesPerRow;

            // Update the rectangle for drawing the current frame
            CurrentFrameToDraw = new Rectangle(column * FrameWidth, row * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
