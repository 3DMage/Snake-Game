using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI;
using SnakeGame.GameComponents.UI.Menus;
using Shared.Datastructures;
using System.Threading;
using Client.SimulatorStuff;

namespace GameComponents
{
    // Scene containing main menu content.
    public class Sc_Menu : Scene
    {
        // Current menu being drawn and operated on.
        public Menu currentMenu;

        // Menus
        public ScoresMenu scoresMenu { get; private set; }
        public MainMenu mainMenu { get; private set; }
        public SettingsMenu settingsMenu { get; private set; }
        public CreditsMenu creditsMenu { get; private set; }
        public PlayerNameInput playerNameInput { get; private set; }
        public TutorialScreen tutorialScreen { get; private set; }
        public ConnectionScreen connectionScreen { get; private set; }


        // Background element.
        private Rectangle backgroundRectangle;

        // Current input config being used.
        public InputConfigEntry currentInputConfigEntry { get; set; }

        // Command packets associated with scene.
        InputCommand moveSelectionUp_Command;
        InputCommand moveSelectionDown_Command;
        InputCommand confirmSelection_Command;
        InputCommand returnToMainMenu_Command;
        InputCommand gotoConnection_Command;


        //? Gamestates
        public Gamestate mainmenu_State { get; private set; }
        public Gamestate inputconfig_State { get; private set; }
        public Gamestate enterNewHighscore_State { get; private set; }
        public Gamestate playerNameInput_State { get; private set; }
        public Gamestate tutorialScreen_State { get; private set; }
        public Gamestate connectionScreen_State { get; private set; }



        // Initializes state of Menu scene.
        public override void Initialize()
        {
            mainMenu = new MainMenu(this);
            scoresMenu = new ScoresMenu(this);
            settingsMenu = new SettingsMenu(this);
            creditsMenu = new CreditsMenu(this);
            playerNameInput = new PlayerNameInput(this);
            connectionScreen = new ConnectionScreen(this);
            tutorialScreen = new TutorialScreen(this);

            //? Initialize gamestates
            mainmenu_State = new Gamestate(Context.MENU, InputMode.COMMAND_MODE, MainMenu_Update, MainMenu_Render);
            inputconfig_State = new Gamestate(Context.INPUT_CONFIG, InputMode.INPUT_CONFIG_MODE, InputChange_Update, InputChange_Render);
            enterNewHighscore_State = new Gamestate(Context.MENU, InputMode.TYPE_MODE, HighScoreEnter_Update, HighScoreEnter_Render);
            playerNameInput_State = new Gamestate(Context.MENU, InputMode.TYPE_MODE, PlayerNameInput_Update, PlayerNameInput_Render);
            connectionScreen_State = new Gamestate(Context.GAME, InputMode.DISABLED_MODE, ConnectionScreen_Update, ConnectionScreen_Render);
            tutorialScreen_State = new Gamestate(Context.TUTORIAL_MENU, InputMode.COMMAND_MODE, TutorialScreen_Update, TutorialScreen_Render);

            mainMenu.ConstructMenu();
            scoresMenu.ConstructMenu();
            settingsMenu.ConstructMenu();
            creditsMenu.ConstructMenu();
            playerNameInput.ConstructMenu();
            connectionScreen.ConstructMenu();
            tutorialScreen.ConstructMenu();

            currentMenu = mainMenu;

            backgroundRectangle = new Rectangle(0, 0, GraphicsManager.REFERENCE_WINDOW_WIDTH, GraphicsManager.REFERENCE_WINDOW_HEIGHT);

      

            TransitionState(mainmenu_State);
        }

        // Registers Commands into the Input Manager.
        public override void RegisterCommands()
        {
            moveSelectionUp_Command = new InputCommand(PressMode.PRESS_ONLY, MoveSelectionUp, null, "Move Selection Up");
            moveSelectionDown_Command = new InputCommand(PressMode.PRESS_ONLY, MoveSelectionDown, null, "Move Selection Down");
            confirmSelection_Command = new InputCommand(PressMode.PRESS_ONLY, ConfirmSelection, null, "Confirm Selection");
            returnToMainMenu_Command = new InputCommand(PressMode.PRESS_ONLY, ReturnToMainMenu, null, "Return to Main Menu");

            gotoConnection_Command = new InputCommand(PressMode.PRESS_ONLY, GotoConnectionScreen, null, "GotoConnection");


            InputManager.commandMap.RegisterCommand(moveSelectionUp_Command.commandName, moveSelectionUp_Command);
            InputManager.commandMap.RegisterCommand(moveSelectionDown_Command.commandName, moveSelectionDown_Command);
            InputManager.commandMap.RegisterCommand(confirmSelection_Command.commandName, confirmSelection_Command);
            InputManager.commandMap.RegisterCommand(returnToMainMenu_Command.commandName, returnToMainMenu_Command);

            InputManager.commandMap.RegisterCommand(gotoConnection_Command.commandName, gotoConnection_Command);
        }

