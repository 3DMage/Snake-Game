using Client.__GAME.SnakeGame.Components.DeathWallComponents;
using Client.GameComponents.UI.GameUI;
using Client.SimulatorStuff;
using Contracts.DataContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shared.Datastructures;
using System.Collections.Generic;

namespace GameComponents
{
    public class Sc_SnakeGame : Scene
    {
        //? ASSETS ===============================================================
        public World world;
        public SnakeSystem snakeSystem;
        public FoodSystem foodSystem;

        public DeathWall westWall { get; private set; }
        public DeathWall eastWall { get; private set; }
        public DeathWall northWall { get; private set; }
        public DeathWall southWall { get; private set; }


        bool initialized = false;



        public LevelGround worldBorder;


        InputCommand MoveUp;
        InputCommand MoveDown;
        InputCommand MoveRight;
        InputCommand MoveLeft;

        InputCommand PlayAgain;
        InputCommand ReturnToMainMenu;


        ScoreUI scoreUI;
        GameOverMenu gameOverUI;



        //? ASSETS ===============================================================


        //? GAME STATES ==========================================================
        public Gamestate Setup_State;
        public Gamestate MainGamePlay_State;
        public Gamestate GameOver_State;
        //? GAME STATES ==========================================================

        public override void Initialize()
        {
            world = new World(4000, 4000);
            world.viewportCamera = new Camera(); // Initialize the camera
            world.viewportCamera.ViewportWidth = GraphicsManager.REFERENCE_WINDOW_WIDTH;
            world.viewportCamera.ViewportHeight = GraphicsManager.REFERENCE_WINDOW_HEIGHT;
            world.viewportCamera.Zoom = 1.2f;
            worldBorder = new LevelGround(world);

            foodSystem = new FoodSystem(world);
            snakeSystem = new SnakeSystem(world);

            westWall = new DeathWall(new Vector2(-2000f, 0), new Vector2(40f, 4000f), world);
            eastWall = new DeathWall(new Vector2(2000f, 0), new Vector2(40f, 4000f), world);
            northWall = new DeathWall(new Vector2(0, 2000f), new Vector2(4000f, 40f), world);
            southWall = new DeathWall(new Vector2(0, -2000f), new Vector2(4000f, 40f), world);


            //? Initialize game states
            Setup_State = new Gamestate(Context.GAME, InputMode.COMMAND_MODE, Setup_Update, Setup_Render);
            MainGamePlay_State = new Gamestate(Context.GAME, InputMode.COMMAND_MODE, MainGamePlay_Update, MainGamePlay_Render);
            GameOver_State = new Gamestate(Context.GAME_OVER, InputMode.COMMAND_MODE, GameOver_Update, GameOver_Render);

            scoreUI = new ScoreUI();
            gameOverUI = new GameOverMenu();






            //_Client.Inject(this, simulator); 
        }

        private void Setup_Update(GameTime gameTime)
        {



            TransitionState(MainGamePlay_State);

        }

        private void Setup_Render()
        {

        }

