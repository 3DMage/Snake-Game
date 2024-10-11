using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;

namespace Server.Simulation.SimulationComponents.Sim_Physics_Components
{
    public class Sim_BoxCollider : Sim_Collider
    {

        public Vector2 size;

        public Sim_BoxCollider(Vector2 position, Vector2 size, Sim_Object worldObject, Sim_GridCollisionHandling gridCollisionHandling) : base(position, worldObject, gridCollisionHandling)
        {
            this.size = size;
            centerToBoundLengths = new Vector2(size.X / 2f, size.Y / 2f);

            colliderID = gridCollisionHandling.NextColliderID();
            gridCollisionHandling.boxColliders.Add(colliderID, this);
            gridCollisionHandling.QueueUpBoxColliderForUpdate(colliderID);
        }

        // Method to remove this collider from the list.
        public void DeleteCollider()
        {
            IsPendingRemoval = true;
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
            gridCollisionHandling.QueueUpBoxColliderForUpdate(colliderID);
        }
    }
}
