using Microsoft.Xna.Framework;

namespace GameComponents
{
 

    // The base class that represents a game state.
    public abstract class Scene
    {
        protected Gamestate currentGamestate;

        // Calls the current draw delegate method.
        public void Draw()
        {
            currentGamestate.renderMethod();
        }

        // Calls the current update delegate method.
        public void Update(GameTime gameTime)
        {
            currentGamestate.updateMethod(gameTime);
        }

        // Initialization.  Called when state added the GameStateManager.
        public abstract void Initialize();

        // Abstract method to register default keys when InputMap is not loaded via persistent storage.
        public abstract void RegisterDefaultKeys();

        // Abstract method to register scene CommandPackets.
        public abstract void RegisterCommands();

        // Updates state update and drawing methods, as well as input context and input mode.
        public void TransitionState(Gamestate gamestate)
        {
            currentGamestate = gamestate;
            InputManager.TransitionInputContext(gamestate.context);
            InputManager.ResetKeyboardState();
        }

        // Called when the scene is entered into.
        public virtual void OnEnterScene() { }

        // Called when the scene is exited.
        public virtual void OnExitScene() { }
    }
}