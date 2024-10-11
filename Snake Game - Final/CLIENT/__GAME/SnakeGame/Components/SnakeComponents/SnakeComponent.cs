using Client.GameComponents._WorldObjectsComponents.ParticleSystems.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameComponents
{
    public abstract class SnakeComponent : WorldObject
    {
        public Vector2 targetPosition;
        public Queue<Vector2> positionHistory = new Queue<Vector2>();
        public Snake snake;
        public static float segmentRadius { get; private set; } = 20.0f;

        protected SnakeComponent(Vector2 position, float radianRotation, ObjectTag entityTag, World world, Snake snake) : base(position, radianRotation, new Vector2(2 * segmentRadius, 2 * segmentRadius), entityTag, GraphicsManager.bodySegmentTexture, snake.color, world)
        {
            SetupTextureScaleAndOrigin(GraphicsManager.bodySegmentTexture);
            this.snake = snake;
        }

        public void UpdatePositionWithHistory(Vector2 newPosition)
        {
            // Store the new position in the history, ensuring we only keep the last five
            if (positionHistory.Count >= 3)
            {
                positionHistory.Dequeue(); // Remove the oldest if we exceed the history limit
            }
            positionHistory.Enqueue(newPosition);

            // Update the actual position of the component
            UpdateWorldPosition(newPosition);
        }

        public void DeleteSegment()
        {
            Emitter_Explosion effect = new Emitter_Explosion(position, 0, 250, GraphicsManager.explosionParticleTexture, world);
            ParticleSystem.AddEmitter(effect);
            effect.Emit();
            DeleteWorldObject();
        }

        public override void Draw(Camera viewportCamera)
        {
            if (!snake.invincible)
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(texture, new Vector2(position.X, -position.Y), null, color, -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0f);
                GraphicsManager.spriteBatch.End();
            }
            else
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(texture, new Vector2(position.X, -position.Y), null, new Color(color.R, color.G, color.B, 0.25f), -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0f);
                GraphicsManager.spriteBatch.End();
            }
        }
    }
}
