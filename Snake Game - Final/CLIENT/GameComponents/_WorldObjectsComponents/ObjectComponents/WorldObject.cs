using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameComponents
{
    public class WorldObject
    {
        public World world;
        public int worldObject_ID;

        public ObjectTag entityTag { get; private set; }

        // The Entity's position, size and rotation data.
        public Vector2 position { get; protected set; }
        public Vector2 size { get; set; }
        public Vector2 centerToBoundLengths { get; private set; }
        public float radianRotation { get; set; }

        public Vector2 renderScaleFactor { get; protected set; }
        public Vector2 originOffset { get; protected set; }
        public Texture2D texture { get; protected set; }
        public Color color { get; protected set; }

        // Returns the radian rotation in terms of degrees.
        public float DegreeRotation()
        {
            return (float)(radianRotation * 180.0f / Math.PI);
        }

        public Vector2 GetDirectionVector()
        {
            return new Vector2((float)Math.Cos(radianRotation), (float)Math.Sin(radianRotation));
        }

        public WorldObject(Vector2 position, float radianRotation, Vector2 size, ObjectTag entityTag, Texture2D texture, Color color, World world)
        {
            this.world = world;

            worldObject_ID = world.AddWorldObject(this);

            this.position = position;
            this.radianRotation = radianRotation;
            this.size = size;
            centerToBoundLengths = new Vector2(size.X / 2f, size.Y / 2f);
            this.entityTag = entityTag;
            this.texture = texture;
            this.color = color;
            SetupTextureScaleAndOrigin(texture);

            // Ensure placed world object is clamped to world boundaries.
            UpdateWorldPosition(position);
        }

        // Method to remove this collider from the list.
        protected void DeleteWorldObject()
        {
            world.RemoveWorldObject(worldObject_ID);
        }

        public virtual void UpdateWorldPosition(Vector2 position)
        {
            this.position = position;
        }

        protected void SetupTextureScaleAndOrigin(Texture2D texture)
        {
            if (texture != null)
            {
                renderScaleFactor = new Vector2(size.X / texture.Width, size.Y / texture.Height);
                originOffset = new Vector2(texture.Width / 2f, texture.Height / 2f);
            }
        }

        public Rectangle GetBounds()
        {
            // Calculate the corners of the bounding rectangle
            int left = (int)(position.X - size.X / 2);
            int top = (int)(position.Y - size.Y / 2);
            int width = (int)size.X;
            int height = (int)size.Y;

            // Create and return the bounding rectangle
            return new Rectangle(left, top, width, height);
        }

        public virtual void Draw(Camera viewportCamera)
        {
                GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, viewportCamera.TranslationMatrix);
                GraphicsManager.spriteBatch.Draw(texture, new Vector2(position.X, -position.Y), null, color, -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0f);
                GraphicsManager.spriteBatch.End();
        }
    }
}