        private void MainGamePlay_Update(GameTime gameTime)
        {
            SimulatorManager.input.elapsedTime = gameTime.ElapsedGameTime;

            Output output = new Output();

            List<Output> outputs = SimulatorManager.GetOutput();

            for (int outputIndex = 0; outputIndex < outputs.Count; outputIndex++)
            {

                output = outputs[outputIndex];

                if (SimulatorManager.simulator.IsNetwork())
                {

                    for (int i = 0; i < output.connectedSignalDatas.Count; i++)
                    {
                        GameDataManager.clientID = output.connectedSignalDatas[i].clientID;

                        ClientJoinRequestData joinrequest = new ClientJoinRequestData(GameDataManager.clientID, GameDataManager.playerName);
                        SimulatorManager.input.clientJoinRequestDatas.Add(joinrequest);
                        SimulatorManager.simulator.SendInput(SimulatorManager.input);
                        initialized = true;
                    }
                }
                else
                {
                    GameDataManager.clientID = 0;

                    ClientJoinRequestData joinrequest = new ClientJoinRequestData(GameDataManager.clientID, GameDataManager.playerName);
                    SimulatorManager.input.clientJoinRequestDatas.Add(joinrequest);
                    SimulatorManager.simulator.SendInput(SimulatorManager.input);
                    initialized = true;
                }


                if (initialized)
                {

                    for (int i = 0; i < output.foodStateDatas.Count; i++)
                    {
                        foodSystem.InitializeFoodState(output.foodStateDatas[i].foodIDs, output.foodStateDatas[i].foodPositions_X, output.foodStateDatas[i].foodPositions_Y);
                    }

                    for (int i = 0; i < output.foodSpawnDatas.Count; i++)
                    {
                        foodSystem.AddFood(new Vector2(output.foodSpawnDatas[i].positionX, output.foodSpawnDatas[i].positionY), output.foodSpawnDatas[i].foodID);
                    }

                    for (int i = 0; i < output.foodDeleteDatas.Count; i++)
                    {
                        foodSystem.RemoveFood(output.foodDeleteDatas[i].foodID);
                    }

                    for (int i = 0; i < output.snakeSystemStatesDatas.Count; i++)
                    {
                        snakeSystem.InitializeSnakeSystemState(output.snakeSystemStatesDatas[i].clientIDs, output.snakeSystemStatesDatas[i].playerNames, output.snakeSystemStatesDatas[i].segmentPositions, output.snakeSystemStatesDatas[i].activeStates, output.snakeSystemStatesDatas[i].invincibleFlags);
                    }

                    for (int i = 0; i < output.addSnakeDatas.Count; i++)
                    {
                        snakeSystem.AddSnake(output.addSnakeDatas[i].clientID, output.addSnakeDatas[i].playerName, output.addSnakeDatas[i].segmentPositions);
                    }

                    for (int i = 0; i < output.removeSnakeDatas.Count; i++)
                    {
                        snakeSystem.RemoveSnake(output.removeSnakeDatas[i].clientID);
                    }

                    for (int i = 0; i < output.changeSnakeAngleDatas.Count; i++)
                    {
                        snakeSystem.ChangeSnakeDirection(output.changeSnakeAngleDatas[i].clientID, output.changeSnakeAngleDatas[i].radianAngle);
                    }

                    for (int i = 0; i < output.moveSnakeDatas.Count; i++)
                    {
                        snakeSystem.MoveSnakes(output.moveSnakeDatas[i].clientID, output.moveSnakeDatas[i].segmentPositions);
                    }

                    for (int i = 0; i < output.expandSnakeDatas.Count; i++)
                    {
                        snakeSystem.ExpandSnake(output.expandSnakeDatas[i].clientID, output.expandSnakeDatas[i].numberOfSegments);
                    }

                    for (int i = 0; i < output.reviveSnakeDatas.Count; i++)
                    {
                        snakeSystem.ReviveSnake(output.reviveSnakeDatas[i].clientID, output.reviveSnakeDatas[i].position);
                    }

                    for (int i = 0; i < output.killSnakeDatas.Count; i++)
                    {

                        if (output.killSnakeDatas[i].clientID == GameDataManager.clientID)
                        {
                            GameDataManager.playerScore = snakeSystem.snakes[output.killSnakeDatas[i].clientID].segments.Count;
                            GameDataManager.highScoresList.latestScore = GameDataManager.playerScore;
                            ScoreEntry newEntry = new ScoreEntry(GameDataManager.highScoresList.latestScore, "");

                            if (GameDataManager.highScoresList.AddScore(newEntry))
                            {
                                GameDataManager.scoreData_IO.SaveScoresList();
                            }

                            gameOverUI.playerScore.SetText(GameDataManager.playerScore.ToString());

                            TransitionState(GameOver_State);
                        }

                        snakeSystem.KillSnake(output.killSnakeDatas[i].clientID);
                    }

                    for (int i = 0; i < output.makeSnakeNotInvincibleDatas.Count; i++)
                    {
                        snakeSystem.snakes[output.makeSnakeNotInvincibleDatas[i].clientID].invincible = false;
                    }
                }
            }

            SimulatorManager.CleanInput();


            //? HANDLE INPUT
            InputManager.ProcessCommands(gameTime);

            float diagonalDirection = CalculateDiagonalDirection();
            if (diagonalDirection != 0) // Check if a diagonal direction is calculated
            {
                if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
                {
                    snakeSystem.snakes[GameDataManager.clientID].MakeChangeDirectionRequest(diagonalDirection, SimulatorManager.input);
                }
            }
            else
            {
                // Normal movement processing
                InputManager.ProcessCommands(gameTime);
            }


            ParticleSystem.UpdateEmittersParticles(gameTime);

            //? UPDATE CAMERA
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                if (snakeSystem.snakes[GameDataManager.clientID].head.position != null)
                {
                    world.viewportCamera.MoveCamera(snakeSystem.snakes[GameDataManager.clientID].head.position);
                }
            }

