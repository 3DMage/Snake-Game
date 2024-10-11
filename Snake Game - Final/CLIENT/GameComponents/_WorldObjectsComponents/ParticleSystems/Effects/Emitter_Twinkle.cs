using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameComponents
{
    public class Emitter_Twinkle : Emitter
    {
        Random random = new Random();
        float maxLifeTimeSeconds = 0.4f; // Adjusted lifetime for variability in twinkling
        BlendState additiveBlend = new BlendState
        {
            ColorBlendFunction = BlendFunction.Add,
            AlphaBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One
        };

        Vector2 areaSize; // Size of the area for random dispersion

        public Emitter_Twinkle(Vector2 position, Vector2 areaSize, Texture2D particleTexture, World world) : base(position, 0, 15, particleTexture, world)
        {
            this.areaSize = areaSize;
        }

        public override void Emit()
        {
            for (int i = 0; i < maxParticleCount; i++)
            {
                if (vacantParticleIndices.Count > 0)
                {
                    ParticleParameters parameters = MakeTwinkleParticleComponent();
                    particles[vacantParticleIndices.Pop()].InitiateParticle(parameters, particleTexture);
                }
            }
        }

        private ParticleParameters MakeTwinkleParticleComponent()
        {
            float startSize = random.Next(12, 30);

            // Particles are static after being placed
            Vector2 velocity = Vector2.Zero;

            Color color = Color.Lerp(Color.White, Color.Yellow, (float)random.NextDouble()); // Colors closer to white and yellow for twinkling stars
            float opacity = (float)random.NextDouble() * 0.5f + 0.5f; // Ensures that particles start between 50% and 100% opacity
            TimeSpan lifeTime = TimeSpan.FromSeconds(random.NextDouble() * maxLifeTimeSeconds);

            // Randomly disperse particles within the specified area
            Vector2 positionOffset = new Vector2((float)random.NextDouble() * areaSize.X, (float)random.NextDouble() * areaSize.Y);
            Vector2 position = this.position - areaSize / 2 + positionOffset; // Center the dispersion area around the emitter's position

            Vector2 size = new Vector2(startSize, startSize);

            return new ParticleParameters(position, 0, size, lifeTime, velocity, color, opacity);
        }

        public override void DrawParticles()
        {
            GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, additiveBlend, null, null, null, null, world.viewportCamera.TranslationMatrix);
            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].isAlive)
                {
                    particles[i].DrawParticle();
                }
            }
            GraphicsManager.spriteBatch.End();
        }
    }
}