        // Registers default input for Input Manager if no InputMap was loaded via persistent storage.
        public override void RegisterDefaultKeys()
        {
            InputManager.inputMap.RegisterKey(Keys.Up, Context.MENU, moveSelectionUp_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.W, Context.MENU, moveSelectionUp_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.Down, Context.MENU, moveSelectionDown_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.S, Context.MENU, moveSelectionDown_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.Enter, Context.MENU, confirmSelection_Command.commandName);
            InputManager.inputMap.RegisterKey(Keys.Escape, Context.MENU, returnToMainMenu_Command.commandName);

            InputManager.inputMap.RegisterKey(Keys.Enter, Context.TUTORIAL_MENU, gotoConnection_Command.commandName);
        }

        // Moves currently selected menu item upwards.
        public void MoveSelectionUp(GameTime gameTime)
        {
            AudioManager.src_moveSelectBeep.Play();
            currentMenu.menuOptions[currentMenu.selectionIndex].UnmarkSelected();
            currentMenu.selectionIndex = MathKit.mod(currentMenu.selectionIndex - 1, currentMenu.menuOptions.Count);
            currentMenu.menuOptions[currentMenu.selectionIndex].MarkSelected();
        }

        // Moves currently selected menu item downwards.
        public void MoveSelectionDown(GameTime gameTime)
        {
            AudioManager.src_moveSelectBeep.Play();
            currentMenu.menuOptions[currentMenu.selectionIndex].UnmarkSelected();
            currentMenu.selectionIndex = MathKit.mod(currentMenu.selectionIndex + 1, currentMenu.menuOptions.Count);
            currentMenu.menuOptions[currentMenu.selectionIndex].MarkSelected();
        }

        // Confirms selection and activates button action.
        public void ConfirmSelection(GameTime gameTime)
        {
            AudioManager.src_selectBeep.Play();
            currentMenu.menuOptions[currentMenu.selectionIndex].ButtonAction();
        }

    

        // Goes to main menu from current menu.
        public void ReturnToMainMenu(GameTime gameTime)
        {
            AudioManager.src_selectBeep.Play();
            currentMenu = mainMenu;
        }

        // Called when the scene is entered into.
        public override void OnEnterScene()
        {
            TransitionState(mainmenu_State);
            currentMenu = mainMenu;
            AudioManager.menuMusic.Play();
        }

        // Called when the scene is exited.
        public override void OnExitScene()
        {
            AudioManager.menuMusic.Stop();
        }


        // Goes to main menu from current menu.
        public void GotoConnectionScreen(GameTime gameTime)
        {
            AudioManager.src_selectBeep.Play();
            currentMenu = connectionScreen;
            TransitionState(connectionScreen_State);
        }



        //? STATE METHODS ===================================================================================
        // Normal menu state.
        public void MainMenu_Update(GameTime gameTime)
        {
            InputManager.ProcessCommands(gameTime);
        }

        // Render the menu state.
        public void MainMenu_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }

        // High score state.
        public void HighScoreEnter_Update(GameTime gameTime)
        {
            InputManager.GetTypedCharacters();
            GameDataManager.highScoresList.scores[GameDataManager.highScoresList.latestHighScoreInputIndex].name = InputManager.workingString;
            scoresMenu.RefreshList();
            scoresMenu.currentScoreTextEnter.UpdateBlinker(gameTime);

            if (InputManager.doneTyping)
            {
                AudioManager.src_selectBeep.Play();
                GameDataManager.scoreData_IO.SaveScoresList();
                scoresMenu.selectionIndex = 0;
                scoresMenu.menuOptions[scoresMenu.selectionIndex].MarkSelected();
                scoresMenu.drawInstructions = false;
                scoresMenu.currentScoreTextEnter.ClearBlinker();
                InputManager.doneTyping = false;
                TransitionState(mainmenu_State);
            }
        }

