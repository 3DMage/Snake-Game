using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Server.Simulation.SimulationComponents.Sim_Object_Components;
using Server.Simulation.SimulationComponents.Sim_World_Components;

namespace GameComponents
{
    public class Sim_Head : Sim_SnakeComponent
    {


        public Sim_Head(Vector2 sim_position, Sim_World sim_world, Sim_Snake sim_snake) : base(sim_position, 0, Sim_ObjectTag.HEAD, sim_world, sim_snake)
        {
            sim_boxCollider.collisionActions.Add(Sim_ObjectTag.HEAD, OnCollide_Head);
            sim_boxCollider.collisionActions.Add(Sim_ObjectTag.FOOD, OnCollide_Food);
            sim_boxCollider.collisionActions.Add(Sim_ObjectTag.BODY_SEGMENT, OnCollide_BodySegment);
            sim_boxCollider.collisionActions.Add(Sim_ObjectTag.DEATH_WALL, OnCollide_DeathWall);
        }



        public void OnCollide_Head(Sim_Object sim_Object)
        {
            if (sim_snake.active && !sim_snake.invincible)
            {
                //? You are dead
                Sim_Head otherHead = (Sim_Head)sim_Object;
                if (otherHead.sim_snake.active && !otherHead.sim_snake.invincible)
                {
                    sim_snake.sim_snakeSystem.KillSnake(sim_snake.clientID, sim_snake.sim_snakeSystem.output);
                    sim_snake.sim_snakeSystem.KillSnake(otherHead.sim_snake.clientID, otherHead.sim_snake.sim_snakeSystem.output);
                }
            }        
        }

        public void OnCollide_Food(Sim_Object sim_Object)
        {
            if (sim_snake.active)
            {
                //? You get bigger.
                Sim_Food food = (Sim_Food)sim_Object;
                sim_snake.EatFood(food.sim_foodValue, sim_snake.sim_snakeSystem.output);
                food.DeleteFood();
            }
        }
    

        public void OnCollide_BodySegment(Sim_Object sim_Object)
        {
            if (sim_snake.active && !sim_snake.invincible)
            {
                //? You are dead.
                Sim_BodySegment bodySegment = (Sim_BodySegment)sim_Object;
                if (bodySegment.sim_snake.clientID != sim_snake.clientID && !bodySegment.sim_snake.invincible)
                {
                    sim_snake.sim_snakeSystem.KillSnake(sim_snake.clientID, sim_snake.sim_snakeSystem.output);
                }
            }
        }

        public void OnCollide_DeathWall(Sim_Object sim_Object) 
        {
            if (sim_snake.active)
            {
                //? You are dead.
                sim_snake.sim_snakeSystem.KillSnake(sim_snake.clientID, sim_snake.sim_snakeSystem.output);
            }
        }
    }
}
