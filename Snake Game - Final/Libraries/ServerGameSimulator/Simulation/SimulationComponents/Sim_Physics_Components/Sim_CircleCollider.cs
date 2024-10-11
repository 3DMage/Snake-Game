using Contracts.DataContracts;
using GameSimulator;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;

namespace Server.Simulation.SimulationComponents.Sim_Physics_Components
{
    public class Sim_CircleCollider : Sim_Collider
    {
        public float radius { get; set; }

        public Sim_CircleCollider(Vector2 position, float radius, Sim_Object worldObject, Sim_GridCollisionHandling gridCollisionHandling) : base(position, worldObject, gridCollisionHandling)
        {
            this.radius = radius;
            centerToBoundLengths = new Vector2(radius, radius);

            colliderID = gridCollisionHandling.NextColliderID();
            gridCollisionHandling.circleColliders.Add(colliderID, this);
            gridCollisionHandling.QueueUpCircleColliderForUpdate(colliderID);
        }

        public void DeleteCollider()
        {
            IsPendingRemoval = true;
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
            gridCollisionHandling.QueueUpCircleColliderForUpdate(colliderID);
        }
    }
}
