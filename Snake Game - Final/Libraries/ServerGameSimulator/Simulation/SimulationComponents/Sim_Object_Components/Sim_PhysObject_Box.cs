using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Physics_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace Server.Simulation.SimulationComponents.Sim_Object_Components
{
    public class Sim_PhysObject_Box : Sim_Object
    {
        public Sim_BoxCollider sim_boxCollider { get; private set; }

        public Sim_PhysObject_Box(Vector2 sim_position, float sim_radianRotation, Vector2 sim_size, Sim_ObjectTag entityTag, Sim_World sim_world) : base(sim_position, sim_radianRotation, sim_size, entityTag, sim_world)
        {
            sim_boxCollider = new Sim_BoxCollider(sim_position, sim_size, this, sim_world.sim_gridCollisionHandling);
        }

        public override void UpdateSimWorldPosition(Vector2 sim_position)
        {
            this.sim_position = sim_position;
            sim_boxCollider.UpdatePosition(sim_position);
        }
    }
}
