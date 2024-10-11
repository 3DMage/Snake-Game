﻿using GameComponents;
using Microsoft.Xna.Framework.Input;
using Shared.Datastructures;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI
{
    // Represents an entry for input config.
    public class InputConfigEntry : MenuItem
    {
        // UI display for input config entry.
        public UI_TextPair ui_TextPair;

        // Context the input.
        public Context inputContext;

        // Key assigned to command.
        public Keys key;

        // Reference to main menu scene.
        public Sc_Menu menuScene { get; private set; }

        // Constructor.
        public InputConfigEntry(UI_TextPair ui_TextPair, Keys key, Context inputContext, Sc_Menu menuScene) : base(ui_TextPair)
        {
            this.ui_TextPair = ui_TextPair;
            this.key = key;
            this.ui_TextPair.rightText_UI.SetText(this.key.ToString());
            this.menuScene = menuScene;
            this.inputContext = inputContext;
        }

        // Action to execute when button is initiated.  In this case, it makes menu go into Input Config mode and updates menu display as needed.
        public override void ButtonAction()
        {
            menuScene.currentInputConfigEntry = this;
            menuScene.TransitionState(menuScene.inputconfig_State);
            menuScene.settingsMenu.UpdateInstructionMessage("Press a key to remap to it.  Press Esc to cancel.");
            // Change display to blank
            UpdateText("<Press a key.>");
        }

        // Update key display.
        public void UpdateKey(Keys key)
        {
            this.key = key;
            ui_TextPair.rightText_UI.SetText(key.ToString());
        }

        // Updates text on right side of the entry.
        public void UpdateText(string text)
        {
            ui_TextPair.rightText_UI.SetText(text);
        }

        // Marks an element with selection color.
        public override void MarkSelected()
        {
            ui_TextPair.rightText_UI.color = selectionColor;
        }

        // Marks an element with default color.
        public override void UnmarkSelected()
        {
            ui_TextPair.rightText_UI.color = defaultColor;
        }
    }
}
