using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameComponents
{
    public class World
    {
        private int nextWorldObjectId = 0;
        public Dictionary<int, WorldObject> worldObjects { get; private set; }
        public Boundary worldBoundary { get; private set; }
        public Vector2 worldDimensions { get; private set; }
        public Camera viewportCamera;

        public World(float worldDimensionX, float worldDimensionY)
        {
            worldDimensions = new Vector2(worldDimensionX, worldDimensionY);
            Vector2 upperLeft = new Vector2(0 - (worldDimensionX / 2f), 0 + (worldDimensionY/ 2f));
            Vector2 bottomRight = new Vector2(0 + (worldDimensionX / 2f), 0 - (worldDimensionY/ 2f));

            worldBoundary = new Boundary(upperLeft, bottomRight);
            worldObjects = new Dictionary<int, WorldObject>();
        }

        











        // Method to add a WorldObject to the dictionary
        public int AddWorldObject(WorldObject worldObject)
        {
            int id = nextWorldObjectId++;
            worldObjects[id] = worldObject;
            return id; // Return the ID assigned to the object
        }

        // Simplified method to remove a WorldObject
        public void RemoveWorldObject(int id)
        {
            worldObjects.Remove(id);
        }

       
    }
}
