using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;


namespace GameComponents
{
    public class Sim_Snake
    {
        public int clientID { get; private set; }

        public Sim_Head sim_head;
        public List<Sim_SnakeComponent> sim_segments;
        public float sim_speed = 500f;
        public int sim_amountEaten = 0;
        private int sim_foodValueSinceLastSegment = 0;
        public Sim_World sim_world;
        public Sim_SnakeSystem sim_snakeSystem;
        public float padding = 0.05f;
        public string playerName;

        public bool active = true;
        public bool invincible { get; private set; } = true;
        private TimeSpan invincibleTimer = TimeSpan.FromSeconds(3);
        private TimeSpan currentTime = TimeSpan.Zero;




        public Sim_Snake(int initialSize, Vector2 startingPosition, float initialRadianRotation, Sim_World sim_world, int clientID, string playerName, Sim_SnakeSystem sim_snakeSystem)
        {
            this.sim_snakeSystem = sim_snakeSystem;
            this.clientID = clientID;
            this.sim_world = sim_world;
            this.playerName = playerName;


            sim_segments = new List<Sim_SnakeComponent>();
            sim_head = new Sim_Head(startingPosition, this.sim_world, this);
            sim_head.sim_radianRotation = initialRadianRotation;
            sim_segments.Add(sim_head);

            for (int i = 1; i < initialSize; i++)
            {
                AddSegment();
            }
        }

        public Vector2 CloneVector(Vector2 vector)
        {
            Vector2 newVector = new Vector2(vector.X, vector.Y);
            return newVector;
        }

        public void PositionSnake(Vector2 position)
        {
            Vector2 newHeadPosition = new Vector2(position.X, position.Y);

            // Update the head's position and its history
            sim_head.UpdateSimPositionWithHistory(newHeadPosition);

            // Update each segment based on the fifth last position of the segment in front of it
            for (int i = 1; i < sim_segments.Count; i++)
            {
                if (sim_segments[i - 1].simPositionHistory.Count == 3)
                {
                    // We update this segment's position to the fifth last position of the segment ahead
                    Vector2 newPosition = sim_segments[i - 1].simPositionHistory.Peek();
                    sim_segments[i].UpdateSimPositionWithHistory(newPosition);
                }
            }
        }

        public void MoveSnake(TimeSpan gameTime)
        {
            if (active)
            {
                if(invincible)
                {
                    currentTime += gameTime;
                    if(currentTime >= invincibleTimer)
                    {
                        invincible = false;
                        MakeSnakeNotInvincibleData makeSnakeNotInvincible = new MakeSnakeNotInvincibleData(clientID);
                        sim_snakeSystem.output.makeSnakeNotInvincibleDatas.Add(makeSnakeNotInvincible);
                    }
                }

                // Calculate the distance the head moves this frame
                float moveDistance = sim_speed * (float)gameTime.TotalSeconds;
                Vector2 directionVector = sim_head.GetDirectionVector();

                Vector2 potentialPosition = sim_head.sim_position + directionVector * moveDistance;


                float x = Math.Clamp(potentialPosition.X, sim_world.sim_worldBoundary.upperLeft.X - sim_world.sim_worldBoundary.upperLeft.X * padding, sim_world.sim_worldBoundary.bottomRight.X - sim_world.sim_worldBoundary.bottomRight.X * padding);
                float y = Math.Clamp(potentialPosition.Y, sim_world.sim_worldBoundary.bottomRight.Y - sim_world.sim_worldBoundary.bottomRight.Y * padding, sim_world.sim_worldBoundary.upperLeft.Y - sim_world.sim_worldBoundary.upperLeft.Y * padding);



                Vector2 newHeadPosition = new Vector2(x, y);

                // Update the head's position and its history
                sim_head.UpdateSimPositionWithHistory(newHeadPosition);

                // Update each segment based on the fifth last position of the segment in front of it
                for (int i = 1; i < sim_segments.Count; i++)
                {
                    if (sim_segments[i - 1].simPositionHistory.Count == 3)
                    {
                        // We update this segment's position to the fifth last position of the segment ahead
                        Vector2 newPosition = sim_segments[i - 1].simPositionHistory.Peek();
                        sim_segments[i].UpdateSimPositionWithHistory(newPosition);
                    }
                }
            }
        }

