using Contracts.DataContracts;
using GameComponents;
using Microsoft.Xna.Framework;

namespace Client.__GAME.SnakeGame.Components.DeathWallComponents
{
    public class DeathWall : WorldObject
    {
        public DeathWall(Vector2 position, Vector2 size, World world) : base(position, 0, size, ObjectTag.DEATH_WALL, GraphicsManager.rectColorTexture, Color.Red, world)
        {

        }
    }
}
