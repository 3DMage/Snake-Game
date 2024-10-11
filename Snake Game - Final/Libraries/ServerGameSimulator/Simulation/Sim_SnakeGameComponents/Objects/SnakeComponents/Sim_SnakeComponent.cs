using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public abstract class Sim_SnakeComponent : Sim_PhysObject_Box
    {
        public Vector2 targetSimPosition;
        public Queue<Vector2> simPositionHistory = new Queue<Vector2>();
        public Sim_Snake sim_snake;
        public static float simSegmentRadius { get; private set; } = 20.0f;

        protected Sim_SnakeComponent(Vector2 sim_position, float sim_radianRotation, Sim_ObjectTag sim_objectTag, Sim_World sim_world, Sim_Snake sim_snake) : base(sim_position, sim_radianRotation, new Vector2(simSegmentRadius * 2, simSegmentRadius * 2), sim_objectTag, sim_world)
        {
            this.sim_snake = sim_snake;
        }

        public void UpdateSimPositionWithHistory(Vector2 newSimPosition)
        {
            // Store the new position in the history, ensuring we only keep the last five
            if (simPositionHistory.Count >= 3)
            {
                simPositionHistory.Dequeue(); // Remove the oldest if we exceed the history limit
            }

            simPositionHistory.Enqueue(newSimPosition);

            // Update the actual position of the component
            UpdateSimWorldPosition(newSimPosition);
            sim_boxCollider.UpdatePosition(newSimPosition);
        }


        public void DeleteSegment()
        {
            sim_boxCollider.DeleteCollider();
            DeleteSimWorldObject();
        }

        public void DeleteCollider()
        {
            sim_boxCollider.DeleteCollider();
        }

    }
}
