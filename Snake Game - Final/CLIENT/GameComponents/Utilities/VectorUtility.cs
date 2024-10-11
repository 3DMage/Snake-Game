using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using System;


namespace GameComponents
{
    public static class VectorUtility
    {
        public static Vector2 GetDirectionVector(Vector2 start, Vector2 end)
        {
            // Calculate the direction vector from start to end
            Vector2 direction = new Vector2(end.X - start.X, end.Y - start.Y);

            // Calculate the magnitude (length) of the direction vector
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            // Normalize the direction vector to get a unit vector
            Vector2 unitVector = new Vector2(direction.X / magnitude, direction.Y / magnitude);

            return unitVector;
        }
    }
}
