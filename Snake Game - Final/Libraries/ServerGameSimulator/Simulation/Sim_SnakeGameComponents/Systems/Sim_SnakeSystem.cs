using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_SnakeSystem
    {
        public Dictionary<int, Sim_Snake> snakes;
        public Sim_World sim_world;
        Sim_FoodSystem foodSystem;
        public Output output { get; private set; }

        public Sim_SnakeSystem(Sim_World world, Sim_FoodSystem foodSystem)
        {
            snakes = new Dictionary<int, Sim_Snake>();
            this.sim_world = world;
            this.foodSystem = foodSystem;
        }

        public void AddSnake(string playerName, int clientID, Output output)
        {
            Sim_Snake newSnake = new Sim_Snake(5, sim_world.GenerateRandomCoordinate(15), 0, sim_world, clientID, playerName, this);
            //Sim_Snake newSnake = new Sim_Snake(5, new Vector2(0,0), 0, sim_world, clientID, playerName, this);
            if(!snakes.ContainsKey(clientID))
            {
                snakes.Add(clientID, newSnake);
            }

            List<Vector2> positionList = newSnake.sim_segments.Select(segment => segment.sim_position).ToList();

            

            AddSnakeData addSnakeData = new AddSnakeData(clientID, playerName, positionList);
            output.addSnakeDatas.Add(addSnakeData);
        }

        public void InjectOutputObject(Output output)
        {
            this.output = output;
        }

        public void RemoveSnake(int clientID, Output output) 
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].RemoveAllSegmentsButHead();
                snakes[clientID].sim_head.DeleteSegment();
                snakes.Remove(clientID);

                RemoveSnakeData removeSnakeData = new RemoveSnakeData(clientID);
                output.removeSnakeDatas.Add(removeSnakeData);
            }
        }

        public void ReviveSnake(int clientID, Output output)
        {
            if (snakes.ContainsKey(clientID))
            {
                Vector2 spawnPosition = sim_world.GenerateRandomCoordinate(10);

                snakes[clientID].PositionSnake(spawnPosition);
                snakes[clientID].sim_head.simPositionHistory.Clear();
                snakes[clientID].AddSegments(4); // Spawn snake with 5 segments initially (head + 4 segements).
                snakes[clientID].active = true;
                snakes[clientID].ResetInvincibility();

                ReviveSnakeData reviveSnakeData = new ReviveSnakeData(clientID, spawnPosition);
                output.reviveSnakeDatas.Add(reviveSnakeData);
            }
        }

        public void KillSnake(int clientID, Output output)
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].active = false;

                for (int i = 0; i < snakes[clientID].sim_segments.Count; i++)
                {
                    foodSystem.AddFoodAt(snakes[clientID].sim_segments[i].sim_position, 5, output);
                }

                snakes[clientID].RemoveAllSegmentsButHead();
                KillSnakeData killSnakeData = new KillSnakeData(clientID);
                output.killSnakeDatas.Add(killSnakeData);
            }
        }

        public void MoveSnakes(TimeSpan gameTime, Output output)
        {
            //? Construct a message to list where all snakes moved.

            foreach(var snake in snakes.Values)
            { 
                snake.MoveSnake(gameTime);

                List<Vector2> positionList = snake.sim_segments.Select(segment => segment.sim_position).ToList();
                MoveSnakeData moveSnakeData = new MoveSnakeData(snake.clientID, positionList);
                output.moveSnakeDatas.Add(moveSnakeData);
            }
        }

        public void ChangeSnakeDirection(int clientID, float radianAngle,  Output output) 
        {
            if (snakes.ContainsKey(clientID))
            {
                snakes[clientID].ChangeDirection(radianAngle);
                ChangeSnakeAngleData changeSnakeAngleData = new ChangeSnakeAngleData(clientID, radianAngle);
                output.changeSnakeAngleDatas.Add(changeSnakeAngleData);
            }
        }


        public void CreateSnakeSystemStateData(int sendToClientID, Output output)
        {

            List<int> clientIDs = new List<int>();
            List<string> playerNames = new List<string>();
            List<List<Vector2>> segmentPositions = new List<List<Vector2>>();
            List<bool> activeStates = new List<bool>();
            List<bool> invincibleFlags = new List<bool>();


            foreach (var snake in snakes.Values)
            {
                clientIDs.Add(snake.clientID);
                playerNames.Add(snake.playerName);
                activeStates.Add(snake.active);
                invincibleFlags.Add(snake.invincible);

                List<Vector2> positions = new List<Vector2>();
                foreach (var segment in snake.sim_segments)
                {
                    positions.Add(segment.sim_position);
                }
                segmentPositions.Add(positions);
            }

            SnakeSystemStateData snakeSystemStateData = new SnakeSystemStateData(sendToClientID, clientIDs, playerNames, segmentPositions, activeStates, invincibleFlags);
            output.snakeSystemStatesDatas.Add(snakeSystemStateData);
        }
    }
}