        // Render the high score state.
        public void HighScoreEnter_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }

        // High score state.
        public void PlayerNameInput_Update(GameTime gameTime)
        {
            InputManager.GetTypedCharacters();
  
            playerNameInput.playerNameInputTextInput.SetText(InputManager.workingString);
            playerNameInput.playerNameInputTextInput.UpdateBlinker(gameTime);

            if (InputManager.doneTyping && InputManager.workingString != "")
            {
                AudioManager.src_selectBeep.Play();

                GameDataManager.playerName = InputManager.workingString;

                playerNameInput.invalidMessage.SetText("");




                InputManager.doneTyping = false;
                InputManager.workingString = "";


                Sc_Menu menuScene = (Sc_Menu)SceneManager.scenes[SceneLabel.MAIN_MENU];
                menuScene.currentMenu = menuScene.tutorialScreen;
                tutorialScreen.mappedKeysText.SetText("Mapped Keys: " + InputManager.inputMap.commandNameToKeyMap["Move Up"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Down"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Left"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Right"].Item2.ToString());
                menuScene.TransitionState(menuScene.tutorialScreen_State);

            }
            else if (InputManager.doneTyping && InputManager.workingString == "")
            {
                InputManager.doneTyping = false;

                // Display a message.
                playerNameInput.invalidMessage.SetText("Empty names are not allowed.");
            }
        }

        public void PlayerNameInput_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }



        public void ConnectionScreen_Update(GameTime gameTime)
        {
            Thread.Sleep(1500);

            bool connected = SimulatorManager.Connect().Result;

            if (connected)
            {
                SceneManager.TransitionScene(SceneLabel.GAME);
            }
            else
            {
                SimulatorManager.Disconnect();
                connectionScreen.message.SetText("Failed to connect.  Ensure server is on and try again.");
                currentMenu = mainMenu;
                TransitionState(mainmenu_State);
                connectionScreen.message.SetText("Connecting . . .");

            }
        }



        public void ConnectionScreen_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }



        // Input change state.
        public void InputChange_Update(GameTime gameTime)
        {
            InputConfigState remapped = InputManager.ConfigKey(currentInputConfigEntry.inputContext, currentInputConfigEntry.key);
            if (remapped == InputConfigState.VALID)
            {
                AudioManager.src_selectBeep.Play();
                Keys newKey = InputManager.latestPressedKey;
                //? REFINEMENT NOTE: The currentInputConfig entry should be tracked by settings mneu, not Sc_Menu!
                currentInputConfigEntry.UpdateKey(newKey);
                settingsMenu.ClearMessages();
                InputManager.inputMap_IO.SaveInputMap();
                TransitionState(mainmenu_State);
            }
            else if (remapped == InputConfigState.ITSELF || remapped == InputConfigState.CANCELED)
            {
                AudioManager.src_moveSelectBeep.Play();
                currentInputConfigEntry.UpdateKey(currentInputConfigEntry.key);
                settingsMenu.ClearMessages();
                TransitionState(mainmenu_State);
            }
            else if (remapped == InputConfigState.ALREADY_MAPPED)
            {
                AudioManager.src_moveSelectBeep.Play();
                settingsMenu.UpdateStatusMessage("Key already mapped.  Use a different key.");
            }
        }

        // Render the input change state.
        public void InputChange_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }

        // Normal menu state.
        public void TutorialScreen_Update(GameTime gameTime)
        {
            InputManager.ProcessCommands(gameTime);
        }

        // Render the menu state.
        public void TutorialScreen_Render()
        {
            // Clear frame.
            GraphicsManager.graphicsDevice.Clear(Color.Black);

            // Begin drawing.
            GraphicsManager.spriteBatch.Begin();

            GraphicsManager.spriteBatch.Draw
            (
              GraphicsManager.menuBackgroundTexture,
              backgroundRectangle,
              Color.White
            );

            currentMenu.Draw();

            // If the item is selected, draw that, else draw non-selected.
            GraphicsManager.spriteBatch.End();
        }
        //? STATE METHODS ===================================================================================

    }
}