        public void AddSegment()
        {
            // Reference to the last segment in the snake
            Sim_SnakeComponent lastSegment = sim_segments[^1];

            // Create a new body segment that is initially placed at the position of the last segment
            Sim_BodySegment newSegment = new Sim_BodySegment(lastSegment.sim_position, sim_world, this);

            // If the last segment has a position history, use it to initialize the new segment's history
            if (lastSegment.simPositionHistory.Count > 0)
            {
                // Copy the last 5 positions from the last segment's history to the new segment's history
                // This assumes the last segment has at least 5 positions in its history; adjust logic as needed
                var historyToCopy = new Queue<Vector2>(lastSegment.simPositionHistory.Reverse().Take(3));
                foreach (var position in historyToCopy)
                {
                    newSegment.simPositionHistory.Enqueue(position);
                }
            }
            else
            {
                // If the last segment does not have a history (unlikely, but just in case),
                // initialize the new segment's position history with its current position
                for (int i = 0; i < 3; i++) // Assuming a history size of 5
                {
                    newSegment.simPositionHistory.Enqueue(newSegment.sim_position);
                }
            }



            // Add the new segment to the snake
            sim_segments.Add(newSegment);
            sim_foodValueSinceLastSegment = 0;
        }

        public void AddSegments(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // Reference to the last segment in the snake
                Sim_SnakeComponent lastSegment = sim_segments[^1];

                // Create a new body segment that is initially placed at the position of the last segment
                Sim_BodySegment newSegment = new Sim_BodySegment(lastSegment.sim_position, sim_world, this);

                // If the last segment has a position history, use it to initialize the new segment's history
                if (lastSegment.simPositionHistory.Count > 0)
                {
                    // Copy the last 5 positions from the last segment's history to the new segment's history
                    // This assumes the last segment has at least 5 positions in its history; adjust logic as needed
                    var historyToCopy = new Queue<Vector2>(lastSegment.simPositionHistory.Reverse().Take(3));
                    foreach (var position in historyToCopy)
                    {
                        newSegment.simPositionHistory.Enqueue(position);
                    }
                }
                else
                {
                    // If the last segment does not have a history (unlikely, but just in case),
                    // initialize the new segment's position history with its current position
                    for (int k = 0; k < 3; k++) // Assuming a history size of 5
                    {
                        newSegment.simPositionHistory.Enqueue(newSegment.sim_position);
                    }
                }

                // Add the new segment to the snake
                sim_segments.Add(newSegment);
                sim_foodValueSinceLastSegment = 0;
            }
        }

        public void RemoveSegment()
        {
            sim_segments.RemoveAt(sim_segments.Count - 1);
        }

        public void RemoveSegments(int count)
        {
            for(int i =0; i < count; i++) 
            {
                sim_segments.RemoveAt(sim_segments.Count - 1);
            }
        }

        public void RemoveAllSegmentsButHead()
        {
            int segmentsToRemove = sim_segments.Count - 1;
            for (int i = 0; i < segmentsToRemove; i++)
            {
                sim_segments[sim_segments.Count - 1].DeleteSegment();
                sim_segments.RemoveAt(sim_segments.Count - 1);
            }
        }

        public void EatFood(int foodValue, Output output)
        {
            if (active)
            {


                sim_amountEaten += foodValue;
                sim_foodValueSinceLastSegment += foodValue;

                if (sim_foodValueSinceLastSegment >= 10)
                {
                    int segmentsToAdd = sim_foodValueSinceLastSegment / 10;
                    for (int i = 0; i < segmentsToAdd; i++)
                    {
                        AddSegment();
                    }
                    // Update foodValueSinceLastSegment to the remainder to handle any excess correctly
                    sim_foodValueSinceLastSegment %= 10;

                    ExpandSnakeData expandSnakeData = new ExpandSnakeData(clientID, segmentsToAdd);
                    output.expandSnakeDatas.Add(expandSnakeData);
                }
            }
        }

        public void ChangeDirection(float radianRotation)
        {
            sim_head.sim_radianRotation = radianRotation;
        }

        public void ResetInvincibility()
        {
            currentTime = TimeSpan.Zero;
            invincible = true;
        }
    }
}
