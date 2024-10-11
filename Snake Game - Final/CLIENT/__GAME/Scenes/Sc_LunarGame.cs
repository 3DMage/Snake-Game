using Microsoft.Xna.Framework;
using LunarLander.Input;
using Microsoft.Xna.Framework.Input;
using LunarLander.Assets.GameObjects;
using LunarLander.Assets.Menu.GameUI;
using LunarLander.Entities.GameObjects;
using LunarLander.Assets.ParticleSystems;
using System;
using LunarLander.DataManagement.GameData;
using Graphics;
using Input;
using Audio;
using LunarLander.DataManagement;
using LunarLander.SceneManagement.SceneComponents;
using LunarLander.Assets.UI.UI_Items;
using LunarLander.Physics.LunarLanderStuff;

namespace LunarLander.SceneManagement.Scenes
{
    // Scene containing main gameplay content.
    public class Sc_LunarGame : Scene
    {
        public WorldSystem world;

        // Gravity data.
        public Gravity gravity;

        // Terrain.
        public Terrain terrain;

        // Lander.
        public Lander lander;

        // Terrain collision handling.
        public TerrainCollisionHandling terrainCollisionHandling;

        // Game UI display.
        public GameUI gameUI;

        // Level message.
        public LevelMessage levelMessage;

        // Commands associated with scene.
        Command thrust_Command;
        Command rotateLeft_Command;
        Command rotateRight_Command;

        //? Gamestates
        Gamestate IntroMessage_State;
        Gamestate Countdown_State;
        Gamestate MainGamePlay_State;
        Gamestate GameOver_State;
        Gamestate Victory_State;

        // Position to place lander on when level begins.
        public Vector2 initialPosition { get; private set; } = new Vector2(640.0f, 120.0f);

        // Background element.
        public Rectangle backgroundRectangle { get; private set; }

        // Timer for displaying the level message.
        public TimeSpan messageTimer { get; private set; }

        // Timer for countdown display.
        public TimeSpan countdownTimerInterval;

        // Current timer for various timed events.
        public TimeSpan currentTimer;

        // Start value for countdown.
        public int countDownValueStart = 3;

        // Current countdown value.
        public int countDownValue;

        // Color for good status message.
        public Color goodColor = new Color(115, 255, 33);

        // Background color for frame for level messages.
        public Color transparentBackgroundColor = new Color(36, 36, 36, 215);

        // Origin offset to transparent background frame.
        public Vector2 transparentBackgroundOriginOffset;

        // The size of transparent background frame.
        public Vector2 transparentBackgroundSize = new Vector2(700, 100);

        // Position of the center of the screen.
        public Vector2 centerScreenPosition;








        // Initializes state of LunarGame scene.
        public override void Initialize()
        {
            world = new WorldSystem(GraphicsManager.REFERENCE_WINDOW_WIDTH, GraphicsManager.REFERENCE_WINDOW_HEIGHT);

            gravity = new Gravity();
            terrain = new Terrain(GraphicsManager.REFERENCE_WINDOW_DIMENSIONS);
            backgroundRectangle = new Rectangle(0, 0, GraphicsManager.REFERENCE_WINDOW_WIDTH, GraphicsManager.REFERENCE_WINDOW_HEIGHT);

            transparentBackgroundOriginOffset = new Vector2((float)GraphicsManager.rectColorTexture.Width / 2, (float)GraphicsManager.rectColorTexture.Height / 2);
            centerScreenPosition = new Vector2(GraphicsManager.REFERENCE_WINDOW_WIDTH / 2, GraphicsManager.REFERENCE_WINDOW_HEIGHT / 2);

            terrainCollisionHandling = new TerrainCollisionHandling();
            lander = new Lander(GraphicsManager.landerTexture, gravity, initialPosition, world);
            gameUI = new GameUI(lander);
            levelMessage = new LevelMessage();

            messageTimer = new TimeSpan(0, 0, 3);
            countdownTimerInterval = new TimeSpan(0, 0, 1);
            currentTimer = new TimeSpan(0, 0, 0);



            //? Initialize game states
            IntroMessage_State = new Gamestate(Context.GAME, InputMode.DISABLED_MODE, IntroMessage_Update, StartState_Render);
            Countdown_State = new Gamestate(Context.GAME, InputMode.DISABLED_MODE, Countdown_Update, StartState_Render);
            MainGamePlay_State = new Gamestate(Context.GAME, InputMode.COMMAND_MODE, MainGamePlay_Update, MainGamePlay_Render);
            Victory_State = new Gamestate(Context.GAME, InputMode.DISABLED_MODE, Victory_Update, Victory_Render);
            GameOver_State = new Gamestate(Context.GAME, InputMode.DISABLED_MODE, GameOver_Update, GameOver_Render);


        }

