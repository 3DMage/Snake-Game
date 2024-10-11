using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;


namespace Server.Simulation.SimulationComponents.Sim_Physics_Components
{
    public class Sim_GridCollisionHandling
    {
        private int gridPartitions_X = 50;
        private int gridPartitions_Y = 50;
        private float gridPartitions_X_Length;
        private float gridPartitions_Y_Length;

        public Dictionary<int, Sim_CircleCollider> circleColliders = new Dictionary<int, Sim_CircleCollider>();
        public Dictionary<int, Sim_BoxCollider> boxColliders = new Dictionary<int, Sim_BoxCollider>();
        public GridCell_ColliderIndicesSets[,] gridItems { get; private set; }
        private int nextColliderID = 0;

        public List<int> circleCollidersToBeUpdated;
        public List<int> boxCollidersToBeUpdated;
        public Sim_Boundary gridCollisionBoundary;
        Vector2 gridOrigin;

        public Sim_GridCollisionHandling(Sim_Boundary boundary)
        {
            gridCollisionBoundary = boundary;

            gridOrigin = new Vector2(gridCollisionBoundary.upperLeft.X, gridCollisionBoundary.upperLeft.Y);

            gridPartitions_X_Length = (boundary.bottomRight.X - boundary.upperLeft.X) / gridPartitions_X;
            gridPartitions_Y_Length = (boundary.upperLeft.Y - boundary.bottomRight.Y) / gridPartitions_Y;


            gridItems = new GridCell_ColliderIndicesSets[gridPartitions_X, gridPartitions_Y];

            for (int x = 0; x < gridPartitions_X; x++)
            {
                for (int y = 0; y < gridPartitions_Y; y++)
                {
                    gridItems[x, y] = new GridCell_ColliderIndicesSets();
                }
            }

            circleCollidersToBeUpdated = new List<int>();
            boxCollidersToBeUpdated = new List<int>();
        }

        public void QueueUpCircleColliderForUpdate(int ID)
        {
            circleCollidersToBeUpdated.Add(ID);
        }

        public void QueueUpBoxColliderForUpdate(int ID)
        {
            boxCollidersToBeUpdated.Add(ID);
        }

