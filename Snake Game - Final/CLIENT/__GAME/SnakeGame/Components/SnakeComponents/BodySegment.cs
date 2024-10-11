


using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameComponents
{
    public class BodySegment : SnakeComponent
    {
        public Texture2D snakeHeadTexture;

        public BodySegment(Vector2 position, World world, Snake snake) : base(position,0, ObjectTag.BODY_SEGMENT, world, snake) { }
    }
}
