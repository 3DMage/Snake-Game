using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameComponents
{
    public class SnakeSystem
    {
        public Dictionary<int, Snake> snakes;
        public World world;

        public SnakeSystem(World world)
        {
            snakes = new Dictionary<int, Snake>();
            this.world = world;
        }

        public void AddSnake(int clientID, string playerName, List<Vector2> segmentPositions)
        {
            if (!snakes.ContainsKey(clientID))
            {
                Snake newSnake = new Snake(5, new Vector2(0, 0), 0, world, clientID, playerName, this);
                snakes.Add(clientID, newSnake);

                for (int i = 0; i < segmentPositions.Count; i++)
                {
                    newSnake.segments[i].UpdateWorldPosition(segmentPositions[i]);
                }
            }
        }

        public void RemoveSnake(int clientID)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes.Remove(clientID);
            }
        }

        public void ExpandSnake(int clientID, int segmentsToAdd)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].AddSegments(segmentsToAdd);
            }
        }



        public void ReviveSnake(int clientID, Vector2 position)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].AddSegments(4); // Spawn snake with 5 segments initially (head + 4 segements).
                snakes[clientID].PositionSnake(position);
                snakes[clientID].active = true;
                snakes[clientID].invincible = true;
            }
        }

        public void KillSnake(int clientID)
        {
            if (snakes.ContainsKey(clientID))
            {
                AudioManager.src_explosion.Play();
                snakes[clientID].active = false;
                snakes[clientID].RemoveAllSegmentsButHead();
            }
        }

        public void ChangeSnakeDirection(int clientID, float radianAngle)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].ChangeDirection(radianAngle);
            }
        }


        public void MoveSnakes(int clientID, List<Vector2> segmentPositions)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].UpdateSnakePosition(segmentPositions);
            }
        }

        public void DrawSnakes()
        {
            foreach (var snake in snakes.Values)
            {
                snake.Draw(world.viewportCamera);
            }
        }

        public void InitializeSnakeSystemState(List<int> clientIDs, List<string> playerNames, List<List<Vector2>> segmentPositions, List<bool> activeStates, List<bool> invincibleFlags)
        {
            for (int i = 0; i < clientIDs.Count; i++)
            {
                // Add each snake to the system with the provided details
                if (!snakes.ContainsKey(clientIDs[i]))
                {
                    Snake newSnake = new Snake(segmentPositions[i].Count, segmentPositions[i][0], 0, world, clientIDs[i], playerNames[i], this);
                    snakes.Add(clientIDs[i], newSnake);
                    snakes[clientIDs[i]].active = activeStates[i];
                    snakes[clientIDs[i]].invincible = invincibleFlags[i];
                }
            }
        }

        public List<int> GetTopFiveSnakesIDs()
        {
            List<Tuple<int, int>> scores = new List<Tuple<int, int>>();
            foreach (var snake in snakes.Values)
            {
                if (snake.active)
                {
                    scores.Add(new Tuple<int, int>(snake.clientID, snake.segments.Count));
                }
            }

            // Sort the list by the second value (score) in descending order
            var sortedScores = scores.OrderByDescending(t => t.Item2).ToList();

            // Extract the top five client IDs
            var topFiveClientIDs = sortedScores.Take(5).Select(t => t.Item1).ToList();

            return topFiveClientIDs;
        }
    }
}
