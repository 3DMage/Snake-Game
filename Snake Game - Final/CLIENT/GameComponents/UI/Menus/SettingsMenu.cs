using GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Shared.Datastructures;
using SnakeGame.GameComponents.UI.UI_Components;
using System.Collections.Generic;

namespace SnakeGame.GameComponents.UI.Menus
{
    // Menu to config user input controls.
    public class SettingsMenu : Menu
    {
        // UI element displaying messages based on condition of input configuration.
        UI_Text statusMessage_UI;

        // UI element displaying instructions for input configuration.
        UI_Text instructionText_UI;

        // Initial message for instructionText_UI.
        string startingInstructions = "Press Enter to remap key.";

        // Constructor.
        public SettingsMenu(Sc_Menu menuScene) : base(menuScene) { }

        // Constructs the menu.
        public override void ConstructMenu()
        {
            // Menu coordinates.
            int leftX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.10f);
            int centerX = (int)(GraphicsManager.REFERENCE_WINDOW_WIDTH * 0.5f);
            int bottomY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.9f);
            int positionY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.10f);
            int positionY_2 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.75f);
            int positionY_3 = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * 0.79f);

            // Header element.
            UI_Text headerLabel_UI = new UI_Text(centerX, positionY, UI_Origin.CENTER, GraphicsManager.calibriHeaderFont, Color.White, "Settings");

            // Status display element.
            statusMessage_UI = new UI_Text(centerX, positionY_2, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.Red, "");

            // Instructions element.
            instructionText_UI = new UI_Text(centerX, positionY_3, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, startingInstructions);

            // Add non-interactive elements to menuDecor.
            menuDecor.Add(headerLabel_UI);
            menuDecor.Add(statusMessage_UI);
            menuDecor.Add(instructionText_UI);

            // Compute Y coordinates for each input config entry.
            float currentOffset = 0.3f;
            int currentY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * currentOffset);

            foreach (KeyValuePair<(Context, Keys), string> kvp in InputManager.inputMap.keyToCommandNameMap)
            {
                if (kvp.Key.Item1 == Context.GAME && kvp.Key.Item2 != Keys.Escape)
                {
                    UI_TextPair entry = new UI_TextPair(centerX, currentY, UI_Origin.CENTER, GraphicsManager.calibriFont, Color.White, 400, kvp.Value, kvp.Key.Item2.ToString());
                    menuOptions.Add(new InputConfigEntry(entry, kvp.Key.Item2, kvp.Key.Item1, menuScene));
                    currentOffset += 0.04f;
                    currentY = (int)(GraphicsManager.REFERENCE_WINDOW_HEIGHT * currentOffset);
                }
            }

            // Back button element.
            UI_Text backButton_UI = new UI_Text(leftX, bottomY, UI_Origin.CENTER_LEFT, GraphicsManager.calibriFont, Color.White, "Back");

            // Add interactive elements to menu options.
            menuOptions.Add(new TransitionButton(backButton_UI, menuScene, menuScene.mainMenu, menuScene.mainmenu_State));

            // Mark currently selected element.
            menuOptions[selectionIndex].MarkSelected();
        }

        // Updates the status message.
        public void UpdateStatusMessage(string text)
        {
            statusMessage_UI.SetText(text);
        }

        // Updates the instructions message.
        public void UpdateInstructionMessage(string text)
        {
            instructionText_UI.SetText(text);
        }

        // Clears the message.
        public void ClearMessages()
        {
            statusMessage_UI.SetText("");
            instructionText_UI.SetText(startingInstructions);
        }
    }
}
