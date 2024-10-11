using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameComponents
{
    // Base class for Emitter objects.
    public abstract class Emitter : WorldObject
    {
        // Max number of particles that the emitter can emit.
        public int maxParticleCount;

        // List of particles.
        public Particle[] particles;

        // List of that can be spawned.
        public Stack<int> vacantParticleIndices;

        // Texture to use for each particle from the emitter.
        public Texture2D particleTexture { get; private set; }

        // Constructor.
        public Emitter(Vector2 position, float radianRotation, int maxParticleCount, Texture2D particleTexture, World world) : base(position, radianRotation, new Vector2(0, 0), ObjectTag.EMITTER, null, Color.White, world)
        {
            this.particleTexture = particleTexture;
            this.maxParticleCount = maxParticleCount;
            particles = new Particle[maxParticleCount];
            vacantParticleIndices = new Stack<int>(maxParticleCount);

            // Fill particles array with blank particles.
            for (int i = 0; i < maxParticleCount; i++)
            {
                particles[i] = new Particle(particleTexture, world);
                vacantParticleIndices.Push(i);
            }

            // Add emitter to the Particle System.
            ParticleSystem.AddEmitter(this);
        }

        // Updates all alive particles.
        public void UpdateParticles(GameTime gameTime)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isAlive)
                {
                    particles[i].Update(gameTime);

                    if (!particles[i].isAlive)
                    {
                        vacantParticleIndices.Push(i);
                    }
                }
            }
        }

        // Clears all active particles.
        public void ClearParticles()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].isAlive = false;
            }
        }

        // Draws all alive particles.
        public virtual void DrawParticles()
        {
            GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, world.viewportCamera.TranslationMatrix);
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isAlive)
                {
                    particles[i].DrawParticle();
                }
            }
            GraphicsManager.spriteBatch.End();
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
        }

        public abstract void Emit();
    }
}
