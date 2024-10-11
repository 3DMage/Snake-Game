using Contracts.DataContracts;
using Microsoft.Xna.Framework;

namespace Server.Simulation.SimulationComponents.Sim_World_Components
{
    public class Sim_Boundary
    {
        public Vector2 upperLeft;
        public Vector2 bottomRight;

        public Sim_Boundary(Vector2 upperLeft, Vector2 bottomRight)
        {
            this.upperLeft = upperLeft;
            this.bottomRight = bottomRight;
        }
    }
}