        public void BroadPhase()
        {
            for (int i = 0; i < circleCollidersToBeUpdated.Count; i++)
            {
                Sim_CircleCollider currentCircleCollider = circleColliders[circleCollidersToBeUpdated[i]];

                // Clear corresponding grid indices in the grid items array.
                for (int k = 0; k < currentCircleCollider.occupiedGridCells.Count; k++)
                {
                    (int, int) currentGridCell = currentCircleCollider.occupiedGridCells[k];
                    gridItems[currentGridCell.Item1, currentGridCell.Item2].circleColliderIDs.Remove(currentCircleCollider.colliderID);
                }

                currentCircleCollider.occupiedGridCells.Clear();

                float upperLeftCorner_X = currentCircleCollider.position.X - currentCircleCollider.centerToBoundLengths.X;
                float upperLeftCorner_Y = currentCircleCollider.position.Y + currentCircleCollider.centerToBoundLengths.Y;

                float bottomRightCorner_X = currentCircleCollider.position.X + currentCircleCollider.centerToBoundLengths.X;
                float bottomRightCorner_Y = currentCircleCollider.position.Y - currentCircleCollider.centerToBoundLengths.Y;

                Vector2 normalized_upperLeftCorner = new Vector2(upperLeftCorner_X - gridOrigin.X, -upperLeftCorner_Y + gridOrigin.Y);
                Vector2 normalized_bottomRightCorner = new Vector2(bottomRightCorner_X - gridOrigin.X, -bottomRightCorner_Y + gridOrigin.Y);

                int leftX = (int)(normalized_upperLeftCorner.X / gridPartitions_X_Length);
                int upperY = (int)(normalized_upperLeftCorner.Y / gridPartitions_Y_Length);

                int rightX = (int)(normalized_bottomRightCorner.X / gridPartitions_X_Length);
                int bottomY = (int)(normalized_bottomRightCorner.Y / gridPartitions_Y_Length);

                for (int x = leftX; x <= rightX; x++)
                {
                    for (int y = upperY; y <= bottomY; y++)
                    {
                        currentCircleCollider.occupiedGridCells.Add((x, y));
                        gridItems[x, y].circleColliderIDs.Add(currentCircleCollider.colliderID);
                    }
                }
            }

            circleCollidersToBeUpdated.Clear();


            for (int i = 0; i < boxCollidersToBeUpdated.Count; i++)
            {
                Sim_BoxCollider currentBoxCollider = boxColliders[boxCollidersToBeUpdated[i]];

                // Clear corresponding grid indices in the grid items array.
                for (int k = 0; k < currentBoxCollider.occupiedGridCells.Count; k++)
                {
                    (int, int) currentGridCell = currentBoxCollider.occupiedGridCells[k];
                    gridItems[currentGridCell.Item1, currentGridCell.Item2].boxColliderIDs.Remove(currentBoxCollider.colliderID);
                }

                currentBoxCollider.occupiedGridCells.Clear();

                float upperLeftCorner_X = currentBoxCollider.position.X - currentBoxCollider.centerToBoundLengths.X;
                float upperLeftCorner_Y = currentBoxCollider.position.Y + currentBoxCollider.centerToBoundLengths.Y;

                float bottomRightCorner_X = currentBoxCollider.position.X + currentBoxCollider.centerToBoundLengths.X;
                float bottomRightCorner_Y = currentBoxCollider.position.Y - currentBoxCollider.centerToBoundLengths.Y;

                Vector2 normalized_upperLeftCorner = new Vector2(upperLeftCorner_X - gridOrigin.X, -upperLeftCorner_Y + gridOrigin.Y);
                Vector2 normalized_bottomRightCorner = new Vector2(bottomRightCorner_X - gridOrigin.X, -bottomRightCorner_Y + gridOrigin.Y);

                int leftX = (int)(normalized_upperLeftCorner.X / gridPartitions_X_Length);
                int upperY = (int)(normalized_upperLeftCorner.Y / gridPartitions_Y_Length);

                int rightX = (int)(normalized_bottomRightCorner.X / gridPartitions_X_Length);
                int bottomY = (int)(normalized_bottomRightCorner.Y / gridPartitions_Y_Length);

                for (int x = leftX; x <= rightX; x++)
                {
                    for (int y = upperY; y <= bottomY; y++)
                    {
                        currentBoxCollider.occupiedGridCells.Add((x, y));
                        gridItems[x, y].boxColliderIDs.Add(currentBoxCollider.colliderID);
                    }
                }
            }

            boxCollidersToBeUpdated.Clear();
        }

        public int NextColliderID()
        {
            return nextColliderID++;
        }

