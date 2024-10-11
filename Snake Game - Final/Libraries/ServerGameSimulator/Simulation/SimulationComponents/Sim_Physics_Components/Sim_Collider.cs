using Contracts.DataContracts;
using GameSimulator;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using System.Collections.Generic;

namespace Server.Simulation.SimulationComponents.Sim_Physics_Components
{
    public delegate void OnCollideAction(Sim_Object worldObject);

    public abstract class Sim_Collider
    {
        public int colliderID;

        public List<(int, int)> occupiedGridCells;

        public HashSet<int> collidedWith;

        public bool IsPendingRemoval { get; set; } = false;

        public Dictionary<Sim_ObjectTag, OnCollideAction> collisionActions { get; private set; }

        public Sim_Object sim_Object { get; private set; }

        public Vector2 position { get; protected set; }
        public Vector2 centerToBoundLengths { get; protected set; }


        protected Sim_GridCollisionHandling gridCollisionHandling;

        public Sim_Collider(Vector2 position, Sim_Object worldObject, Sim_GridCollisionHandling gridCollisionHandling)
        {
            collisionActions = new Dictionary<Sim_ObjectTag, OnCollideAction>();
            this.position = position;
            occupiedGridCells = new List<(int, int)>();
            this.gridCollisionHandling = gridCollisionHandling;
            sim_Object = worldObject;
            collidedWith = new HashSet<int>();
        }

        public void AddCollisionAction(Sim_ObjectTag collidedInto_Type, OnCollideAction collisionAction)
        {
            collisionActions.Add(collidedInto_Type, collisionAction);
        }

        public void InvokeCollisionAction(Sim_ObjectTag collidedInto_Type, Sim_Object sim_Object)
        {
            if (collisionActions.ContainsKey(collidedInto_Type) && !collidedWith.Contains(sim_Object.sim_worldObject_ID))
            {
                collisionActions[collidedInto_Type](sim_Object);
            }
        }
    }
}
