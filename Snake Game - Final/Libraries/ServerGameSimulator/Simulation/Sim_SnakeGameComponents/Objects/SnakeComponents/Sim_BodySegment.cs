using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_BodySegment : Sim_SnakeComponent
    {
        public Sim_BodySegment(Vector2 sim_position, Sim_World sim_world, Sim_Snake sim_snake) : base(sim_position,0, Sim_ObjectTag.BODY_SEGMENT, sim_world, sim_snake) { }
    }
}
