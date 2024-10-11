using Client.GameComponents.UI;
using GameComponents;
using Microsoft.Xna.Framework;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Main menu for selecting other menus.
    public class MainMenu : Menu
    {
        // Constructor.
        public MainMenu(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {
            // Coordinates for menu components.
            float scalingFactor = 0.6f;
            int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);
            int positionY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.05f);
            int sizeX = (int)(GraphicsManager.titleLogo.Width * scalingFactor);
            int sizeY = (int)(GraphicsManager.titleLogo.Height * scalingFactor);

            // Header element.
            UI_Picture titleLogo = new UI_Picture(centerX, positionY, sizeX, sizeY, UI_Origin.UPPER_CENTER, GraphicsManager.titleLogo, Color.White);

            // Add non-interactive elements to menuDecor.
            menuDecor.Add(titleLogo);

            // Menu Items Y coordinates.
            int positionY_play = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.4f);
            int positionY_highScore = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.46f);
            int positionY_settings = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.52f);
            int positionY_credits = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.58f);
            int positionY_quit = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.64f);

            // Menu buttons.
            UI_Text playButton_UI = new UI_Text(centerX, positionY_play, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Play");
            UI_Text highScoreButton_UI = new UI_Text(centerX, positionY_highScore, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "High Score");
            UI_Text settingsButton_UI = new UI_Text(centerX, positionY_settings, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Settings");
            UI_Text creditsButton_UI = new UI_Text(centerX, positionY_credits, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Credits");
            UI_Text quitButton_UI = new UI_Text(centerX, positionY_quit, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, "Quit");

            // Add interactive elements to menu options.
            menuOptions.Add(new TransitionButton(playButton_UI, menuScene, menuScene.playerNameInput, menuScene.playerNameInput_State));
            menuOptions.Add(new TransitionButton_ScoreMenu(highScoreButton_UI, menuScene, menuScene.scoresMenu, menuScene.mainmenu_State));
            menuOptions.Add(new TransitionButton(settingsButton_UI, menuScene, menuScene.settingsMenu, menuScene.mainmenu_State));
            menuOptions.Add(new TransitionButton(creditsButton_UI, menuScene, menuScene.creditsMenu, menuScene.mainmenu_State));
            menuOptions.Add(new QuitButton(quitButton_UI));

            // Mark currently selected element.
            menuOptions[selectionIndex].MarkSelected();
        }
    }
}
