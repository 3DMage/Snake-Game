using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu for displaying high scores.
    public class PlayerNameInput : Menu
    {
        // Center X coordinate of screen.
        private int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);

        // The current UI_Text element to type player name on.
        public UI_Text playerNameInputTextInput { get; set; }

        // The instructions for telling the player to enter their name if they achieve a high score.
        public UI_Text playerNameInputInstructions { get; set; }

        // Message when invalidName is entered.
        public UI_Text invalidMessage { get; set; }

        // Constructor.
        public PlayerNameInput(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {
            // Menu coordinates.
            int leftX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.10f);
            centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);
            int bottomY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.90f);
            int header_positionY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.10f);

            // Header element.
            UI_Text headerLabel_UI = new UI_Text(centerX, header_positionY, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, "Join");

            // Add header element to menuDecor.
            menuDecor.Add(headerLabel_UI);

            // Y coordinate to the instructions element.
            int instructionsY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.35f);

            // Player name input instructions UI element.
            playerNameInputInstructions = new UI_Text(centerX, instructionsY, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Type in your name, then press Enter.");

            // Add the latest score UI element to menuDecor
            menuDecor.Add(playerNameInputInstructions);


            int playerNameInputY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.45f);

            playerNameInputTextInput = new UI_Text(centerX, playerNameInputY, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "");

            menuDecor.Add(playerNameInputTextInput);


            int invalidInputMessageY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.58f);

            invalidMessage = new UI_Text(centerX, invalidInputMessageY, UI_Origin.CENTER, GraphicsManager.calibriFont, new Color(255, 130, 130), "");

            menuDecor.Add(invalidMessage);

        }

        // Draws the menu.
        public override void Draw()
        {
            base.Draw();

            playerNameInputInstructions.Draw_UI();
        }

       
    }
}
