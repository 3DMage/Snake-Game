using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_FoodSystem
    {
        public Sim_World sim_world {  get; private set; }
        public Dictionary<int, Sim_Food> foodPool { get; private set; }
        private int nextFoodId = 0;
        private int foodLimit = 500;
        public Output output { get; private set; }


        public Sim_FoodSystem(Sim_World sim_world)
        {
            this.sim_world = sim_world;
            foodPool = new Dictionary<int, Sim_Food>();   
        }


        public void InjectOutputObject(Output output)
        {
            this.output = output;

          
        }

        public void InitializeFood()
        {
            for (int i = 0; i < 75; i++)
            {
                Sim_Food food = new Sim_Food(sim_world.GenerateRandomCoordinate(5), 0, 40, sim_world, this, 1);
                foodPool[nextFoodId] = food;
                food.sim_foodID = nextFoodId++;
            }
        }

        public void AddFood(Output output)
        {
            if (foodPool.Count < foodLimit)
            {
                Sim_Food food = new Sim_Food(sim_world.GenerateRandomCoordinate(5), 0, 40, sim_world, this, 1);
                foodPool[nextFoodId] = food; 
                food.sim_foodID = nextFoodId++;

                FoodSpawnData foodSpawnData = new FoodSpawnData(food.sim_position.X, food.sim_position.Y, food.sim_foodID);
                output.foodSpawnDatas.Add(foodSpawnData);
            }
        }



        public void AddFoodAt(Vector2 position, int foodValue, Output output)
        {
            // Generate random color
            Sim_Food food = new Sim_Food(position, 0, 40, sim_world, this, foodValue);
            foodPool[nextFoodId] = food;
            food.sim_foodID = nextFoodId++;

            FoodSpawnData foodSpawnData = new FoodSpawnData(food.sim_position.X, food.sim_position.Y, food.sim_foodID);
            output.foodSpawnDatas.Add(foodSpawnData);
        }

        public void RemoveFood(int foodID, Output output)
        {
            if (foodPool.ContainsKey(foodID))
            {
                foodPool.Remove(foodID);

                FoodDeleteData foodDeleteData = new FoodDeleteData(foodID);
                output.foodDeleteDatas.Add(foodDeleteData);
            }
        }

        public void GetFoodState(int clientID, Output output)
        {
            List<int> foodIDs = new List<int>();
            List<float> foodPosXs = new List<float>();
            List<float> foodPosYs = new List<float>();


            foreach (var food in foodPool.Values)
            {
                foodIDs.Add(food.sim_foodID);
                foodPosXs.Add(food.sim_position.X);
                foodPosYs.Add(food.sim_position.Y);
            }

            FoodStateData foodStateData = new FoodStateData(clientID, foodIDs, foodPosXs, foodPosYs);
            output.foodStateDatas.Add(foodStateData);
        }
    }
}
