using System.Collections.Generic;

namespace GameComponents
{
    // This stores all possible game states and manages what is the current game state.
    public static class SceneManager
    {
        // Holds all the game states.
        public static Dictionary<SceneLabel, Scene> scenes { get; private set; }

        // The current game state in use.
        public static Scene currentGameState { get; private set; }

        // Constructor.
        public static void Initialize()
        {
            scenes = new Dictionary<SceneLabel, Scene>();
        }

        // Adds a new game state to the game states dictonary.
        public static void AddScene(SceneLabel gameStateLabel, Scene scene)
        {
            scenes.Add(gameStateLabel, scene);
        }

        // Initializes the scene.  All scenes must register any CommandPackets attached to it.
        public static void InitializeScenes()
        {
            //? Register commands FIRST
            foreach (Scene scene in scenes.Values)
            {
                scene.RegisterCommands();
            }

            //? Register default inputs if no pre-existing map was loaded.
            if (!InputManager.loadedPreexistingInputMap)
            {
                foreach (Scene scene in scenes.Values)
                {
                    scene.RegisterDefaultKeys();
                }

                InputManager.inputMap_IO.SaveInputMap();
            }

            //? Intialize each scene..
            foreach (Scene scene in scenes.Values)
            {
                scene.Initialize();
            }
        }

        // Goes to one game state to another.  If no current game state exists, just enter into the input game state.
        public static void TransitionScene(SceneLabel sceneLabel)
        {
            // Check if a current scene is active.
            if (scenes.ContainsKey(sceneLabel) && currentGameState != null)
            {
                // Call exit scene and enter state methods in addition to current game state update.
                currentGameState.OnExitScene();
                currentGameState = scenes[sceneLabel];
                currentGameState.OnEnterScene();
            }
            else
            {
                // Only call enter state methods in addition to current game state update.
                currentGameState = scenes[sceneLabel];
                currentGameState.OnEnterScene();
            }
        }
    }
}
