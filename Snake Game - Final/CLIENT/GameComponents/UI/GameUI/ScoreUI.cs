using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace Client.GameComponents.UI.GameUI
{
    public class ScoreUI
    {
        // The UI element holding the message text.
        UI_Picture scoreBoardFrame;
        public UI_Text playerScore;
        public UI_TextPair slot1Score;
        public UI_TextPair slot2Score;
        public UI_TextPair slot3Score;
        public UI_TextPair slot4Score;
        public UI_TextPair slot5Score;

        int slotXCoordinate = 1774;

        int slotYCoordinateStart = 215;
        int yOffset = 30;
        int gap = 50;

        // Constructor.
        public ScoreUI()
        {
            scoreBoardFrame = new UI_Picture(1734, 223, 327, 388, UI_Origin.CENTER, GraphicsManager.scoreBoardTexture, Color.White);
            playerScore = new UI_Text(1734, 124, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, GameDataManager.playerScore.ToString());

            int currentSlotYCoordinate = slotYCoordinateStart;
            slot1Score = new UI_TextPair(slotXCoordinate, currentSlotYCoordinate, UI_Origin.CENTER, GraphicsManager.calibriSmallFont, Color.White, gap, "", "");
            currentSlotYCoordinate += yOffset;

            slot2Score = new UI_TextPair(slotXCoordinate, currentSlotYCoordinate, UI_Origin.CENTER, GraphicsManager.calibriSmallFont, Color.White, gap, "", "");
            currentSlotYCoordinate += yOffset;

            slot3Score = new UI_TextPair(slotXCoordinate, currentSlotYCoordinate, UI_Origin.CENTER, GraphicsManager.calibriSmallFont, Color.White, gap, "", "");
            currentSlotYCoordinate += yOffset;

            slot4Score = new UI_TextPair(slotXCoordinate, currentSlotYCoordinate, UI_Origin.CENTER, GraphicsManager.calibriSmallFont, Color.White, gap, "", "");
            currentSlotYCoordinate += yOffset;

            slot5Score = new UI_TextPair(slotXCoordinate, currentSlotYCoordinate, UI_Origin.CENTER, GraphicsManager.calibriSmallFont, Color.White, gap, "", "");
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
            scoreBoardFrame.Draw_UI();
            playerScore.Draw_UI();
            slot1Score.Draw_UI();
            slot2Score.Draw_UI();
            slot3Score.Draw_UI();
            slot4Score.Draw_UI();
            slot5Score.Draw_UI();
        }
    }
}