        // Registers CommandPackets into the Input Manager.
        public override void RegisterCommands()
        {
            thrust_Command = new Command(CommandMode.PRESS_REGULAR, Thrust, Unthrust, "Thrust");
            rotateLeft_Command = new Command(CommandMode.PRESS_REGULAR, RotateLanderLeft, null, "Rotate Left");
            rotateRight_Command = new Command(CommandMode.PRESS_REGULAR, RotateLanderRight, null, "Rotate Right");

            InputManager.inputMap.RegisterCommandPacket(thrust_Command.commandName, thrust_Command);
            InputManager.inputMap.RegisterCommandPacket(rotateLeft_Command.commandName, rotateLeft_Command);
            InputManager.inputMap.RegisterCommandPacket(rotateRight_Command.commandName, rotateRight_Command);
        }

        // Registers default input for Input Manager if no InputMap was loaded via persistent storage.
        public override void RegisterDefaultKeys()
        {
            InputManager.inputMap.RegisterKey(Keys.Up, Context.GAME, thrust_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.Left, Context.GAME, rotateLeft_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.Right, Context.GAME, rotateRight_Command.commandName);
        }





        //? COMMAND METHODS ===================================================================================
        // Rotates the lander left.
        public void RotateLanderLeft(GameTime gameTime)
        {
            lander.UpdateAngle(false, gameTime);
        }

        // Rotates the lander right.
        public void RotateLanderRight(GameTime gameTime)
        {
            lander.UpdateAngle(true, gameTime);
        }

        // Applies thrust to the lander.
        public void Thrust(GameTime gameTime)
        {
            lander.ApplyThrust(gameTime);
        }

        // Deactivates thrust.
        public void Unthrust(GameTime gameTime)
        {
            lander.Unthrust();
        }
        //? COMMAND METHODS ===================================================================================





        //? STATE METHODS =====================================================================================




        // State for when the main game scene is started.
        public void IntroMessage_Update(GameTime gameTime)
        {
            // Display countdown.
            currentTimer += gameTime.ElapsedGameTime;
            if (currentTimer >= countdownTimerInterval)
            {
                AudioManager.src_messageBeep.Play();
                countDownValue += -1;
                levelMessage.UpdateMessageText(countDownValue.ToString());
                currentTimer = TimeSpan.Zero;
            }

            // Transition state to main gameplay state.
            if (countDownValue <= 0)
            {
                TransitionState(MainGamePlay_State);
            }
        }

