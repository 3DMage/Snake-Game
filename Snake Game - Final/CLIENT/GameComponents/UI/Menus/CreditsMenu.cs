using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu displaying credits to the game.
    public class CreditsMenu : Menu
    {
        // Constructor.
        public CreditsMenu(Sc_Menu menuScene) : base(menuScene) { }

        // Center X coordinate.
        private int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);

        // Constructs the menu.
        public override void ConstructMenu()
        {
            // Menu coordinates.
            int leftX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.10f);
            centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);
            int bottomY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.9f);
            int header_positionY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.10f);

            // Header element.
            UI_Text headerLabel_UI = new UI_Text(centerX, header_positionY, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, "Credits");

            // Y positions of all credit entries.
            int Y_position1 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.20);
            int Y_position2 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.24);
            int Y_position3_1 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.28);
            int Y_position4 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.36);
            int Y_position5 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.40);

            // Credit entries.
            UI_TextPair creditsEntry1 = new UI_TextPair(centerX, Y_position1, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, 40, "Programming:", "Benjamin Ricks");
            UI_TextPair creditsEntry2 = new UI_TextPair(centerX, Y_position2, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, 40, "Graphics:", "Benjamin Ricks");
            UI_TextPair creditsEntry3_2 = new UI_TextPair(centerX, Y_position3_1, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, 40, "Music:", "Kevin Macleod");
            UI_Text creditsEntry5 = new UI_Text(centerX, Y_position4, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "All other assets used are in the public domain and");
            UI_Text creditsEntry6 = new UI_Text(centerX, Y_position5, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "are from OpenGameArt.org and FreeSound.org.");

            // Add non-interactive elements to menuDecor.
            menuDecor.Add(headerLabel_UI);
            menuDecor.Add(creditsEntry1);
            menuDecor.Add(creditsEntry2);
            menuDecor.Add(creditsEntry3_2);
            menuDecor.Add(creditsEntry5);
            menuDecor.Add(creditsEntry6);

            // Back button element.
            UI_Text backButton_UI = new UI_Text(leftX, bottomY, UI_Origin.CENTER_LEFT, GraphicsManager.calibriFont, Color.White, "Back");

            // Add interactive elements to menu options.
            menuOptions.Add(new TransitionButton(backButton_UI, menuScene, menuScene.mainMenu, menuScene.mainmenu_State));

            // Mark currently selected element.
            menuOptions[selectionIndex].MarkSelected();
        }
    }
}