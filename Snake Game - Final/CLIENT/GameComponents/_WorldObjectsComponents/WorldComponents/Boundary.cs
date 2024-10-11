using Contracts.DataContracts;
using Microsoft.Xna.Framework;

namespace GameComponents
{
    public class Boundary
    {
        public Vector2 upperLeft;
        public Vector2 bottomRight;

        public Boundary(Vector2 upperLeft, Vector2 bottomRight)
        {
            this.upperLeft = upperLeft;
            this.bottomRight = bottomRight;
        }
    }
}