        public void NarrowPhase()
        {
            for (int x = 0; x < gridPartitions_X; x++)
            {
                for (int y = 0; y < gridPartitions_Y; y++)
                {
                    GridCell_ColliderIndicesSets currentPartitionItems = gridItems[x, y];

                    // Circle to Circle
                    for (int i = 0; i < currentPartitionItems.circleColliderIDs.Count; i++)
                    {
                        int colliderID_At_i = currentPartitionItems.circleColliderIDs.ElementAt(i);

                        for (int j = i + 1; j < currentPartitionItems.circleColliderIDs.Count; j++)
                        {
                            int colliderID_At_j = currentPartitionItems.circleColliderIDs.ElementAt(j);

                            if (CircleToCircle_CollisionCheck(circleColliders[colliderID_At_i], circleColliders[colliderID_At_j]))
                            {
                                circleColliders[colliderID_At_i].InvokeCollisionAction(circleColliders[colliderID_At_j].sim_Object.sim_objectTag, circleColliders[colliderID_At_j].sim_Object);
                                circleColliders[colliderID_At_j].InvokeCollisionAction(circleColliders[colliderID_At_i].sim_Object.sim_objectTag, circleColliders[colliderID_At_i].sim_Object);

                                if (!circleColliders[colliderID_At_i].collidedWith.Contains(circleColliders[colliderID_At_j].sim_Object.sim_worldObject_ID))
                                {
                                    circleColliders[colliderID_At_i].collidedWith.Add(circleColliders[colliderID_At_j].sim_Object.sim_worldObject_ID);
                                }

                                if (!circleColliders[colliderID_At_j].collidedWith.Contains(circleColliders[colliderID_At_i].sim_Object.sim_worldObject_ID))
                                {
                                    circleColliders[colliderID_At_j].collidedWith.Add(circleColliders[colliderID_At_i].sim_Object.sim_worldObject_ID);
                                }
                            }
                        }
                    }

                    // Box to Box
                    for (int i = 0; i < currentPartitionItems.boxColliderIDs.Count; i++)
                    {
                        int colliderID_At_i = currentPartitionItems.boxColliderIDs.ElementAt(i);

                        for (int j = i + 1; j < currentPartitionItems.boxColliderIDs.Count; j++)
                        {
                            int colliderID_At_j = currentPartitionItems.boxColliderIDs.ElementAt(j);

                            if (BoxToBox_CollisionCheck(boxColliders[colliderID_At_i], boxColliders[colliderID_At_j]))
                            {
                                boxColliders[colliderID_At_i].InvokeCollisionAction(boxColliders[colliderID_At_j].sim_Object.sim_objectTag, boxColliders[colliderID_At_j].sim_Object);
                                boxColliders[colliderID_At_j].InvokeCollisionAction(boxColliders[colliderID_At_i].sim_Object.sim_objectTag, boxColliders[colliderID_At_i].sim_Object);

                                if (!boxColliders[colliderID_At_i].collidedWith.Contains(boxColliders[colliderID_At_j].sim_Object.sim_worldObject_ID))
                                {
                                    boxColliders[colliderID_At_i].collidedWith.Add(boxColliders[colliderID_At_j].sim_Object.sim_worldObject_ID);
                                }

                                if (!boxColliders[colliderID_At_j].collidedWith.Contains(boxColliders[colliderID_At_i].sim_Object.sim_worldObject_ID))
                                {
                                    boxColliders[colliderID_At_j].collidedWith.Add(boxColliders[colliderID_At_i].sim_Object.sim_worldObject_ID);
                                }
                            }
                        }
                    }

                    // Circle to Box
                    for (int i = 0; i < currentPartitionItems.circleColliderIDs.Count; i++)
                    {
                        int colliderID_At_i = currentPartitionItems.circleColliderIDs.ElementAt(i);

                        for (int j = 0; j < currentPartitionItems.boxColliderIDs.Count; j++)
                        {
                            int colliderID_At_j = currentPartitionItems.boxColliderIDs.ElementAt(j);

                            if (CircleToBox_CollisionCheck(circleColliders[colliderID_At_i], boxColliders[colliderID_At_j]))
                            {
                                circleColliders[colliderID_At_i].InvokeCollisionAction(boxColliders[colliderID_At_j].sim_Object.sim_objectTag, boxColliders[colliderID_At_j].sim_Object);
                                boxColliders[colliderID_At_j].InvokeCollisionAction(circleColliders[colliderID_At_i].sim_Object.sim_objectTag, circleColliders[colliderID_At_i].sim_Object);

                                if (!circleColliders[colliderID_At_i].collidedWith.Contains(boxColliders[colliderID_At_j].sim_Object.sim_worldObject_ID))
                                {
                                    circleColliders[colliderID_At_i].collidedWith.Add(boxColliders[colliderID_At_j].sim_Object.sim_worldObject_ID);
                                }

                                if (!boxColliders[colliderID_At_j].collidedWith.Contains(circleColliders[colliderID_At_i].sim_Object.sim_worldObject_ID))
                                {
                                    boxColliders[colliderID_At_j].collidedWith.Add(circleColliders[colliderID_At_i].sim_Object.sim_worldObject_ID);
                                }
                            }
                        }
                    }
                }
            }

            // Clear collided with data for next frame.
            foreach (var collider in circleColliders.Values)
            {
                collider.collidedWith.Clear();
            }

            foreach (var collider in boxColliders.Values)
            {
                collider.collidedWith.Clear();
            }
        }

