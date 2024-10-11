using Contracts.DataContracts;
using GameSimulator;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Physics_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace Server.Simulation.SimulationComponents.Sim_Object_Components
{
    public class Sim_PhysObject_Circle : Sim_Object
    {
        public Sim_CircleCollider sim_circleCollider { get; private set; }
        public float sim_radius { get; protected set; }


        public Sim_PhysObject_Circle(Vector2 sim_position, float sim_radianRotation, float sim_radius, Sim_ObjectTag entityTag, Sim_World sim_world) : base(sim_position, sim_radianRotation, new Vector2(2 * sim_radius, 2 * sim_radius), entityTag, sim_world)
        {
            sim_circleCollider = new Sim_CircleCollider(sim_position, sim_radius, this, sim_world.sim_gridCollisionHandling);
        }

        public override void UpdateSimWorldPosition(Vector2 sim_position)
        {
            this.sim_position = sim_position;
            sim_circleCollider.UpdatePosition(sim_position);
        }
    }
}
