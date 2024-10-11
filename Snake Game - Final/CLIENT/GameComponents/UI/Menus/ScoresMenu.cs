using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu for displaying high scores.
    public class ScoresMenu : Menu
    {
        // Center X coordinate of screen.
        private int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);

        // The starting index within the menuDecor array for elements dealing with score entries.
        public int startingScoreItemIndex { get; private set; }

        // The index of menu item where the player is typing their name.
        public int currentScoreEnterIndex { get; set; } = -1;

        // The current UI_Text element to type player name on.
        public UI_Text currentScoreTextEnter { get; set; }

        // The instructions for telling the player to enter their name if they achieve a high score.
        public UI_Text playerNameInputInstructions { get; set; }

        // UI element to display latest score achieved from previous gameplay session.
        UI_Text latestScore_UI { get; set; }

        // Color to render playerNameInputInstructions text in.
        private Color instructionTextColor = new Color(115, 255, 33);

        // Toggle to draw the playerNameInputInstructions element.
        public bool drawInstructions { get; set; } = false;

        // Constructor.
        public ScoresMenu(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {
            // Menu coordinates.
            int leftX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.10f);
            centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);
            int bottomY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.9f);
            int header_positionY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.10f);

            // Header element.
            UI_Text headerLabel_UI = new UI_Text(centerX, header_positionY, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, "Your High Scores");

            // Add header element to menuDecor.
            menuDecor.Add(headerLabel_UI);

            // This is index to menuDecor to start adding score entries from.
            startingScoreItemIndex = menuDecor.Count;

            // Compute Y positions of all score entries.
            float Y_Offset = 0.18f;
            float Y_increment = 0.05f;

            // Add all score entries from high score list data.
            for (int i = 0; i < GameDataManager.highScoresList.scores.Count; i++)
            {
                int Y_position = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * Y_Offset);
                UI_TextPair currentEntry = new UI_TextPair(centerX, Y_position, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, -70, "", "");
                menuDecor.Add(currentEntry);
                Y_Offset += Y_increment;
            }

            // Y coordinate to the instructions element.
            int instructionsY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.65f);

            // Player name input instructions UI element.
            playerNameInputInstructions = new UI_Text(centerX, instructionsY, UI_Origin.CENTER, GraphicsManager.calibriFont, instructionTextColor, "High score achieved!  Type your name and press enter.");

            // Y coordinate to latest score UI element.
            int latestScoreY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.75f);

            // Latest score display UI element.
            latestScore_UI = new UI_Text(centerX, latestScoreY, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Latest Score: ");

            // Add the latest score UI element to menuDecor.
            menuDecor.Add(latestScore_UI);

            // Update text of all score entries.
            RefreshList();

            // Back button element.
            UI_Text backButton_UI = new UI_Text(leftX, bottomY, UI_Origin.CENTER_LEFT, GraphicsManager.calibriFont, Color.White, "Back");

            // Add interactive elements to menu options.
            menuOptions.Add(new TransitionButton(backButton_UI, menuScene, menuScene.mainMenu, menuScene.mainmenu_State));

            // Mark currently selected element.
            menuOptions[selectionIndex].MarkSelected();
        }

        // Draws the menu.
        public override void Draw()
        {
            base.Draw();

            // Only draw this if high score was achieved.
            if (drawInstructions)
            {
                playerNameInputInstructions.Draw_UI();
            }
        }

        // Updates text on all score entries.
        public void RefreshList()
        {
            latestScore_UI.SetText("Latest Score: " + GameDataManager.highScoresList.latestScore.ToString());

            int currentScoreItemIndex = startingScoreItemIndex;
            for (int i = 0; i < GameDataManager.highScoresList.scores.Count; i++)
            {
                UI_TextPair currentEntry = (UI_TextPair)menuDecor[currentScoreItemIndex];
                currentEntry.leftText_UI.SetText(GameDataManager.highScoresList.scores[i].name);
                currentEntry.rightText_UI.SetText(GameDataManager.highScoresList.scores[i].score.ToString());
                currentScoreItemIndex++;
            }
        }
    }
}
