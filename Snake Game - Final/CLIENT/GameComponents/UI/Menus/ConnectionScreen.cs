using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu for displaying high scores.
    public class ConnectionScreen : Menu
    {
        // Center X coordinate of screen.
        private int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);

        // Center X coordinate of screen.
        private int centerY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.5f);

        // The instructions for telling the player to enter their name if they achieve a high score.
        public UI_Text message { get; set; }

     

        // Constructor.
        public ConnectionScreen(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {

            // Player name input instructions UI element.
            message = new UI_Text(centerX, centerY, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Connecting . . .");

  
            menuDecor.Add(message);






        }

        // Draws the menu.
        public override void Draw()
        {
            base.Draw();

            message.Draw_UI();
        }

       
    }
}
