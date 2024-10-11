using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace Client.GameComponents.UI.GameUI
{
    public class GameOverMenu
    {
        // The UI element holding the message text.
        UI_Picture gameOverMenu;
        public UI_Text playerScore;


      

        // Constructor.
        public GameOverMenu()
        {
            gameOverMenu = new UI_Picture(GraphicsManager.REFERENCE_WINDOW_WIDTH/2, GraphicsManager.REFERENCE_WINDOW_HEIGHT / 2, 646, 567, UI_Origin.CENTER, GraphicsManager.gameOverMenu, Color.White);
            playerScore = new UI_Text(GraphicsManager.REFERENCE_WINDOW_WIDTH / 2, GraphicsManager.REFERENCE_WINDOW_HEIGHT / 2 + 30, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, GameDataManager.highScoresList.latestScore.ToString());
        }

        //// Updates text of the message.
        //public void UpdateMessageText(string messageText)
        //{
        //    message_UI.SetText(messageText);
        //}

        //// Updates color of the message.
        //public void UpdateColor(Color color)
        //{
        //    message_UI.color = color;
        //}

        // Draws the message.
        public void Draw()
        {
            gameOverMenu.Draw_UI();
            playerScore.Draw_UI();
        }
    }
}
