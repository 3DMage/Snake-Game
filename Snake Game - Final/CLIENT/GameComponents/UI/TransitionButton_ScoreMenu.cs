using GameComponents;
using SnakeGame.GameComponents.UI.Menus;
using SnakeGame.GameComponents.UI.UI_Components;
using SnakeGame.GameComponents.UI;

namespace Client.GameComponents.UI
{
    // Transition button menu item.  Transitions between menus.
    public class TransitionButton_ScoreMenu : TransitionButton
    {
        // Reference to menu scene.
        Sc_Menu menuScene;

        // Target menu to go to.
        ScoresMenu target;

        Gamestate gamestate;

        // Constructor.
        public TransitionButton_ScoreMenu(UI_Item uiItem, Sc_Menu menuScene, ScoresMenu target, Gamestate gameState) : base( uiItem,  menuScene,  target,  gameState)
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
            target.RefreshList();
        }
    }
}
