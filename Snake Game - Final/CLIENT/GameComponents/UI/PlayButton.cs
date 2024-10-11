using GameComponents;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI
{
    // Play button menu item.
    public class PlayButton : MenuItem
    {
        // Constructor.
        public PlayButton(UI_Item uiItem) : base(uiItem) { }

        // Action to execute when button is initiated.
        public override void ButtonAction()
        {
            SceneManager.TransitionScene(SceneLabel.GAME);
        }
    }
}
