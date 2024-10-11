using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameComponents
{
    public class FoodSystem
    {
        Random random = new Random();
        public World world { get; private set; }
        public Dictionary<int, Food> foodPool { get; private set; }

        public FoodSystem(World world)
        {
            this.world = world;
            foodPool = new Dictionary<int, Food>();
        }

        public void AddFood(Vector2 position, int foodID)
        {
            if (!foodPool.ContainsKey(foodID))
            {
                // Generate random color
                int baseValue = 127; // Starting from a medium brightness ensures that the colors are lighter
                Color randomColor = new Color
                (
                    Math.Min(255, baseValue + random.Next(128)), // Red component
                    Math.Min(255, baseValue + random.Next(128)), // Green component
                    Math.Min(255, baseValue + random.Next(128))  // Blue component
                );

                Food food = new Food(position, 0, 40, randomColor, world);
                food.foodID = foodID;
                foodPool.Add(food.foodID, food);
            }
        }

        public void RemoveFood(int foodID)
        {
            if (foodPool.ContainsKey(foodID))
            {
                foodPool[foodID].DeleteFood();
                foodPool.Remove(foodID);
            }
        }

        public void InitializeFoodState(List<int> foodIDs, List<float> foodPosXs, List<float> foodPosYs)
        {


            for (int i = 0; i < foodIDs.Count; i++)
            {
                if (!foodPool.ContainsKey(foodIDs[i]))
                {


                    int baseValue = 127; // Starting from a medium brightness ensures that the colors are lighter

                    Color randomColor = new Color
                   (
                       Math.Min(255, baseValue + random.Next(128)), // Red component
                       Math.Min(255, baseValue + random.Next(128)), // Green component
                       Math.Min(255, baseValue + random.Next(128))  // Blue component
                   );

                    Food food = new Food(new Vector2(foodPosXs[i], foodPosYs[i]), 0, 40, randomColor, world);

                    foodPool.Add(foodIDs[i], food);
                }
            }
        }


        public void DrawAllFood()
        {
            foreach (var food in foodPool.Values)
            {
                food.Draw(world.viewportCamera);
            }
        }
    }
}
