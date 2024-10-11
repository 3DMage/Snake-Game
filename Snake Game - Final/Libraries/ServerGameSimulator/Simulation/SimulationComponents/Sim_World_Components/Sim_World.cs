using Contracts.DataContracts;
using GameSimulator;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_Physics_Components;
using System;
using System.Collections.Generic;

namespace Server.Simulation.SimulationComponents.Sim_World_Components
{
    public class Sim_World
    {
        private int nextSimObjectID = 0;
        public Sim_Boundary sim_worldBoundary { get; private set; }
        public Sim_GridCollisionHandling sim_gridCollisionHandling { get; private set; }
        public Vector2 sim_worldDimensions { get; private set; }
        public Dictionary<int, Sim_Object> sim_Objects { get; private set; }


        public Sim_World(float worldDimensionX, float worldDimensionY)
        {
            sim_worldDimensions = new Vector2(worldDimensionX, worldDimensionY);
            Vector2 upperLeft = new Vector2(0 - worldDimensionX / 2f, 0 + worldDimensionY / 2f);
            Vector2 bottomRight = new Vector2(0 + worldDimensionX / 2f, 0 - worldDimensionY / 2f);

            sim_worldBoundary = new Sim_Boundary(upperLeft, bottomRight);
            sim_Objects = new Dictionary<int, Sim_Object>();
            sim_gridCollisionHandling = new Sim_GridCollisionHandling(sim_worldBoundary);
        }

        // Method to add a WorldObject to the dictionary
        public int AddSimObject(Sim_Object worldObject)
        {
            int id = nextSimObjectID++;
            sim_Objects[id] = worldObject;
            return id; // Return the ID assigned to the object
        }

        // Simplified method to remove a WorldObject
        public void RemoveWorldObject(int id)
        {
            sim_Objects.Remove(id);
        }

        public void HandleCollisions()
        {
            sim_gridCollisionHandling.BroadPhase();

            sim_gridCollisionHandling.NarrowPhase();

            sim_gridCollisionHandling.PostCollisionCleanup();
        }

        public Vector2 GenerateRandomCoordinate(float marginPercent)
        {
            // Ensure the margin percentage is between 0 and 100
            marginPercent = Math.Clamp(marginPercent, 0, 100);

            // Convert marginPercent to a multiplier for dimensions
            float marginMultiplierX = sim_worldDimensions.X * (marginPercent / 100f);
            float marginMultiplierY = sim_worldDimensions.Y * (marginPercent / 100f);

            // Calculate the valid range for X and Y coordinates
            float minX = sim_worldBoundary.upperLeft.X + marginMultiplierX;
            float maxX = sim_worldBoundary.bottomRight.X - marginMultiplierX;
            float minY = sim_worldBoundary.bottomRight.Y + marginMultiplierY;
            float maxY = sim_worldBoundary.upperLeft.Y - marginMultiplierY;

            // Generate a random coordinate within the valid range
            Random rand = new Random();
            float x = (float)(minX + rand.NextDouble() * (maxX - minX));
            float y = (float)(minY + rand.NextDouble() * (maxY - minY));

            return new Vector2(x, y);
        }
    }
}
