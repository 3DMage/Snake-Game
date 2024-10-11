using Contracts.DataContracts;
using Microsoft.Xna.Framework;

namespace GameComponents
{
    public class LevelGround : WorldObject
    {
        public LevelGround(World world) : base(new Vector2(0,0), 0, new Vector2(world.worldDimensions.X, world.worldDimensions.Y), ObjectTag.NONE, GraphicsManager.groundTexture, Color.White, world) { }
    }
}
