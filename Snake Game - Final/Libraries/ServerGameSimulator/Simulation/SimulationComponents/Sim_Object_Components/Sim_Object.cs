using Contracts.DataContracts;
using GameSimulator;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;
using System;

namespace Server.Simulation.SimulationComponents.Sim_Object_Components
{
    public class Sim_Object
    {
        public Sim_World sim_world;
        
        //? DATA PACKET DATA ======================================================================================================
        // ID Data
        public int sim_worldObject_ID;
        public Sim_ObjectTag sim_objectTag { get; private set; }

        // Transform Data
        public Vector2 sim_position { get; protected set; }
        public Vector2 sim_size { get; set; }
        public Vector2 sim_centerToBoundLengths { get; private set; }
        public float sim_radianRotation { get; set; }
        //? DATA PACKET DATA ======================================================================================================


        public Sim_Object(Vector2 sim_position, float sim_radianRotation, Vector2 sim_size, Sim_ObjectTag sim_objectTag, Sim_World sim_world)
        {
            this.sim_world = sim_world;

            sim_worldObject_ID = sim_world.AddSimObject(this);

            this.sim_position = sim_position;
            this.sim_radianRotation = sim_radianRotation;
            this.sim_size = sim_size;
            sim_centerToBoundLengths = new Vector2(sim_size.X / 2f, sim_size.Y / 2f);
            this.sim_objectTag = sim_objectTag;
        }

        public virtual void UpdateSimWorldPosition(Vector2 sim_position)
        {
            this.sim_position = sim_position;
        }

        // Method to remove this collider from the list.
        protected void DeleteSimWorldObject()
        {
            sim_world.RemoveWorldObject(sim_worldObject_ID);
        }

        // Returns the radian rotation in terms of degrees.
        public float DegreeRotation()
        {
            return (float)(sim_radianRotation * 180.0f / Math.PI);
        }

        public Vector2 GetDirectionVector()
        {
            return new Vector2((float)Math.Cos(sim_radianRotation), (float)Math.Sin(sim_radianRotation));
        }
    }
}
