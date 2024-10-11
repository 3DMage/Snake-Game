using Contracts.DataContracts;
using Contracts.Interfaces;
using GameComponents;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameSimulator
{
    public class Simulator : ISimulator
    {


        public List<Output> outputs;

        public Sim_World simWorld { get; private set; }
        public Sim_SnakeSystem snakeSystem { get; private set; }
        public Sim_FoodSystem foodSystem { get; private set; }
        public bool running { get; private set; } = false;


        public Sim_DeathWall westWall { get; private set; }
        public Sim_DeathWall eastWall { get; private set; }
        public Sim_DeathWall northWall { get; private set; }
        public Sim_DeathWall southWall { get; private set; }



        // Add a field to track time since last food addition
        private TimeSpan timeSinceLastFood = TimeSpan.Zero;
        // Define the interval for adding food
        private readonly TimeSpan addFoodInterval = TimeSpan.FromSeconds(5);

        public Simulator()
        {
            outputs = new List<Output>();
            simWorld = new Sim_World(4200, 4200);
            foodSystem = new Sim_FoodSystem(simWorld);
            snakeSystem = new Sim_SnakeSystem(simWorld, foodSystem);
            running = true;

            westWall = new Sim_DeathWall(new Vector2(-2000f, 0), new Vector2(40f, 4000f), simWorld);
            eastWall = new Sim_DeathWall(new Vector2(2000f, 0), new Vector2(40f, 4000f), simWorld);
            northWall = new Sim_DeathWall(new Vector2(0, 2000f), new Vector2(4000f, 40f), simWorld);
            southWall = new Sim_DeathWall(new Vector2(0, -2000f), new Vector2(4000f, 40f), simWorld);


            foodSystem.InitializeFood();
        }

        public Output Update(Input input)
        {
            Output output = new Output();

            snakeSystem.InjectOutputObject(output);
            foodSystem.InjectOutputObject(output);

            // Handle join requests.
            for(int i = 0; i < input.clientJoinRequestDatas.Count; i++)
            {
                snakeSystem.AddSnake(input.clientJoinRequestDatas[i].playerName, input.clientJoinRequestDatas[i].clientID, output);
                foodSystem.GetFoodState(input.clientJoinRequestDatas[i].clientID, output);
                snakeSystem.CreateSnakeSystemStateData(input.clientJoinRequestDatas[i].clientID, output);

            }

            // Update the time since last food was added
            timeSinceLastFood += input.elapsedTime;

            // Check if it's time to add food
            if (timeSinceLastFood >= addFoodInterval)
            {
                // Reset the timer
                timeSinceLastFood = TimeSpan.Zero;
                // Add food
                Random random = new Random();
                int amountToAdd = random.Next(10, 15);
                for (int i = 0; i < amountToAdd; i++)
                {
                    foodSystem.AddFood(output);
                }
            }

            // Handle direction change requests
            for(int i = 0; i < input.directionChangeRequestDatas.Count; i++)
            {
                snakeSystem.ChangeSnakeDirection(input.directionChangeRequestDatas[i].clientID, input.directionChangeRequestDatas[i].radianAngle, output);
            }

            // Handle revive requests
            for (int i = 0; i < input.reviveSnakeRequestDatas.Count; i++)
            {
                snakeSystem.ReviveSnake(input.reviveSnakeRequestDatas[i].clientID, output);
            }

            snakeSystem.MoveSnakes(input.elapsedTime, output);

            simWorld.HandleCollisions();

            return output;
        }

        public async Task<bool> Connect()
        {
            return true;
        }

        public bool IsNetwork()
        {
            return false;
        }
 

        public void Disconnect()
        {
           
        }

        public List<Output> GetOutput()
        {
            List<Output> currentOutputs = new List<Output>();
            currentOutputs.AddRange(outputs);
            outputs.Clear();
            return currentOutputs;
        }

        public void SendInput(Input input)
        {
            Output currentOutput= Update(input);

            outputs.Add(currentOutput);
        }

        public Output RemoveSnakeRequest(int clientID)
        {
            Output output = new Output();
            snakeSystem.RemoveSnake(clientID, output);
            return output;
        }
    }
}
