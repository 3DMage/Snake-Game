using Client.GameComponents.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameComponents
{
    public class Food : WorldObject
    {
        public int foodID;
        public Animation foodAnimation;

        public Food(Vector2 position, float radianRotation, float radius, Color color, World world) : base(position, radianRotation, new Vector2(2 * radius, 2 * radius), ObjectTag.FOOD, GraphicsManager.starCandyTexture, color, world)
        {
            Random random = new Random();

            float radiusScaleFactor = (float)random.NextDouble() * 0.75f + 0.5f;
            float randomRadius = radius * radiusScaleFactor;

            this.color = color;
            size = new Vector2(randomRadius, randomRadius);

            int randomNumber = random.Next(1, 4);

            if(randomNumber == 1)
            {
                foodAnimation = new Animation(GraphicsManager.animatedFoodSpriteSheet, 8, 8);
            }
            else if (randomNumber == 2)
            {
                foodAnimation = new Animation(GraphicsManager.animatedFoodSpriteSheet2, 8, 8);
            }
            else
            {
                foodAnimation = new Animation(GraphicsManager.animatedFoodSpriteSheet3, 8, 8);
            }


            renderScaleFactor = new Vector2(randomRadius / foodAnimation.FrameWidth, randomRadius / foodAnimation.FrameHeight) * 1.5f;
            originOffset = new Vector2(foodAnimation.FrameWidth / 2f, foodAnimation.FrameHeight / 2f);

        }

        public override void Draw(Camera viewportCamera)
        {
            GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
            // Draw the animated sprite instead of a static texture
            GraphicsManager.spriteBatch.Draw(
                foodAnimation.Texture,
                new Vector2(position.X, -position.Y),
                foodAnimation.CurrentFrameToDraw,  // Use the current frame to draw from the sprite sheet
                color,
                -radianRotation,
                originOffset,
                renderScaleFactor,
                SpriteEffects.None,
                0f);
            GraphicsManager.spriteBatch.End();
        }

        public void DeleteFood()
        {
            AudioManager.src_pop.Play();

            Emitter_Twinkle effect = new Emitter_Twinkle(position, size * 2, GraphicsManager.starTexture, world);
            ParticleSystem.AddEmitter(effect);
            effect.Emit();

            DeleteWorldObject();
        }
    }
}
