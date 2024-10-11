using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu for displaying high scores.
    public class TutorialScreen : Menu
    {
        // Center X coordinate of screen.
        private int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);

        // Center X coordinate of screen.
        private int centerY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.5f);

        UI_Picture tutorialScreen;
        public UI_Text mappedKeysText;


        // Constructor.
        public TutorialScreen(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {
            tutorialScreen = new UI_Picture(centerX, centerY, GraphicsManager.REFERENCE_WINDOW_WIDTH, GraphicsManager.REFERENCE_WINDOW_HEIGHT, UI_Origin.CENTER, GraphicsManager.tutorialScreenTexture, Color.White);

            menuDecor.Add(tutorialScreen);

            mappedKeysText = new UI_Text(centerX - 267, centerY - 150, UI_Origin.CENTER_LEFT, GraphicsManager.calibriFont, Color.White, "");
        }

        // Draws the menu.
        public override void Draw()
        {
            base.Draw();

            tutorialScreen.Draw_UI();

            mappedKeysText.Draw_UI();
        }  
    }
}
