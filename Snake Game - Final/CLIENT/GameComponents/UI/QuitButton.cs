using GameComponents;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI
{
    // Quit button menu item.
    public class QuitButton : MenuItem
    {
        // Constructor.
        public QuitButton(UI_Item uiItem) : base(uiItem) { }

        // Action to execute when button is initiated.  In this case, it closes the game.
        public override void ButtonAction()
        {
            GameHandle.game.Exit();
        }
    }
}
