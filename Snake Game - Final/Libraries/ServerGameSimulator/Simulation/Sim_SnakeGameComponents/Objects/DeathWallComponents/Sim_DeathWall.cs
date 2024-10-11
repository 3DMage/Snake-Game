using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_DeathWall : Sim_PhysObject_Box
    {
        public Sim_DeathWall(Vector2 position, Vector2 size, Sim_World world) : base(position, 0, size, Sim_ObjectTag.DEATH_WALL, world)
        {

        }
    }
}