        // State for when the game is transitioning levels.
        public void Countdown_Update(GameTime gameTime)
        {
            // Generate new level and data if safe zone count is bigger than zero.
            if (GameDataManager.sessionData.safeZoneCount > 0)
            {
                // Display the timer.
                currentTimer += gameTime.ElapsedGameTime;
                if (currentTimer >= countdownTimerInterval)
                {
                    AudioManager.src_messageBeep.Play();
                    countDownValue += -1;
                    levelMessage.UpdateMessageText(countDownValue.ToString());
                    currentTimer = TimeSpan.Zero;
                }

                // Reset game data and generate a new level.
                if (countDownValue <= 0)
                {
                    // Clear terrain data.
                    terrain.ClearData();

                    // Clear remaining particles.
                    ParticleSystem.ClearParticles();

                    // Make safe zone areas now small.
                    terrain.currentSafeZoneMin = terrain.smallSafeZoneLength_MIN;
                    terrain.currentSafeZoneMax = terrain.smallSafeZoneLength_MAX;

                    // Generate the terrain.
                    terrain.GenerateTerrain(GameDataManager.sessionData.safeZoneCount);
                    terrainCollisionHandling.InjectTerrainData(terrain.lineList);

                    // Clear level message.
                    levelMessage.UpdateMessageText("");

                    // Reset the lander
                    lander.ResetLander(initialPosition);

                    // Transition to main gameplay state.
                    TransitionState(MainGamePlay_State);
                }
            }
            else
            {
                // Clear terrain data.
                terrain.ClearData();

                // Clear remaining particles.
                ParticleSystem.ClearParticles();

                // Reset the lander
                lander.ResetLander(initialPosition);

                // Save score data.
                GameDataManager.highScoresList.latestScore = GameDataManager.sessionData.score;
                ScoreEntry newEntry = new ScoreEntry(GameDataManager.highScoresList.latestScore, "");

                // Get reference to menu scene.
                Sc_Menu mainMenu = (Sc_Menu)SceneManager.scenes[SceneLabel.MAIN_MENU];

                // Update score data if high score was achieved.
                if (GameDataManager.highScoresList.AddScore(newEntry))
                {
                    GameDataManager.scoreData_IO.SaveScoresList();
                    InputManager.workingString = "";
                    InputManager.SetCharacterLimit(15);
                    mainMenu.scoresMenu.ClearSelections();
                    mainMenu.scoresMenu.currentScoreEnterIndex = GameDataManager.highScoresList.latestHighScoreInputIndex + mainMenu.scoresMenu.startingScoreItemIndex;
                    UI_TextPair scoreEntry = (UI_TextPair)mainMenu.scoresMenu.menuDecor[mainMenu.scoresMenu.currentScoreEnterIndex];
                    mainMenu.scoresMenu.currentScoreTextEnter = scoreEntry.leftText_UI;
                    mainMenu.scoresMenu.drawInstructions = true;

                    // Change input mode to type mode for player name input.
                    mainMenu.TransitionState(mainMenu.enterNewHighscore_State);
                }
                else
                {
                    // Change input mode to command mode.
                    mainMenu.TransitionState(mainMenu.mainmenu_State);
                }

                // Refresh latest score etnry.
                mainMenu.scoresMenu.RefreshList();

                // Transition to score menu.
                SceneManager.TransitionScene(SceneLabel.MAIN_MENU);
                mainMenu.currentMenu = mainMenu.scoresMenu;

                // Clear gameUI display for next round.
                gameUI.UpdateDisplayData();
            }
        }

        // Render the start state.
        public void StartState_Render()
        {
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.backgroundTexture,
              backgroundRectangle,
              Color.White
            );
            GraphicsManager.spriteBatch.End();

            terrain.DrawFill();

            GraphicsManager.spriteBatch.Begin();
            terrain.DrawOutline();
            GraphicsManager.spriteBatch.End();

            ParticleSystem.DrawEmitterParticles();

            GraphicsManager.spriteBatch.Begin();
            lander.Draw();
            gameUI.Draw();
            GraphicsManager.spriteBatch.Draw(GraphicsManager.rectColorTexture, centerScreenPosition, null, transparentBackgroundColor, 0, transparentBackgroundOriginOffset, transparentBackgroundSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            levelMessage.Draw();
            GraphicsManager.spriteBatch.End();
        }



