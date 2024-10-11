using System;

namespace GameComponents
{
    // Extension method to generate a random float number within a range
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}
