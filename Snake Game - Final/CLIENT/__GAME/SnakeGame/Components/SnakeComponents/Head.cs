using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameComponents
{
    public class Head : SnakeComponent
    {
        public float nameLength;

        public Head(Vector2 position, World world, Snake snake) : base(position, 0, ObjectTag.HEAD, world, snake)
        {
            nameLength = GraphicsManager.calibriFont.MeasureString(snake.playerName).X;
        }

      

        public override void Draw(Camera viewportCamera)
        {
            if (!snake.invincible)
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.bodySegmentTexture, new Vector2(position.X, -position.Y), null, snake.color, 0, originOffset, renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.snakeEyesTexture, new Vector2(position.X, -position.Y), null, Color.White, -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.End();
            }
            else
            {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.bodySegmentTexture, new Vector2(position.X, -position.Y), null, new Color(snake.color.R, snake.color.G, snake.color.B, 0.25f), 0, originOffset, renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.Draw(GraphicsManager.snakeEyesTexture, new Vector2(position.X, -position.Y), null, Color.White, -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0);
                GraphicsManager.spriteBatch.End();
            }
        }
    }
}
