using GameComponents;
using SnakeGame.GameComponents.UI.Menus;
using SnakeGame.GameComponents.UI.UI_Components;

namespace SnakeGame.GameComponents.UI
{
    // Transition button menu item.  Transitions between menus.
    public class TransitionButton : MenuItem
    {
        // Reference to menu scene.
        Sc_Menu menuScene;

        // Target menu to go to.
        Menu target;

        Gamestate gamestate;

        // Constructor.
        public TransitionButton(UI_Item uiItem, Sc_Menu menuScene, Menu target, Gamestate gameState) : base(uiItem)
        {
            this.gamestate = gameState;
            this.menuScene = menuScene;
            this.target = target;
        }

        // Action to execute when button is initiated.  In this case, it makes menu transition to the target menu.
        public override void ButtonAction()
        {
            menuScene.TransitionState(gamestate);
            menuScene.currentMenu = target;
        }
    }
}
