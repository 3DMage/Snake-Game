using GameComponents;
using SnakeGame.GameComponents.UI;
using SnakeGame.GameComponents.UI.Menus;
using SnakeGame.GameComponents.UI.UI_Components;


namespace Client.GameComponents.UI
{

    // Transition button menu item.  Transitions between menus.
    public class TransitionButton_TutorialScreen : TransitionButton
    {
        // Reference to menu scene.
        Sc_Menu menuScene;

        // Target menu to go to.
        TutorialScreen target;

        Gamestate gamestate;

        // Constructor.
        public TransitionButton_TutorialScreen(UI_Item uiItem, Sc_Menu menuScene, TutorialScreen target, Gamestate gameState) : base(uiItem, menuScene, target, gameState)
        {
            this.gamestate = gameState;
            this.menuScene = menuScene;
            this.target = target;
        }

        // Action to execute when button is initiated.  In this case, it makes menu transition to the target menu.
        public override void ButtonAction()
        {
            menuScene.currentMenu = target;
            target.mappedKeysText.SetText("Mapped Keys: " + InputManager.inputMap.commandNameToKeyMap["Move Up"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Down"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Left"].Item2.ToString() + ", " + InputManager.inputMap.commandNameToKeyMap["Move Right"].Item2.ToString());
        }
    }
}