        // Main gameplay state.
        public void MainGamePlay_Update(GameTime gameTime)
        {
            // Process currently pressed commands.
            InputManager.ProcessCommands(gameTime);

            // Update lander.
            lander.ApplyGravity(gameTime);
            lander.UpdatePosition();

            // Update GameUI display.
            gameUI.UpdateDisplayData();

            // Update any active emitters.
            ParticleSystem.UpdateEmitters(gameTime);

            world.gridCollisionHandling.BroadPhase();
            world.gridCollisionHandling.NarrowPhase();

            // Handle collision.
            CollisionState landerCollided = terrainCollisionHandling.TestTerrainCollision(lander.collider);
            if (landerCollided != CollisionState.NO_COLLIDE)
            {
                // Grab lander data needed for landing.
                float landerSpeed = lander.CurrentSpeed();
                float degreeAngle = lander.DegreeRotation();

                // Check if lander is ready to land.
                if (landerSpeed >= 0 && landerSpeed < 2 && (degreeAngle >= 355 && degreeAngle <= 360 || degreeAngle >= 0 && degreeAngle <= 5) && landerCollided == CollisionState.SAFE_COLLIDE)
                {
                    // Successful landing.

                    // Play and stop corresponding audio.
                    AudioManager.rocketRumble.Stop();
                    AudioManager.alert.Stop();
                    AudioManager.src_success.Play();

                    // Update UI displays.
                    levelMessage.UpdateColor(goodColor);
                    levelMessage.UpdateMessageText("Success! Fuel added to score");

                    // Update score info.
                    GameDataManager.sessionData.score += (int)lander.fuel;

                    // Reset timer.
                    currentTimer = TimeSpan.Zero;

                    // Transition states to victory state.
                    TransitionState(Victory_State);
                }
                else
                {
                    // Landing failed.

                    // Play and stop corresponding audio.
                    AudioManager.gameMusic.Stop();
                    AudioManager.rocketRumble.Stop();
                    AudioManager.alert.Stop();
                    AudioManager.src_explosion.Play();

                    // Explode the ship.
                    lander.ExplodeEffect();

                    // Display level message.
                    levelMessage.UpdateColor(Color.Red);
                    levelMessage.UpdateMessageText("Game Over");

                    // Reset timer.
                    currentTimer = TimeSpan.Zero;

                    // Transition to game over state.
                    TransitionState(GameOver_State);
                }
            }
        }

        // Render main gameplay state.
        public void MainGamePlay_Render()
        {
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.backgroundTexture,
              backgroundRectangle,
              Color.White
            );

            GraphicsManager.spriteBatch.End();

            terrain.DrawFill();

            GraphicsManager.spriteBatch.Begin();
            terrain.DrawOutline();
            GraphicsManager.spriteBatch.End();

            ParticleSystem.DrawEmitterParticles();

            GraphicsManager.spriteBatch.Begin();
            lander.Draw();
            gameUI.Draw();
            GraphicsManager.spriteBatch.End();
        }



        // Victory state.
        public void Victory_Update(GameTime gameTime)
        {
            // Display victory message.
            currentTimer += gameTime.ElapsedGameTime;
            if (currentTimer >= messageTimer)
            {
                // Reset timer.
                currentTimer = TimeSpan.Zero;

                // Setup countdown.
                countDownValue = countDownValueStart;
                levelMessage.UpdateColor(Color.White);
                AudioManager.src_messageBeep.Play();
                levelMessage.UpdateMessageText(countDownValue.ToString());

                // Update safezone count for next level.
                GameDataManager.sessionData.safeZoneCount += -1;

                // Transition to level transition state.
                TransitionState(Countdown_State);
            }

            // Update any active particles.
            ParticleSystem.UpdateEmitters(gameTime);
        }

        // Render the victory state.
        public void Victory_Render()
        {
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.backgroundTexture,
              backgroundRectangle,
              Color.White
            );
            GraphicsManager.spriteBatch.End();

            terrain.DrawFill();

            GraphicsManager.spriteBatch.Begin();
            terrain.DrawOutline();
            GraphicsManager.spriteBatch.End();

            ParticleSystem.DrawEmitterParticles();

            GraphicsManager.spriteBatch.Begin();
            lander.Draw();
            gameUI.Draw();
            GraphicsManager.spriteBatch.Draw(GraphicsManager.rectColorTexture, centerScreenPosition, null, transparentBackgroundColor, 0, transparentBackgroundOriginOffset, transparentBackgroundSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            levelMessage.Draw();

            GraphicsManager.spriteBatch.End();
        }