            //? UPDATE ANIMATED SPRITE STUFF
            foreach (var food in foodSystem.foodPool.Values)
            {
                food.foodAnimation.Update(gameTime);
            }

            //? UPDATE UI STATE

            //? YOUR SCORE
            if(snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                scoreUI.playerScore.SetText(snakeSystem.snakes[GameDataManager.clientID].segments.Count.ToString());
            }

            //? SLOT SCORES

            if (snakeSystem.snakes.Count > 0)
            {
                List<int> topFiveSnakeIDs = snakeSystem.GetTopFiveSnakesIDs();

                if (topFiveSnakeIDs.Count > 0)
                {
                    scoreUI.slot1Score.leftText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[0]].playerName);
                    scoreUI.slot1Score.rightText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[0]].segments.Count.ToString());
                }
                else
                {
                    scoreUI.slot1Score.leftText_UI.SetText("");
                    scoreUI.slot1Score.rightText_UI.SetText("");
                }

                if (topFiveSnakeIDs.Count > 1)
                {
                    scoreUI.slot2Score.leftText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[1]].playerName);
                    scoreUI.slot2Score.rightText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[1]].segments.Count.ToString());
                }
                else
                {
                    scoreUI.slot2Score.leftText_UI.SetText("");
                    scoreUI.slot2Score.rightText_UI.SetText("");
                }

                if (topFiveSnakeIDs.Count > 2)
                {
                    scoreUI.slot3Score.leftText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[2]].playerName);
                    scoreUI.slot3Score.rightText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[2]].segments.Count.ToString());
                }
                else
                {
                    scoreUI.slot3Score.leftText_UI.SetText("");
                    scoreUI.slot3Score.rightText_UI.SetText("");
                }

                if (topFiveSnakeIDs.Count > 3)
                {
                    scoreUI.slot4Score.leftText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[3]].playerName);
                    scoreUI.slot4Score.rightText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[3]].segments.Count.ToString());
                }
                else
                {
                    scoreUI.slot4Score.leftText_UI.SetText("");
                    scoreUI.slot4Score.rightText_UI.SetText("");
                }

                if (topFiveSnakeIDs.Count > 4)
                {
                    scoreUI.slot5Score.leftText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[4]].playerName);
                    scoreUI.slot5Score.rightText_UI.SetText(snakeSystem.snakes[topFiveSnakeIDs[4]].segments.Count.ToString());
                }
                else
                {
                    scoreUI.slot5Score.leftText_UI.SetText("");
                    scoreUI.slot5Score.rightText_UI.SetText("");
                }
            }
            SimulatorManager.simulator.SendInput(SimulatorManager.input);
        }

        private void MainGamePlay_Render()
        {
            // Draw world
            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw(GraphicsManager.backgroundTexture, new Vector2(0, 0), Color.White);
            GraphicsManager.spriteBatch.End();

            worldBorder.Draw(world.viewportCamera);

            //? DRAW FOOD
            foodSystem.DrawAllFood();

            eastWall.Draw(world.viewportCamera);
            westWall.Draw(world.viewportCamera);
            northWall.Draw(world.viewportCamera);
            southWall.Draw(world.viewportCamera);

            //? DRAW SNAKES
            snakeSystem.DrawSnakes();

            //? DRAW PARTICLES
            ParticleSystem.DrawEmitterParticles();

            //? DRAW UI
            GraphicsManager.spriteBatch.Begin();
            scoreUI.Draw();
            GraphicsManager.spriteBatch.End();
        }


        private void GameOver_Update(GameTime gameTime)
        {
            SimulatorManager.input.elapsedTime = gameTime.ElapsedGameTime;

            Output output = new Output();

            List<Output> outputs = SimulatorManager.GetOutput();

            for (int outputIndex = 0; outputIndex < outputs.Count; outputIndex++)
            {

                output = outputs[outputIndex];

                if (SimulatorManager.simulator.IsNetwork())
                {

                    for (int i = 0; i < output.connectedSignalDatas.Count; i++)
                    {
                        GameDataManager.clientID = output.connectedSignalDatas[i].clientID;

                        ClientJoinRequestData joinrequest = new ClientJoinRequestData(GameDataManager.clientID, GameDataManager.playerName);
                        SimulatorManager.input.clientJoinRequestDatas.Add(joinrequest);
                        SimulatorManager.simulator.SendInput(SimulatorManager.input);
                        initialized = true;
                    }
                }
                else
                {
                    GameDataManager.clientID = 0;

                    ClientJoinRequestData joinrequest = new ClientJoinRequestData(GameDataManager.clientID, GameDataManager.playerName);
                    SimulatorManager.input.clientJoinRequestDatas.Add(joinrequest);
                    SimulatorManager.simulator.SendInput(SimulatorManager.input);
                    initialized = true;
                }


                if (initialized)
                {
                    for (int i = 0; i < output.foodStateDatas.Count; i++)
                    {
                        foodSystem.InitializeFoodState(output.foodStateDatas[i].foodIDs, output.foodStateDatas[i].foodPositions_X, output.foodStateDatas[i].foodPositions_Y);
                    }

                    for (int i = 0; i < output.foodSpawnDatas.Count; i++)
                    {
                        foodSystem.AddFood(new Vector2(output.foodSpawnDatas[i].positionX, output.foodSpawnDatas[i].positionY), output.foodSpawnDatas[i].foodID);
                    }

                    for (int i = 0; i < output.foodDeleteDatas.Count; i++)
                    {
                        foodSystem.RemoveFood(output.foodDeleteDatas[i].foodID);
                    }

                    for (int i = 0; i < output.snakeSystemStatesDatas.Count; i++)
                    {
                        snakeSystem.InitializeSnakeSystemState(output.snakeSystemStatesDatas[i].clientIDs, output.snakeSystemStatesDatas[i].playerNames, output.snakeSystemStatesDatas[i].segmentPositions, output.snakeSystemStatesDatas[i].activeStates, output.snakeSystemStatesDatas[i].invincibleFlags);
                    }

                    for (int i = 0; i < output.addSnakeDatas.Count; i++)
                    {
                        snakeSystem.AddSnake(output.addSnakeDatas[i].clientID, output.addSnakeDatas[i].playerName, output.addSnakeDatas[i].segmentPositions);
                    }

                    for (int i = 0; i < output.removeSnakeDatas.Count; i++)
                    {
                        snakeSystem.RemoveSnake(output.removeSnakeDatas[i].clientID);
                    }

                    for (int i = 0; i < output.changeSnakeAngleDatas.Count; i++)
                    {
                        snakeSystem.ChangeSnakeDirection(output.changeSnakeAngleDatas[i].clientID, output.changeSnakeAngleDatas[i].radianAngle);
                    }

                    for (int i = 0; i < output.moveSnakeDatas.Count; i++)
                    {
                        snakeSystem.MoveSnakes(output.moveSnakeDatas[i].clientID, output.moveSnakeDatas[i].segmentPositions);
                    }

                    for (int i = 0; i < output.expandSnakeDatas.Count; i++)
                    {
                        snakeSystem.ExpandSnake(output.expandSnakeDatas[i].clientID, output.expandSnakeDatas[i].numberOfSegments);
                    }

                    for (int i = 0; i < output.reviveSnakeDatas.Count; i++)
                    {
                        snakeSystem.ReviveSnake(output.reviveSnakeDatas[i].clientID, output.reviveSnakeDatas[i].position);
                    }

                    for (int i = 0; i < output.killSnakeDatas.Count; i++)
                    {
                        snakeSystem.KillSnake(output.killSnakeDatas[i].clientID);
                    }

                    for (int i = 0; i < output.makeSnakeNotInvincibleDatas.Count; i++)
                    {
                        snakeSystem.snakes[output.makeSnakeNotInvincibleDatas[i].clientID].invincible = false;
                    }
                }
            }

            SimulatorManager.CleanInput();


            //? HANDLE INPUT
            InputManager.ProcessCommands(gameTime);

            ParticleSystem.UpdateEmittersParticles(gameTime);

            //? UPDATE CAMERA
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                if (snakeSystem.snakes[GameDataManager.clientID].head.position != null)
                {
                    world.viewportCamera.MoveCamera(snakeSystem.snakes[GameDataManager.clientID].head.position);
                }
            }

            //? UPDATE ANIMATED SPRITE STUFF
            foreach (var food in foodSystem.foodPool.Values)
            {
                food.foodAnimation.Update(gameTime);
            }

            SimulatorManager.simulator.SendInput(SimulatorManager.input);
        }

        private void GameOver_Render()
        {
            // Draw world
            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw(GraphicsManager.backgroundTexture, new Vector2(0, 0), Color.White);
            GraphicsManager.spriteBatch.End();
            worldBorder.Draw(world.viewportCamera);

            //? DRAW FOOD
            foodSystem.DrawAllFood();

            eastWall.Draw(world.viewportCamera);
            westWall.Draw(world.viewportCamera);
            northWall.Draw(world.viewportCamera);
            southWall.Draw(world.viewportCamera);

            //? DRAW SNAKES
            snakeSystem.DrawSnakes();


            //? DRAW PARTICLES
            ParticleSystem.DrawEmitterParticles();

            //? DRAW UI
            GraphicsManager.spriteBatch.Begin();
            gameOverUI.Draw();
            GraphicsManager.spriteBatch.End();
        }

        private void MoveUpAction(GameTime gameTime)
        {
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                snakeSystem.snakes[GameDataManager.clientID].MakeChangeDirectionRequest(MathHelper.PiOver2, SimulatorManager.input);
            }
        }

        private void MoveDownAction(GameTime gameTime)
        {
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                snakeSystem.snakes[GameDataManager.clientID].MakeChangeDirectionRequest(3 * MathHelper.PiOver2, SimulatorManager.input);
            }
        }

        private void MoveLeftAction(GameTime gameTime)
        {
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                snakeSystem.snakes[GameDataManager.clientID].MakeChangeDirectionRequest(MathHelper.Pi, SimulatorManager.input);
            }
        }

        private void MoveRightAction(GameTime gameTime)
        {
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                snakeSystem.snakes[GameDataManager.clientID].MakeChangeDirectionRequest(0, SimulatorManager.input);
            }
        }

        private void PlayAgainAction(GameTime gameTime)
        {
            if (snakeSystem.snakes.ContainsKey(GameDataManager.clientID))
            {
                ReviveSnakeRequestData reviveSnakeRequestData = new ReviveSnakeRequestData(GameDataManager.clientID);
                SimulatorManager.input.reviveSnakeRequestDatas.Add(reviveSnakeRequestData);
            }

            TransitionState(MainGamePlay_State);
        }

        private void ReturnToMenuAction(GameTime gameTime)
        {
            //? DISCONNECT THING!
            SimulatorManager.Disconnect();
     

            //? CLEAR LOCAL DATA
            snakeSystem.snakes.Clear();
            foodSystem.foodPool.Clear();
            GameDataManager.playerScore = 0;
            GameDataManager.clientID = -1;
            GameDataManager.playerName = "";

            SceneManager.TransitionScene(SceneLabel.MAIN_MENU);
        }

        public override void RegisterCommands()
        {

            MoveUp = new InputCommand(PressMode.PRESS_ONLY, MoveUpAction, null, "Move Up");
            MoveDown = new InputCommand(PressMode.PRESS_ONLY, MoveDownAction, null, "Move Down");
            MoveRight = new InputCommand(PressMode.PRESS_ONLY, MoveRightAction, null, "Move Right");
            MoveLeft = new InputCommand(PressMode.PRESS_ONLY, MoveLeftAction, null, "Move Left");

            PlayAgain = new InputCommand(PressMode.PRESS_ONLY, PlayAgainAction, null, "Play Again");
            ReturnToMainMenu = new InputCommand(PressMode.PRESS_ONLY, ReturnToMenuAction, null, "Return to Main Menu from Game");


            //? COMMANDS HERE
            InputManager.commandMap.RegisterCommand(MoveUp.commandName, MoveUp);
            InputManager.commandMap.RegisterCommand(MoveDown.commandName, MoveDown);
            InputManager.commandMap.RegisterCommand(MoveRight.commandName, MoveRight);
            InputManager.commandMap.RegisterCommand(MoveLeft.commandName, MoveLeft);

            InputManager.commandMap.RegisterCommand(PlayAgain.commandName, PlayAgain);
            InputManager.commandMap.RegisterCommand(ReturnToMainMenu.commandName, ReturnToMainMenu);
        }

        public override void RegisterDefaultKeys()
        {
            InputManager.inputMap.RegisterKey(Keys.Up, Context.GAME, MoveUp.commandName);
            InputManager.inputMap.RegisterKey(Keys.Down, Context.GAME, MoveDown.commandName);
            InputManager.inputMap.RegisterKey(Keys.Left, Context.GAME, MoveLeft.commandName);
            InputManager.inputMap.RegisterKey(Keys.Right, Context.GAME, MoveRight.commandName);

            InputManager.inputMap.RegisterKey(Keys.Enter, Context.GAME_OVER, PlayAgain.commandName);
            InputManager.inputMap.RegisterKey(Keys.Escape, Context.GAME_OVER, ReturnToMainMenu.commandName);
        }

        private float CalculateDiagonalDirection()
        {
            bool up = InputManager.IsKeyPressed(InputManager.inputMap.commandNameToKeyMap["Move Up"].Item2);
            bool down = InputManager.IsKeyPressed(InputManager.inputMap.commandNameToKeyMap["Move Down"].Item2);
            bool left = InputManager.IsKeyPressed(InputManager.inputMap.commandNameToKeyMap["Move Left"].Item2);
            bool right = InputManager.IsKeyPressed(InputManager.inputMap.commandNameToKeyMap["Move Right"].Item2);

            if (up && right) return MathHelper.PiOver4; // 45 degrees
            if (up && left) return 3 * MathHelper.PiOver4; // 135 degrees
            if (down && right) return -MathHelper.PiOver4; // 315 degrees
            if (down && left) return -3 * MathHelper.PiOver4; // 225 degrees

            return 0; // No diagonal movement
        }

        // Called when the scene is entered into.
        public override void OnEnterScene()
        {
            AudioManager.gameMusic.Play();
            TransitionState(Setup_State);
        }

        // Called when the scene is exited.
        public override void OnExitScene()
        {
            AudioManager.gameMusic.Stop();
        }
    }
}