        private bool CircleToCircle_CollisionCheck(Sim_CircleCollider circle1, Sim_CircleCollider circle2)
        {
           
            float distance = Vector2.Distance(circle1.position, circle2.position);
            return distance < circle1.radius + circle2.radius;
        }

        private bool BoxToBox_CollisionCheck(Sim_BoxCollider box1, Sim_BoxCollider box2)
        {
            return box1.position.X - box1.centerToBoundLengths.X < box2.position.X + box2.centerToBoundLengths.X &&
                   box1.position.X + box1.centerToBoundLengths.X > box2.position.X - box2.centerToBoundLengths.X &&
                   box1.position.Y - box1.centerToBoundLengths.Y < box2.position.Y + box2.centerToBoundLengths.Y &&
                   box1.position.Y + box1.centerToBoundLengths.Y > box2.position.Y - box2.centerToBoundLengths.Y;
        }

        private bool CircleToBox_CollisionCheck(Sim_CircleCollider circle, Sim_BoxCollider box)
        {
            Vector2 closestPoint = Vector2.Clamp(circle.position, box.position - box.centerToBoundLengths, box.position + box.centerToBoundLengths);
            float distance = Vector2.Distance(circle.position, closestPoint);
            return distance < circle.radius;
        }

        public void ClearUpdateQueues()
        {
            circleCollidersToBeUpdated.Clear();
            boxCollidersToBeUpdated.Clear();
        }

        public void PostCollisionCleanup()
        {
            var circleColliderIDsToRemove = circleColliders.Where(kvp => kvp.Value.IsPendingRemoval)
                                                            .Select(kvp => kvp.Key)
                                                            .ToList();

            var boxColliderIDsToRemove = boxColliders.Where(kvp => kvp.Value.IsPendingRemoval)
                                                      .Select(kvp => kvp.Key)
                                                      .ToList();

            // Remove from dictionaries
            foreach (var id in circleColliderIDsToRemove)
            {
                var collider = circleColliders[id];
                circleColliders.Remove(id);
                // Also remove from spatial grid
                RemoveColliderFromGrid(collider);
            }
            foreach (var id in boxColliderIDsToRemove)
            {
                var collider = boxColliders[id];
                boxColliders.Remove(id);
                // Also remove from spatial grid
                RemoveColliderFromGrid(collider);
            }
        }

        private void RemoveColliderFromGrid(Sim_Collider collider)
        {
            foreach (var cell in collider.occupiedGridCells)
            {
                gridItems[cell.Item1, cell.Item2].RemoveColliderID(collider.colliderID, collider is Sim_CircleCollider ? ColliderType.Circle : ColliderType.Box);
            }
        }
    }












    public class GridCell_ColliderIndicesSets
    {
        public HashSet<int> circleColliderIDs;
        public HashSet<int> boxColliderIDs;

        public GridCell_ColliderIndicesSets()
        {
            circleColliderIDs = new HashSet<int>();
            boxColliderIDs = new HashSet<int>();
        }

        public void RemoveColliderID(int id, ColliderType type)
        {
            if (type == ColliderType.Circle)
            {
                circleColliderIDs.Remove(id);
            }
            else if (type == ColliderType.Box)
            {
                boxColliderIDs.Remove(id);
            }
        }
    }

    public enum ColliderType
    {
        Circle,
        Box
    }
}