        // Game over state.
        public void GameOver_Update(GameTime gameTime)
        {
            // Display game over message.
            currentTimer += gameTime.ElapsedGameTime;
            if (currentTimer >= messageTimer)
            {
                // Clear terrain data.
                terrain.ClearData();

                // Clear remaining particles.
                ParticleSystem.ClearParticles();

                // Reset the lander
                lander.ResetLander(initialPosition);

                // Update score data.
                GameDataManager.highScoresList.latestScore = GameDataManager.sessionData.score;
                ScoreEntry newEntry = new ScoreEntry(GameDataManager.highScoresList.latestScore, "");

                // Get reference to menu scene.
                Sc_Menu mainMenu = (Sc_Menu)SceneManager.scenes[SceneLabel.MAIN_MENU];

                // If high score achieved, do more updates to score data.
                if (GameDataManager.highScoresList.AddScore(newEntry))
                {
                    GameDataManager.scoreData_IO.SaveScoresList();
                    InputManager.workingString = "";
                    InputManager.SetCharacterLimit(15);
                    mainMenu.scoresMenu.ClearSelections();
                    mainMenu.scoresMenu.currentScoreEnterIndex = GameDataManager.highScoresList.latestHighScoreInputIndex + mainMenu.scoresMenu.startingScoreItemIndex;
                    UI_TextPair scoreEntry = (UI_TextPair)mainMenu.scoresMenu.menuDecor[mainMenu.scoresMenu.currentScoreEnterIndex];
                    mainMenu.scoresMenu.currentScoreTextEnter = scoreEntry.leftText_UI;
                    mainMenu.scoresMenu.drawInstructions = true;

                    // Set input mode to type mode for player name input.
                    mainMenu.TransitionState(mainMenu.enterNewHighscore_State);
                }
                else
                {
                    // Set input mode to command mode.
                    mainMenu.TransitionState(mainMenu.mainmenu_State);
                }

                // Refresh latest score entry.
                mainMenu.scoresMenu.RefreshList();

                // Transition to scores menu.
                SceneManager.TransitionScene(SceneLabel.MAIN_MENU);
                mainMenu.currentMenu = mainMenu.scoresMenu;

                // Reset GameUI for next game session.
                gameUI.UpdateDisplayData();
            }

            // Update any active particles.
            ParticleSystem.UpdateEmitters(gameTime);
        }

        // Render the game over state.
        public void GameOver_Render()
        {
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.backgroundTexture,
              backgroundRectangle,
              Color.White
            );
            GraphicsManager.spriteBatch.End();

            terrain.DrawFill();

            GraphicsManager.spriteBatch.Begin();
            terrain.DrawOutline();
            GraphicsManager.spriteBatch.End();

            ParticleSystem.DrawEmitterParticles();

            GraphicsManager.spriteBatch.Begin();
            gameUI.Draw();
            GraphicsManager.spriteBatch.Draw(GraphicsManager.rectColorTexture, centerScreenPosition, null, transparentBackgroundColor, 0, transparentBackgroundOriginOffset, transparentBackgroundSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            levelMessage.Draw();

            GraphicsManager.spriteBatch.End();
        }

        //? STATE METHODS =====================================================================================












        // Called when the scene is entered into.
        public override void OnEnterScene()
        {
            // Update background music.
            AudioManager.gameMusic.Play();

            // Reset session data.
            GameDataManager.sessionData.ResetData();

            // Reset the timer.
            currentTimer = TimeSpan.Zero;

            // Make safe zone areas big for first level.
            terrain.currentSafeZoneMin = terrain.bigSafeZoneLength_MIN;
            terrain.currentSafeZoneMax = terrain.bigSafeZoneLength_MAX;

            // Generate the terrain.
            terrain.GenerateTerrain(GameDataManager.sessionData.safeZoneCount);

            // Setup collision.
            terrainCollisionHandling.InjectTerrainData(terrain.lineList);

            // Display level message.
            AudioManager.src_messageBeep.Play();
            levelMessage.UpdateColor(Color.White);
            countDownValue = countDownValueStart;
            levelMessage.UpdateMessageText(countDownValue.ToString());

            // Transition input mode to disabled mode temporarily while messages are being displayed.
            TransitionState(IntroMessage_State);
        }

        // Called when the scene is exited.
        public override void OnExitScene()
        {
            // Disable any remaining sounds.
            AudioManager.rocketRumble.Stop();
            AudioManager.alert.Stop();
            AudioManager.gameMusic.Stop();
        }
    }
}