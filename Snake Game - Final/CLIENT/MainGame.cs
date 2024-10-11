using Client.__GAME.SnakeGame.Components.SnakeComponents;
using Client.GameComponents.Network;
using Client.SimulatorStuff;
using GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Main
{
    // Main game class.
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        // Initialize foundational settings for game.
        public MainGame()
        {
            // Grab handles to allow state classes to access certain functionalities.
            ContentManagerHandle.Content = Content;
            GameHandle.game = this;

            // Initialize core components.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Initializtion method.
        protected override void Initialize()
        {

            // Initialize managers.
            SceneManager.Initialize();
            GraphicsManager.Initialize(graphicsDeviceManager, GraphicsDevice);
            InputManager.Initialize();
            AudioManager.Initialize();
            GameDataManager.Initialize();

            GameDataManager.TryLoadingScoresList();
            InputManager.TryLoadingInputMap();

            // Configure screen settings.
            GraphicsManager.ConfigureGraphicsDevice();
            ColorTable.Initialize();

            SimulatorManager.Initialize();

            _Client.Initialize(); 

            base.Initialize();
        }

        // Loading method.
        protected override void LoadContent()
        {
            // Load assets.
            GraphicsManager.LoadGraphicsAssets();
            AudioManager.LoadAudio();

            // Configure audio.
            AudioManager.ConfigureAudio();

            //?============ SET UP SCENES ===========================================
            SceneManager.AddScene(SceneLabel.MAIN_MENU, new Sc_Menu());
            SceneManager.AddScene(SceneLabel.GAME, new Sc_SnakeGame());
            SceneManager.InitializeScenes();
            //?============ SET UP SCENES ===========================================

            // Mark initial game state.
            SceneManager.TransitionScene(SceneLabel.MAIN_MENU);
        }

        // Update method.
        protected override void Update(GameTime gameTime)
        {
            // Process input and perform updates for current game state.
            SceneManager.currentGameState.Update(gameTime);

            base.Update(gameTime);
        }

        // Draw method.
        protected override void Draw(GameTime gameTime)
        {
            // Draw current game state into the render target.
            GraphicsManager.DrawToRenderTarget(true);
            SceneManager.currentGameState.Draw();

            // Draw render target.
            GraphicsManager.DrawToRenderTarget(false);
            GraphicsManager.graphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 1.0f, 0);
            GraphicsManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
            GraphicsManager.spriteBatch.Draw(GraphicsManager.renderTarget, GraphicsManager.renderScaleRectangle, Color.White);
            GraphicsManager.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}