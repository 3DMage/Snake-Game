
using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameComponents
{
    // Represents a particle in the world.
    public class Particle : WorldObject
    {
        // Velocity of the particle.
        private Vector2 velocity;

        // Max lifetime possible for the particle.
        private TimeSpan maxLifetime;

        // Current time the particle is active.
        private TimeSpan currentLifeTime;

        // A bool indicating if the particle is alive or not.
        public bool isAlive;

        // Opacity of the particle.
        public float opacity;

        // Constructor.
        public Particle(Texture2D texture, World world) : base(new Vector2(0, 0), 0, new Vector2(0, 0), ObjectTag.PARTICLE, texture, Color.White, world) { }

        // Initiates particle with Particle Parameters object and a texture.
        public void InitiateParticle(ParticleParameters parameters, Texture2D particleTexture)
        {
            this.position = parameters.position;
            this.radianRotation = parameters.radianRotation;
            this.size = parameters.size;
            this.velocity = parameters.velocity;
            this.maxLifetime = parameters.maxLifeTime;
            this.currentLifeTime = TimeSpan.Zero;
            this.isAlive = true;
            this.opacity = parameters.opacity;
            this.color = parameters.color;

            // Compute origin offset to be in center of texture.
            originOffset = new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f);

            // Compute render scale factor.
            float renderScaleFactorX = (float)(size.X / particleTexture.Width);
            float renderScaleFactorY = (float)(size.Y / particleTexture.Height);
            renderScaleFactor = new Vector2(renderScaleFactorX, renderScaleFactorY);
        }

        // Updates the particle.
        public virtual void Update(GameTime gameTime)
        {
            position += velocity;
            currentLifeTime += gameTime.ElapsedGameTime;

            // Kill particle if it's current life time exceeds max life time.
            isAlive = currentLifeTime < maxLifetime;
        }

        // Draw the particle.
        public void DrawParticle()
        {
            GraphicsManager.spriteBatch.Draw(texture, new Vector2(position.X, -position.Y), null, color, -radianRotation, originOffset, renderScaleFactor, SpriteEffects.None, 0f);
        }
    }
}