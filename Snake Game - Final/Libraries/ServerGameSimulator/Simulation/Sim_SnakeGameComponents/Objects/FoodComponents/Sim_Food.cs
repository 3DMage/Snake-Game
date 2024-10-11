using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_Food : Sim_PhysObject_Circle
    {
        public int sim_foodValue { get; private set; }
        public Sim_FoodSystem sim_foodSystem;
        public int sim_foodID;

        public Sim_Food(Vector2 sim_position, float sim_radianRotation, float sim_radius, Sim_World sim_world, Sim_FoodSystem sim_foodSystem, int sim_foodValue) : base(sim_position, sim_radianRotation, sim_radius, Sim_ObjectTag.FOOD, sim_world)
        {
            this.sim_foodSystem = sim_foodSystem;
            this.sim_foodValue = sim_foodValue;
        }

        public void DeleteFood()
        {
            sim_foodSystem.RemoveFood(sim_foodID, sim_foodSystem.output);
            sim_circleCollider.DeleteCollider();
            DeleteSimWorldObject();
        }
    }
}
